using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public LayerMask mask;
    public float rayon;
    public bool detect = false;
    
    // Update is called once per frame
    void Update()
    {
        detect = Physics2D.OverlapCircle(transform.position, rayon, mask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (!detect)
        {
            Gizmos.color = Color.red;
        }
        
        Gizmos.DrawWireSphere(transform.position, rayon);
        
    }
}
