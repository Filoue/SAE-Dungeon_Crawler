using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using PCG;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

struct DebugLine
{
	public Vector3 start;
	public Vector3 end;
}


public class DungeonGenerators : MonoBehaviour
{

	[SerializeField] private Vector2Int _size;
	[SerializeField] private Vector2Int _startPos;
	
	[SerializeField] private float _threshold = 0.5f;
	
	[SerializeField] private Tilemap _tilemap;
	[SerializeField] private TileBase _tile;
	[SerializeField] private TileBase _corridorTile;

	[SerializeField][Range(1, 10)] private int _corridorSize;

	[Header("BSP")]
	[SerializeField][Range(0.1f, 1f)] private float _minCutRatio;
	[SerializeField][Range(0.1f, 1f)] private float _maxCutRatio;
	[SerializeField] private int _maxSurface = 1250;
	[SerializeField] private float _shrinkFactor = 0.8f;
	
	private List<BoundsInt> _rooms = new List<BoundsInt>();
	private List<DebugLine> _debugLines = new List<DebugLine>();
	
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

	    // List<Vector2Int> generatedPositions = Generate();
	    // DrawMap(_tilemap, _tile, generatedPositions);

		Generator();
	    
    }

    public void Generator()
    {
	    Vector2Int corner = _startPos - _size / 2;
	    BoundsInt originalRoom = new BoundsInt(new Vector3Int(corner.x, corner.y, 1), new Vector3Int(_size.x, _size.y, 1));

	    _rooms = PCG.BspTree.Generate(originalRoom, _minCutRatio, _maxCutRatio, _maxSurface);
	    _rooms = ShrinkRooms(_rooms);
	    MakePaths(ref _rooms);
	    
	    _debugLines.Clear();
	    _tilemap.ClearAllTiles();
	    
	    
	    foreach (BoundsInt room in _rooms)
	    {
		    Debug.Log($"Room : {room.min} , {room.size}");
		    DrawMap(_tilemap, _tile, room);
	    }
	    ConnectRooms(_rooms);
    }
    
    
    private void MakePaths(ref List<BoundsInt> rooms)
    {
	    BoundsInt firstRoom = _rooms.OrderBy(r => Vector3.Distance(r.center, new Vector3(_startPos.x, _startPos.y, 0))).First();
    }


    private List<Vector2Int> Generate()
    {
	    List<Vector2Int> positions = new List<Vector2Int>();
	    
	    for(int x = 0; x < _size.x; x++)
	    {
		    for(int y =  0; y < _size.y; y++)
		    {
			    float perlinValue = Mathf.PerlinNoise(x / (float)_size.x, y / (float)_size.y);
			    if(perlinValue > _threshold)
			    {
				    positions.Add(new Vector2Int(_startPos.x + x, _startPos.y + y));
			    }
		    }
	    }
	    
	    return positions;
    }
    
    private void DrawMap(Tilemap map, TileBase tile, List<Vector2Int> generatedPositions)
    {
	    _tilemap.ClearAllTiles();
	    
	    foreach (Vector2Int position in generatedPositions)
	    {
		    _tilemap.SetTile(new Vector3Int(position.x, position.y, 0), _tile);    
	    }
    }
    private void DrawMap(Tilemap map, TileBase tile, BoundsInt positions)
    {
	    foreach (var position in positions.allPositionsWithin)
	    {
		    _tilemap.SetTile(position, _tile);    
	    }
    }

    private List<BoundsInt> ShrinkRooms(List<BoundsInt> rooms)
    {
	    List<BoundsInt> newRooms = new List<BoundsInt>();
	    
	    foreach (BoundsInt room in rooms)
	    {
		    Vector3Int newSize = Vector3Int.RoundToInt(new Vector3(room.size.x * _shrinkFactor, room.size.y * _shrinkFactor, 1));
		    Vector3Int newPosition = Vector3Int.RoundToInt(room.center - new Vector3(0.5f * newSize.x, 0.5f * newSize.y, 1));
		    
		    newRooms.Add(new BoundsInt(newPosition, newSize));
		    
	    }

	    return newRooms;
    }

    public void ConnectRooms(List<BoundsInt> rooms)
    {
	    for (int i = 0; i < rooms.Count - 1; i++)
	    {
		    Vector2Int start = GetCenter(rooms[i]);
		    Vector2Int end = GetCenter(rooms[i + 1]);
		    CreateCorrdior(start, end);
	    }
	    
    }

    private void CreateCorrdior(Vector2Int p1, Vector2Int p2)
    {
	    Vector2Int corner;
	    if (Random.Range(0, 2) == 0) {
		    corner = new Vector2Int(p2.x, p1.y);
		    // Horizontal then Vertical
		    DrawHorizontalLine(p1.x, p2.x, p1.y);
		    DrawVerticalLine(p1.y, p2.y, p2.x);
	    } else {
		    corner = new Vector2Int(p1.x, p2.y);
		    // Vertical then Horizontal
		    DrawVerticalLine(p1.y, p2.y, p1.x);
		    DrawHorizontalLine(p1.x, p2.x, p2.y);
	    }
	    
	    _debugLines.Add(new DebugLine
	    {
		    start = new Vector3(p1.x, p1.y, 0),
		    end = new Vector3(corner.x, corner.y, 0),
	    });

	    _debugLines.Add(new DebugLine
	    {
		    start = new Vector3(corner.x, corner.y, 0),
		    end = new Vector3(p2.x, p2.y, 0),
	    });
    }

    private void DrawVerticalLine(int yStart, int yEnd, int x)
    {
	    for (int y = Mathf.Min(yStart, yEnd); y <= Mathf.Max(yStart, yEnd); y++) 
	    {
			_tilemap.SetTile(new Vector3Int(x, y, 0), _corridorTile);
	    }
    }

    private void DrawHorizontalLine(int xStart, int xEnd, int y)
    {
	    for (int x = Mathf.Min(xStart, xEnd); x <= Mathf.Max(xStart, xEnd); x++) 
	    {
			_tilemap.SetTile(new Vector3Int(x, y, 0), _corridorTile);
	    }
    }

    private Vector2Int GetCenter(BoundsInt bounds)
    {
	    return new Vector2Int(
		    Mathf.RoundToInt(bounds.x + bounds.size.x / 2f),
		    Mathf.RoundToInt(bounds.y + bounds.size.y / 2f));
    }
    
    
    private void OnDrawGizmos()
    {
	    Gizmos.color = Color.red;
	    
	    foreach (BoundsInt room in _rooms)
	    { 
		    Gizmos.DrawWireCube(room.center, room.size);
	    }

	    if (_debugLines == null || _debugLines.Count == 0) return;

	    Gizmos.color = Color.yellow;
	    foreach (DebugLine line in _debugLines)
	    {
		    Gizmos.DrawLine(line.start, line.end);
		    Gizmos.DrawSphere(line.start, 0.5f);
	    }
	    
    }
}
