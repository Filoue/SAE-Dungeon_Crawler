using System.Collections.Generic;
using System.Linq;
using UnityEngine;


class AStarPoint
{
    public Vector3Int Pos;
    
    public float G; // Dijkstra
    public float H; // Heuristic
    public AStarPoint Parent = null;

    public AStarPoint(Vector3Int pos, float g, float h, AStarPoint parent)
    {
        Pos = pos;
        G = g;
        H = h;
        Parent = parent;

    }
    
    public float F => G + H; // Global Score
    

}

public static class AStarProcess
{
    public static Vector3Int[] Process(List<Vector3Int> walkables, Vector3Int start, Vector3Int end)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        if (walkables.Contains(start) && walkables.Contains(end))
        {
            // Basics stuff -------------
            path.Add(start);
            path.Add(end);

            Queue<AStarPoint> _pointQueue = new Queue<AStarPoint>();
            
            _pointQueue.Enqueue(new AStarPoint(start, 0, Vector3Int.Distance(start, end), null));

            List<Vector3Int> closePoint = new List<Vector3Int>();
            
            do
            {
                // BFS pathing
                AStarPoint currentPoint = _pointQueue.Dequeue();
                
                // We found the end
                if(currentPoint.Pos == end) break;
                
                // Check for points already checked
                closePoint.Add(currentPoint.Pos);
                
                // Check neighbours
                foreach (var neighbour in Utils.MooreDirections)
                {
                    Vector3Int pos = currentPoint.Pos + neighbour;
                    
                    // Add a point if
                    // - the point is in the map
                    // - the point is not already checked
                    if (walkables.Contains(pos) && !closePoint.Contains(pos))
                    {
                        float newG = currentPoint.G + Vector3Int.Distance(pos, currentPoint.Pos);
                        float newH = Vector3Int.Distance(pos, end);
                        
                        _pointQueue.Enqueue(new AStarPoint(pos, newG, newH, currentPoint));
                    }
                }
            } while (_pointQueue.Count > 0);
        }
        
        return path.ToArray();
    }
}
