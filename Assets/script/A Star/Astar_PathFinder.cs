using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar_PathFinder : MonoBehaviour
{
    [SerializeField] private Tilemap _map;

    [SerializeField] private BoundsInt _boundsint;
    
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;

    [SerializeField] private Tilemap _pathmap;
    [SerializeField] private TileBase _pathTile;

    private List<Vector3Int> _walkables = new List<Vector3Int>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _map.CompressBounds();
        var mapBounds = _map.cellBounds;
        var tiles = _map.GetTilesBlock(mapBounds);

        foreach (var pos in _map.cellBounds.allPositionsWithin)
        {
            if (_map.HasTile(pos))
            {
                _walkables.Add(pos);
            }
        }

        StartCoroutine(PathFinding());

    }

    IEnumerator PathFinding()
    {
        var path = AStarProcess.Process( _walkables, Vector3Int.FloorToInt(_start.position), Vector3Int.FloorToInt(_end.position));
        if (path.Length > 0)
        {
            _pathmap.ClearAllTiles();
            Utils.DrawMap(_pathmap, _pathTile, path);
        }
        yield return null;
    }
}
