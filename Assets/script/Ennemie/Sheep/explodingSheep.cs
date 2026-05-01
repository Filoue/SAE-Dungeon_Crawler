using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class explodingSheep : MonoBehaviour
{
// 1. Assign your "WoolExplosion" prefab here in the Inspector
    public ParticleSystem particles;

    public Detector detect;
    // Optional: Add a health system if you want
    public int health = 1;

    private AudioSource source;

    private IEnumerator Fexplode;
    
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (detect.detect)
        {
            if(Fexplode == null)
            {
                source.Play();
                Fexplode = Explode();
                StartCoroutine(Fexplode);
            }
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3.5f);
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
