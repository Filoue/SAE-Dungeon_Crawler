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

        List<Vector3Int> aiWalkables = walkables.Where(w => GetWalkableNeighbours(walkables, w) >= 8).ToList();

        if (walkables.Contains(start) && walkables.Contains(end))
        {
            // Basics stuff -------------
            // path.Add(start);
            // path.Add(end);

            List<AStarPoint> openPoints = new List<AStarPoint>();
            
            openPoints.Add(new AStarPoint(start, 0, Vector3Int.Distance(start, end), null));

            List<Vector3Int> closePoint = new List<Vector3Int>();
            
            do
            {
                // BFS pathing
                AStarPoint currentPoint = openPoints.OrderBy(p => p.F).First();
                openPoints.Remove(currentPoint);
                
                // We found the end
                if (currentPoint.Pos == end)
                {
                    Debug.Log("J'ai trouver je vous jure il existe");

                    GetPath(currentPoint, path);
                    return /* j'en sais trop rien */ null;
                }
                

                
                // Check neighbours
                foreach (var neighbour in Utils.MooreDirections.OrderBy(_ => Random.value))
                {
                    Vector3Int pos = currentPoint.Pos + neighbour;
                    
                    // Add a point if
                    // - the point is in the map
                    // - the point is not already checked
                    if (walkables.Contains(pos) && !closePoint.Contains(pos))
                    {
                        float newG = currentPoint.G + Vector3Int.Distance(pos, currentPoint.Pos);
                        float newH = Vector3Int.Distance(pos, end);

                        var existingPoint = openPoints.FirstOrDefault(p => p.Pos == pos);
                        
                        if (existingPoint == null)
                        {
                            openPoints.Add(new AStarPoint(pos, newG, newH, currentPoint));
                        }
                        else if(existingPoint.F > newH + newG)
                        {
                            existingPoint.G = newG;
                            existingPoint.H = newH;

                            existingPoint.Parent = currentPoint;
                        }
                        
                        // Check for points already checked
                        closePoint.Add(pos);
                    }
                }
            } while (openPoints.Count > 0 && closePoint.Count > walkables.Count);
        }
        
        return path.ToArray();
    }

    private static void GetPath(AStarPoint pathPoint, List<Vector3Int> path)
    {
        path.Add(pathPoint.Pos);
        
        if (pathPoint.Parent != null) GetPath(pathPoint.Parent, path);
    }

    private static int GetWalkableNeighbours(List<Vector3Int> walkable, Vector3Int position)
    {
        int neigbourCount = 0;
        foreach (var neighbour in Utils.MooreDirections);
        {
            //if (walkable.Contains(position + neighbour)) neigbourCount++;
        }

        return 0;
    }
}
