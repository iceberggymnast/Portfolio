using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
     Vector3 _previousPosition;
     Vector3 _velocity;

    void Start()
    {
        _previousPosition = transform.position;
    }

    void Update()
    {
        _velocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }


}
