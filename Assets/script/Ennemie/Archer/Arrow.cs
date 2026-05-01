using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _force = 5;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField]private HealthSystem _healthSystem;
    

    private GameObject _player;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
        _healthSystem = FindAnyObjectByType<HealthSystem>();
        Vector2 direction = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddRelativeForce(direction * _force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _healthSystem.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
