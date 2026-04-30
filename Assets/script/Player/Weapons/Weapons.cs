using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    public EquipedWeapon weapon;
    public float attackSpeed;
    public float damage;
}
