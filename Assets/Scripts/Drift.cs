using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public float driftSpeed = 2f;

    private new Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var direction = new Vector2(Random.value - .5f, Random.value - .5f).normalized;
        rigidbody.velocity += direction * Time.deltaTime * driftSpeed;
    }
}
