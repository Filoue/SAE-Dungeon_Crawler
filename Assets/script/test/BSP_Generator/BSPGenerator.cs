using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BSPGenerator : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;
    
    [Header("Dungeon Settings")]
    public int dungeonWidth = 50;
    public int dungeonHeight = 50;
    public int minLeafSize = 10;
    public int maxLeafSize = 20;

    [Header("Corridor Settings")]
    [Range(1, 5)] public int corridorWidth = 3;
    
    [Header("Tilemap References")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap; 

    [Header("Tile References")]
    public TileBase wallTile;
    public TileBase roomFloorTile;
    public TileBase corridorFloorTile;
    public TileBase startRoomTile;
    public TileBase bossRoomTile;

    [Header("Mob Spawning")] 
    public GameObject roomTriggerPrefab;
    public GameObject bossRoomTrigger;
    public GameObject[] enemyPrefabs;
    public GameObject[] BossPrefabs;
    

    // 0 = Wall, 1 = Room Floor, 2 = Corridor, 3 = Start, 4 = Boss
    private int[,] dungeonGrid; 
    private List<Leaf> leaves = new List<Leaf>();
    private List<RectInt> rooms = new List<RectInt>();
    
    private RectInt _startRoom;
    private RectInt _bossRoom;

    void Start()
    {
        GeminiGenerate();
    }

    public void GeminiGenerate()
    {
        GenerateDungeon();
        DrawDungeonTilemap();
    }

    public void GenerateDungeon()
    {
        dungeonGrid = new int[dungeonWidth, dungeonHeight];
        leaves.Clear();
        rooms.Clear();

        Leaf root = new Leaf(0, 0, dungeonWidth, dungeonHeight);
        leaves.Add(root);

        bool didSplit = true;
        
        while (didSplit)
        {
            didSplit = false;
            List<Leaf> currentLeaves = new List<Leaf>(leaves);
            foreach (Leaf leaf in currentLeaves)
            {
                if (leaf.leftChild == null && leaf.rightChild == null) 
                {
                    if (leaf.width > maxLeafSize || leaf.height > maxLeafSize || Random.value > 0.25f)
                    {
                        if (leaf.Split(minLeafSize))
                        {
                            leaves.Add(leaf.leftChild);
                            leaves.Add(leaf.rightChild);
                            didSplit = true;
                        }
                    }
                }
            }
        }

        root.CreateRooms(this);
        AssignStartAndBossRooms();
        
        CreateRoomTriggers();
    }

    private void AssignStartAndBossRooms()
    {
        if (rooms.Count < 2) return;

        // Find the Biggest Leaf to host the Boss
        Leaf biggestLeaf = leaves[0];
        float maxArea = 0;

        foreach (var leaf in leaves)
        {
            // Only consider "terminal" leaves (those that actually contain a room)
            if (leaf.leftChild == null && leaf.rightChild == null)
            {
                float area = leaf.width * leaf.height;
                if (area > maxArea)
                {
                    maxArea = area;
                    biggestLeaf = leaf;
                }
            }
        }

        RectInt bossRoom = biggestLeaf.room;

        // Find the room furthest away from the Boss Room to be the Start
        RectInt startRoom = rooms[0];
        float maxDistance = 0;

        foreach (var room in rooms)
        {
            if (room == bossRoom) continue; // Don't start in the boss room!

            float dist = Vector2.Distance(room.center, bossRoom.center);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                startRoom = room;
            }
        }

        // Mark the grid for the Tilemap logic
        // We mark the center tile of the rooms
        dungeonGrid[(int)startRoom.center.x, (int)startRoom.center.y] = 3; // Start
        dungeonGrid[(int)bossRoom.center.x, (int)bossRoom.center.y] = 4;   // Boss
        _startRoom = startRoom;
        _bossRoom = bossRoom;
        playerPrefab.transform.position = startRoom.center;
        
        Debug.Log($"Boss Room set in the largest leaf ({biggestLeaf.width}x{biggestLeaf.height}) at {bossRoom.center}");
    }

    public void CarveRoom(RectInt room)
    {
        rooms.Add(room);
        for (int x = room.x; x < room.x + room.width; x++)
        {
            for (int y = room.y; y < room.y + room.height; y++)
            {
                dungeonGrid[x, y] = 1; 
            }
        }
    }

    public void CarveCorridor(Vector2Int start, Vector2Int end)
    {
        int x = start.x;
        int y = start.y;

        // Helper to carve a "thick" point
        void CarveThickPoint(int px, int py)
        {
            // Center the thickness so the path stays aligned with the room centers
            int offset = corridorWidth / 2;
            for (int i = -offset; i <= offset; i++)
            {
                for (int j = -offset; j <= offset; j++)
                {
                    int targetX = px + i;
                    int targetY = py + j;

                    // Stay within dungeon bounds
                    if (targetX >= 1 && targetX < dungeonWidth - 1 && targetY >= 1 && targetY < dungeonHeight - 1)
                    {
                        // Only overwrite walls (0), don't overwrite rooms (1) or special tiles (3, 4)
                        if (dungeonGrid[targetX, targetY] == 0) 
                        {
                            dungeonGrid[targetX, targetY] = 2; 
                        }
                    }
                }
            }
        }

        // Move along X first
        while (x != end.x)
        {
            CarveThickPoint(x, y);
            x += (int)Mathf.Sign(end.x - x);
        }
        // Then move along Y
        while (y != end.y)
        {
            CarveThickPoint(x, y);
            y += (int)Mathf.Sign(end.y - y);
        }
    }

    private void DrawDungeonTilemap()
    {
        // Clear old tiles in case you call this multiple times
        if (floorTilemap != null) floorTilemap.ClearAllTiles();
        if (wallTilemap != null) wallTilemap.ClearAllTiles();

        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Read our 2D array and place the corresponding tile
                switch (dungeonGrid[x, y])
                {
                    case 0: // Wall
                        if (wallTilemap != null && wallTile != null)
                            wallTilemap.SetTile(tilePosition, wallTile);
                        break;
                    case 1: // Room Floor
                        if (floorTilemap != null && roomFloorTile != null)
                            floorTilemap.SetTile(tilePosition, roomFloorTile);
                        break;
                    case 2: // Corridor
                        if (floorTilemap != null && corridorFloorTile != null)
                            floorTilemap.SetTile(tilePosition, corridorFloorTile);
                        break;
                    case 3: // Start Room Center
                        if (floorTilemap != null && startRoomTile != null)
                            floorTilemap.SetTile(tilePosition, startRoomTile);
                        break;
                    case 4: // Boss Room Center
                        if (floorTilemap != null && bossRoomTile != null)
                            floorTilemap.SetTile(tilePosition, bossRoomTile);
                        break;
                }
            }
        }
    }
    private void CreateRoomTriggers()
    {
        foreach (RectInt room in rooms)
        {
            if (room != _startRoom)
            {
                if(room != _bossRoom)
                {
                    GameObject triggerObj = new GameObject("RoomTrigger_" + room.center);
                    triggerObj.transform.position = new Vector3(room.center.x - 3, room.center.y + 2, 0);

                    BoxCollider2D collider = triggerObj.AddComponent<BoxCollider2D>();
                    collider.isTrigger = true;

                    collider.size = new Vector2(room.width - 0.5f, room.height - 0.5f);

                    RoomTrigger rt = triggerObj.AddComponent<RoomTrigger>();

                    rt.mobPrefabs = enemyPrefabs;
                    rt.Setup(room);
                }
                else
                {
                    GameObject triggerObj = new GameObject("BossTriggerRoom_" + room.center);
                    triggerObj.transform.position = new Vector3(room.center.x - 3, room.center.y + 2, 0);

                    BoxCollider2D collider = triggerObj.AddComponent<BoxCollider2D>();
                    collider.isTrigger = true;

                    collider.size = new Vector2(room.width - 0.5f, room.height - 0.5f);

                    BossRoomTrigger rt = triggerObj.AddComponent<BossRoomTrigger>();
                    rt.bossPrefabs = BossPrefabs;
                    rt.Setup(room);
                }
            }
        }
    }
}


public class Leaf
{
    public int x, y, width, height;
    public Leaf leftChild, rightChild;
    public RectInt room;

    public Leaf(int x, int y, int width, int height)
    {
        this.x = x; this.y = y;
        this.width = width; this.height = height;
    }

    public bool Split(int minLeafSize)
    {
        if (leftChild != null || rightChild != null) return false;

        bool splitHorizontally = Random.value > 0.5f;
        if (width > height && width / height >= 1.25) splitHorizontally = false;
        else if (height > width && height / width >= 1.25) splitHorizontally = true;

        int max = (splitHorizontally ? height : width) - minLeafSize;
        if (max <= minLeafSize) return false;

        int split = Random.Range(minLeafSize, max);

        if (splitHorizontally)
        {
            leftChild = new Leaf(x, y, width, split);
            rightChild = new Leaf(x, y + split, width, height - split);
        }
        else
        {
            leftChild = new Leaf(x, y, split, height);
            rightChild = new Leaf(x + split, y, width - split, height);
        }
        return true;
    }

    public void CreateRooms(BSPGenerator generator)
    {
        if (leftChild != null || rightChild != null)
        {
            if (leftChild != null) leftChild.CreateRooms(generator);
            if (rightChild != null) rightChild.CreateRooms(generator);

            if (leftChild != null && rightChild != null)
            {
                generator.CarveCorridor(leftChild.GetRoomCenter(), rightChild.GetRoomCenter());
            }
        }
        else
        {
            Vector2Int roomSize = new Vector2Int(Random.Range(3, width - 2), Random.Range(3, height - 2));
            Vector2Int roomPos = new Vector2Int(Random.Range(1, width - roomSize.x - 1), Random.Range(1, height - roomSize.y - 1));
            
            room = new RectInt(x + roomPos.x, y + roomPos.y, roomSize.x, roomSize.y);
            generator.CarveRoom(room);
        }
    }



    public Vector2Int GetRoomCenter()
    {
        if (leftChild != null && rightChild != null)
        {
            return leftChild.GetRoomCenter();
        }
        return new Vector2Int(room.x + room.width / 2, room.y + room.height / 2);
    }
}
