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

    public EngineController portEngine;
    public EngineController stbdEngine;

    public float stabilizerMaxSpeed = 1f;
    public float stabilizerForce = 1f;

    public Vector3 Heading => transform.up;
    public Vector3 Position => transform.position;
    public Vector3 Velocity => rigidbody.velocity;

    private new Rigidbody rigidbody;

    private bool isStabilizing = false;
    public bool IsStabilizing => isStabilizing;

    private bool isMining = false;
    public bool IsMining => isMining;

    private float acceleration = 0f;
    public float Acceleration => acceleration;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // apply throttle
        portEngine.throttle = throttle;
        stbdEngine.throttle = throttle;
        var thrust = (portEngine.Thrust + stbdEngine.Thrust) / 2f;
        acceleration = thrust * moveSpeed;
        rigidbody.velocity += transform.up * acceleration * Time.deltaTime;

        // apply stabilizers
        var speed = Velocity.magnitude;
        isStabilizing = Acceleration == 0f && speed > 0f && speed <= stabilizerMaxSpeed;
        if (isStabilizing)
        {
            var force = stabilizerForce * Time.deltaTime;

            if (force >= speed)
                rigidbody.velocity = Vector3.zero;
            else
                rigidbody.velocity -= rigidbody.velocity.normalized * force;
        }
    }

    void Update()
    {
        // mine asteroids
        isMining = false;
        if (rigidbody.velocity.sqrMagnitude == 0f)
        {
            var asteroids = GameObject.FindObjectsOfType<Asteroid>();
            foreach (var asteroid in asteroids)
            {
                var distance = (asteroid.transform.position - transform.position).magnitude;
                if (distance <= miningDistance)
                {
                    isMining = true;
                    asteroid.ore -= miningSpeed * Time.deltaTime;

                    if (asteroid.ore <= 0f)
                    {
                        GameObject.Destroy(asteroid.gameObject);
                    }
                }
            }
        }
    }
}
