using System.Linq;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    [SerializeField] private float _radius;
    
    [SerializeField] private LayerMask _layerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] objects = Physics.OverlapSphere(this.transform.position, _radius, _layerMask, QueryTriggerInteraction.Ignore);

       // var nearTarget = objects.OrderBy(o => Vector3.Distance(o.transform, this.transform.position));
    }
}
