using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float throttle = 0;
    public float moveSpeed;
    public Vector3? target;
    public float miningDistance;
    public float miningSpeed;

    public Vector3 Heading => transform.up;
    public Vector3 Position => transform.position;
    public Vector3 Velocity => rigidbody.velocity;

    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.velocity += transform.up * throttle * moveSpeed * Time.deltaTime;
    }

    void Update()
    {
        var asteroids = GameObject.FindObjectsOfType<Asteroid>();

        foreach (var asteroid in asteroids)
        {
            var distance = (asteroid.transform.position - transform.position).magnitude;
            if (distance <= miningDistance)
            {
                asteroid.ore -= miningSpeed * Time.deltaTime;

                if (asteroid.ore <= 0f)
                {
                    GameObject.Destroy(asteroid.gameObject);
                }
            }
        }
    }
}
