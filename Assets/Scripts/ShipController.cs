using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float throttle = 0;
    public float moveSpeed;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 forward = transform.rotation * Vector3.up;
        rigidbody.velocity += forward * throttle * Time.deltaTime;
    }
}
