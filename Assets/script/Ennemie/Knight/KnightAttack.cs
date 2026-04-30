using System;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Detector _detector;

    [SerializeField] private SpriteRenderer _sr;
    
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _rb.linearVelocity.magnitude);

        if (_rb.linearVelocity.magnitude > 0.1f)
        {
            _sr.flipX = true;
        }else if (_rb)
        {
            
        }


    }
}
