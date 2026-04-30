using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;


public class Wapons_Manager : MonoBehaviour
{
    [Header("Armes Dispo")] [Tooltip("tout les arme jouable")] [SerializeField]
    private Weapons[] _weapons;
    
    
    [SerializeField] private Animator _animator;

    private float _attackCooldown;
    private Weapons _currentWeapons;
    private string animatorWeaponsParam = "wapons";

    public bool attackInput;

    private float weaponsIndexInput;
    
    
    private void Start()
    {
        EquipWeapon(EquipedWeapon.Sword);
    }

    private void EquipWeapon(EquipedWeapon type)
    {
        _currentWeapons = System.Array.Find(_weapons, w => w.weapon == type);

        if (_currentWeapons == null)
        {
            Debug.LogWarning($"Aucun WeaponData trouvé pour {type}");
            return;
        }
    }

    private void Update()
    {
        _attackCooldown -= Time.deltaTime;
        
        _animator.SetInteger(animatorWeaponsParam, (int)_currentWeapons.weapon);

        if (attackInput && _attackCooldown <= 0f)
        {
            _attackCooldown = _currentWeapons.attackSpeed;
            
            Debug.Log("Attack");
            _animator.SetBool("Attack", true);
        }
        else
        {
            _animator.SetBool("Attack", false);
        }
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        attackInput = ctx.ReadValueAsButton();
    }


    private int choice;
    public void ChangeWeapons(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<float>());
        weaponsIndexInput = ctx.ReadValue<float>();

        choice += (int)weaponsIndexInput;

        choice = choice switch
        {
            > 4 => 0,
            < 0 => 4,
            _ => choice
        };

        _animator.SetLayerWeight(1, _currentWeapons.weapon == EquipedWeapon.Axe ? 1 : 0);
        _animator.SetLayerWeight(2, _currentWeapons.weapon == EquipedWeapon.Hammer ? 1 : 0);
        _animator.SetLayerWeight(3, _currentWeapons.weapon == EquipedWeapon.Sword ? 1 : 0);
        _animator.SetLayerWeight(4, _currentWeapons.weapon == EquipedWeapon.PickAxe ? 1 : 0);

        
        _currentWeapons = _weapons[choice];
    }
}
