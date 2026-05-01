using System;
using UnityEngine;
using UnityEngine.Events;


public class HealthSystem : MonoBehaviour
{
    [Header("Config")] 
    public int maxHearts = 3;
    private int currentHalfHearts;

    private GameObject _player;
    
    public UnityEvent onHealthChanged;

    private void Start()
    {
        currentHalfHearts = maxHearts * 2;

        _player = GameObject.FindGameObjectWithTag("Player");
        onHealthChanged.Invoke();
    }

    public void TakeDamage(int halfHearts)
    {
        currentHalfHearts = Mathf.Max(0, currentHalfHearts - halfHearts);
        onHealthChanged.Invoke();

        if (currentHalfHearts == 0) Die();
    }

    public void Heal(int halfHearts)
    {
        currentHalfHearts = Mathf.Min(maxHearts * 2, currentHalfHearts + halfHearts);
        onHealthChanged.Invoke();
    }

    public int GetCurrentHalfHearts() => currentHalfHearts;
    public int GetMaxHearts() => maxHearts;

    private void Die()
    {
        Debug.Log("you are dumb");
        _player.SetActive(false);
    }
}
