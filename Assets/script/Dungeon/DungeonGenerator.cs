using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Size")]
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _startPos;

    [Header("BSP Min Max")] 
    [SerializeField][Range(0.1f, 1f)] private float _minCutRatio;
    [SerializeField][Range(0.1f, 1f)] private float _maxCutRatio;
    
    [Header("Tilemap")]
    [SerializeField] private float _threshold;
    
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    
    private List<BoundsInt> _rooms = new List<BoundsInt>();
    
    void Start()
    {
        // List<Vector2Int> generatedPositions = Generated();
        // DrawMap(_tilemap, _tile, generatedPositions);

        Vector2Int corner = _startPos - _size / 2;
        BoundsInt originalRoom = new BoundsInt(new Vector3Int(corner.x , corner.y, 1), new Vector3Int(_size.x, _size.y, 1));
        
        
        _rooms = BspTree.Generate(originalRoom, _minCutRatio, _maxCutRatio);
        
        _tilemap.ClearAllTiles();
        foreach (BoundsInt room in _rooms)
        {
            DrawMap(_tilemap, _tile, room);
        }
    }

    private void OnDrawGizmos()
    {
        
        foreach (BoundsInt room in _rooms)
        {
            Gizmos.color = Color.navyBlue;
            Gizmos.DrawWireCube(room.center, room.size);
        }
    }

    private List<Vector2Int> Generated()
    {
        List<Vector2Int> position = new List<Vector2Int>();
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                float perlinValue = Mathf.PerlinNoise(x / (float)_size.x ,y / (float)_size.y);
                if (perlinValue > _threshold)
                {
                    position.Add(new Vector2Int(_startPos.x + x, _startPos.y + y));
                }
            }
        }
        return position;
    }

    private void DrawMap(Tilemap map, TileBase tile, List<Vector2Int> generatedPositions)
    {
        foreach (Vector2Int position in generatedPositions)
        {
            _tilemap.SetTile(new Vector3Int(position.x, position.y, 0), _tile);
        }
    }

    private void DrawMap(Tilemap map, TileBase tile, BoundsInt position)
    {
        foreach (var pos in position.allPositionsWithin)
        {
            _tilemap.SetTile(pos, tile);
        }
    }

    void Update()
    {

    }
}
