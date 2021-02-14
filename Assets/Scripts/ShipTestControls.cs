using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTestControls : MonoBehaviour
{
    public float turnSpeed = 90;
    public float throttle = 0;
    public float throttleSpeed = 1;

    void Start() { }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            throttle = Mathf.Clamp01(throttle + throttleSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.X))
            throttle = Mathf.Clamp01(throttle - throttleSpeed * Time.deltaTime);

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        transform.rotation *= Quaternion.AngleAxis(horizontal * Time.deltaTime * turnSpeed, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(-vertical * Time.deltaTime * turnSpeed, Vector3.right);

        var rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.rotation * Vector3.up * throttle);
    }
}
