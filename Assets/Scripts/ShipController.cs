using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float throttle = 0;
    public float moveSpeed;
    public Vector3? target;
    public Vector3? hoveredTarget;

    public float ore = 0f;
    public float capacity = 1000f;

    public EngineController portEngine;
    public EngineController stbdEngine;

    public Shake shake;
    public SfxController sfx;

    public float stabilizerMaxSpeed = 1f;
    public float stabilizerForce = 1f;

    public Vector3 Heading => transform.up;
    public Vector3 Position => transform.position;
    public Vector3 Velocity => rigidbody.velocity;

    private new Rigidbody rigidbody;

    private float acceleration = 0f;
    public float Acceleration => acceleration;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        shake.Add(Acceleration);
        sfx.SetEngineRumble(Acceleration / moveSpeed);
    }

    void FixedUpdate()
    {
        // apply throttle
        portEngine.throttle = throttle;
        stbdEngine.throttle = throttle;
        var thrust = (portEngine.Thrust + stbdEngine.Thrust) / 2f;
        acceleration = thrust * moveSpeed;
        rigidbody.velocity += transform.up * acceleration * Time.deltaTime;
    }

    public void Stop()
    {
        rigidbody.velocity = Vector3.zero;
    }
}
