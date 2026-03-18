using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCG
{

    public enum CutType
    {
        Horizontal,
        Vertical
    }

    public class BspNode
    {
        public BoundsInt Bounds;
        public CutType CutType;
        // public RectInt area;
        // public BspNode leftchild;
        // public BspNode rightchild;
        // public Vector2Int Center => new Vector2Int((int)Bounds.center.x, (int)Bounds.center.y);
    }

    public class BspTree
    {

        public static List<BoundsInt> Generate(BoundsInt firstRoom, float minCutRatio, float maxCutRatio, float surfaceCriteria)
        {
            List<BoundsInt> rooms = new List<BoundsInt>();
            CutType cutType = Random.value > 0.5f ? CutType.Horizontal : CutType.Vertical;

            Queue<BspNode> cutsQueue = new Queue<BspNode>();
            cutsQueue.Enqueue(new BspNode{Bounds = firstRoom, CutType = cutType});
            
            do
            {
                BspNode roomToCut = cutsQueue.Dequeue();

                if ((roomToCut.Bounds.size.x * roomToCut.Bounds.size.y) > surfaceCriteria)
                {
                    
                    float cutRatio = Random.Range(Mathf.Min(minCutRatio, maxCutRatio), Mathf.Max(minCutRatio, maxCutRatio));

                    BoundsInt[] boundsCutResult = new BoundsInt[2];
                    CutType cutTypeResult = CutType.Horizontal;
                    switch (roomToCut.CutType)
                    {
                        case CutType.Horizontal:
                            boundsCutResult = CutVertical(roomToCut.Bounds, cutRatio);
                            cutTypeResult = CutType.Vertical;
                            break;
                        case CutType.Vertical:
                            boundsCutResult = CutHorizontal(roomToCut.Bounds, cutRatio);
                            cutTypeResult = CutType.Horizontal;
                            break;
                    }
                    foreach (BoundsInt boundsInt in boundsCutResult)
                    {
                        cutsQueue.Enqueue(new BspNode{Bounds = boundsInt, CutType = cutTypeResult});
                    }
                }
                else
                {
                    rooms.Add(roomToCut.Bounds);
                }
                
            } while (cutsQueue.Count > 0);
            
            return rooms;
        }

        private static BoundsInt[] CutVertical(BoundsInt room, float cutRatio)
        {
            
            BoundsInt roomA = new BoundsInt(
                room.min,
                new Vector3Int(Mathf.RoundToInt(room.size.x * cutRatio), room.size.y, room.size.z)
            );
            BoundsInt roomB = new BoundsInt(
                new Vector3Int(roomA.max.x, roomA.min.y, roomA.min.z),
                new Vector3Int(Mathf.RoundToInt(room.size.x * (1 - cutRatio)), room.size.y, room.size.z)
            );

            return new[] { roomA, roomB };

        }

        private static BoundsInt[] CutHorizontal(BoundsInt room, float cutRatio)
        {

            BoundsInt roomA = new BoundsInt(
                room.min,
                new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y * cutRatio), room.size.z)
            );
            BoundsInt roomB = new BoundsInt(
                new Vector3Int(roomA.min.x, roomA.max.y, roomA.min.z),
                new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y * (1 - cutRatio)), room.size.z)
            );

            return new[] { roomA, roomB };

        }


    }
}
