
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CutType
{
    Horizontal,
    Vertical
}

public struct BspNode
{
    public BoundsInt Room;
    public CutType CutType;
    
}


public class BspTree
{

    public static  List<BoundsInt> Generate(BoundsInt firstroom, float minCutRatio, float maxCutRatio)
    {

        List<BoundsInt> rooms = new List<BoundsInt>();
        CutType cutType = CutType.Horizontal;

        float SurfaceCriteria = 10f;
        int MaxIteraations = 50;
        int idxIteration = 0;
        
        Queue<BspNode> cutsQueue = new Queue<BspNode>();
        
        cutsQueue.Enqueue(new BspNode{Room = firstroom, CutType = cutType});
        
        
        do
        {
            if (++idxIteration < MaxIteraations) break;
                
                BspNode roomToCut = cutsQueue.Dequeue();
                
                if((roomToCut.Room.size.x * roomToCut.Room.size.y) > 10)
                {
                    
                    float cutRatio = Random.Range(Mathf.Min(minCutRatio), Mathf.Max(maxCutRatio));
                    
                    if (cutType == CutType.Horizontal) cutType = CutType.Vertical;            
                    if (cutType == CutType.Vertical) cutType = CutType.Horizontal;
                    
                    switch (roomToCut.CutType)
                    {
                        case CutType.Horizontal:
                            BoundsInt[] cutRoomsH = CutVertical(roomToCut, cutRatio);
                            cutsQueue.AddRange(cutRoomsH);
                            break;
                        
                        case CutType.Vertical:
                            BoundsInt[] cutRoomsV = CutHoritontal(roomToCut, cutRatio);
                            cutsQueue.AddRange(cutRoomsV);
                            break;
                    }
                }
                else
                {
                    rooms.Add(roomToCut);
                }

            
        } while (cutsQueue.Count > 0);
        
        return rooms;
    }
    
    private static BoundsInt[] CutVertical(BoundsInt room, float cutRatio)
    {

        
        BoundsInt roomA = new BoundsInt(room.min,  
            new Vector3Int(Mathf.RoundToInt(room.min.x * cutRatio), room.size.y, room.size.z));
        BoundsInt roomB = new BoundsInt(
            new Vector3Int(roomA.max.x, roomA.min.y, roomA.min.z),  
            new Vector3Int(Mathf.RoundToInt(room.min.x * (1 - cutRatio)), room.size.y, room.size.z));

        return new []{ roomA, roomB };
    }    
    private static BoundsInt[] CutHorizontal(BoundsInt room, float cutRatio)
    {

        new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y * cutRatio), room.size.z);
        
        BoundsInt roomA = new BoundsInt(room.min, new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y * cutRatio), room.size.z));
        BoundsInt roomB = new BoundsInt(
            new Vector3Int(roomA.min.x, roomA.max.y, roomA.min.z), new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y *
                (1 - cutRatio)), room.size.z));

        return new []{ roomA, roomB };
    }

}
