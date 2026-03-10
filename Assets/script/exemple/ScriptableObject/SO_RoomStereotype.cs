using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RoomType
{
    Fight,
    Shop,
    Boss,
    End,
    Pool,
    Secret
}

[System.Serializable] public struct RoomLink
{
    public float Weight;
    public SO_RoomStereotype State;
}

[CreateAssetMenu(fileName = "SO_RoomStereotype", menuName = "Room Service/RoomStereotype")]
public class SO_RoomStereotype : ScriptableObject
{
    public Vector2 Size;
    public RoomType Type;

    public List<RoomLink> Links;

    public SO_RoomStereotype NextRoom()
    {
        if (Links.Count > 0)
        {
            float rng = Random.value * Links.Sum(l => l.Weight);
            int idx = Mathf.FloorToInt(rng * Links.Count);
            return Links[idx].State;
        }
        return null;
    }
}
