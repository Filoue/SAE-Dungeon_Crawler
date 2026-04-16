using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [Header("Detector")]
    [SerializeField] private Detector _detector;
    [SerializeField] private Detector _detector2;
    
    [Header("munition")] [SerializeField] private GameObject _munition;
    private IEnumerator FShoot;
    [SerializeField] private float _shootingSpeed = 3f;
    

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(_detector.detect)  _rb.linearVelocity = Vector2.zero;
        
        if (_detector2.detect)
        {
           
            if (FShoot == null)
            {
                FShoot = CanShoot();
                StartCoroutine(FShoot);
            }
        }
        else
        {
            if (FShoot != null)
            {
                StopCoroutine(FShoot);
                FShoot = null;
            }
        }
    }

    private IEnumerator CanShoot()
    {
        while (_detector2.detect)
        {
            Shoot();
            yield return new WaitForSeconds(_shootingSpeed);
        }
    }

    private void Shoot()
    {
       Instantiate(_munition, transform.position, Quaternion.identity);
    }
}
