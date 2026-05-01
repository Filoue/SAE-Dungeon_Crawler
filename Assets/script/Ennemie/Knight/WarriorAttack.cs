using UnityEngine;

public class WarriorAttack : MonoBehaviour
{
    private HealthSystem _healthSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthSystem = FindAnyObjectByType<HealthSystem>();
    }

    public void Attack()
    {
        _healthSystem.TakeDamage(1);
    }
}
