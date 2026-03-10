using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public enum EnnemieType
{
    Elf,
    Dwarf,
    King,
    Demon,
    Goblin,
    Slime,
}

[System.Serializable] public struct EnnemieLink
{
    public float Weight;
    public SO_EnnemieStereotype Type;
}

[CreateAssetMenu(fileName = "SO_EnnemieStereotype", menuName = "Ennemie/EnnemieStereotype")]
public class SO_EnnemieStereotype : ScriptableObject
{
    public EnnemieType Type;

    public List<EnnemieLink> Links;

    [SerializeField] private int _menacePoint;
    
    [SerializeField] private float _atkRate;
    [SerializeField] private float _defRate;
    [SerializeField] private float _hpMax;
    
    

    public SO_EnnemieStereotype NextEnnemie()
    {
        if (Links.Count > 0)
        {
            float rng = Random.value * Links.Sum(l => l.Weight);
            int idx = Mathf.FloorToInt(rng * Links.Count);
            return Links[idx].Type;
        }

        return null;
    }
    
}
