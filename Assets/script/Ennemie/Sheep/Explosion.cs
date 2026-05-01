using UnityEngine;

public class Explosion : MonoBehaviour
{

    private HealthSystem _healthSystem;

    private Detector _detector;

    private bool explo = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _detector = GetComponent<Detector>();
        _healthSystem = FindAnyObjectByType<HealthSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_detector.detect && explo == true)
        {
            explo = false;
            _healthSystem.TakeDamage(4);
        }
    }
}
