using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class KnightSteering : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 10f;

    [SerializeField] private GameObject _player;

    private Rigidbody2D _rb;


    private void Start()
    {
        _player = GameObject.Find("Player");
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    private void Update()
    {
        Vector2 desiredVelocity = Vector2.zero;

        desiredVelocity += seek(_player.transform);

        Vector2 currentVelocity = _rb.linearVelocity;
        Vector2 steeringForce = desiredVelocity - currentVelocity;

        Vector2 result = currentVelocity + steeringForce * Time.deltaTime;

        _rb.linearVelocity = Vector2.ClampMagnitude(result, _maxSpeed);
    }

    private Vector2 seek(Transform seekTarget) => seekTarget.position - transform.position;
}
