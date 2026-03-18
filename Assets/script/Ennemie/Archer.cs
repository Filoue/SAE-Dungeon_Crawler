using System;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private AnimationCurveInspector _test;
    [SerializeField] private float _input;
    [SerializeField] private float _score;


    private void Update()
    {
        _score  = _animationCurve.Evaluate(_input);        
    }
}
