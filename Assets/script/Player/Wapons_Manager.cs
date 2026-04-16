using System;
using UnityEngine;
using UnityEngine.Serialization;


public class Wapons_Manager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    
    public EquipedWapons Wapons;
    private void Update()
    {
        switch (Wapons)
        {
            case EquipedWapons.None:
                _animator.SetBool("HasWapons", false);
                break;
            case EquipedWapons.Axe:
                _animator.SetBool("HasWapons", true);
                
                _animator.SetBool("Axe", true);
                _animator.SetBool("Hammer", false);
                _animator.SetBool("PickAxe", false);
                _animator.SetBool("Sword", false);
                break;
            case EquipedWapons.Hammer:
                _animator.SetBool("HasWapons", true);
                
                _animator.SetBool("Axe", false);
                _animator.SetBool("Hammer", true);
                _animator.SetBool("PickAxe", false);
                _animator.SetBool("Sword", false);
                break;
            case EquipedWapons.Sword:
                _animator.SetBool("HasWapons", true);
                
                _animator.SetBool("Axe", false);
                _animator.SetBool("Hammer", false);
                _animator.SetBool("PickAxe", false);
                _animator.SetBool("Sword", true);
                break;
            case EquipedWapons.PickAxe:
                _animator.SetBool("HasWapons", true);
                
                _animator.SetBool("Axe", false);
                _animator.SetBool("Hammer", false);
                _animator.SetBool("PickAxe", true);
                _animator.SetBool("Sword", false);
                break;
        }
    }
}
