using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rate = 30f;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        transform.localRotation = initialRotation * Quaternion.AngleAxis(Time.time * rate, Vector3.up);
    }
}
