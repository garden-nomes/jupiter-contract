using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float throttle = 0;
    public float moveSpeed;
    public Vector3? target;
    public Vector3? hoveredTarget;
    public Transform station;
    public OreTransferSequence oreTransferSequence;
    public HoldFullMessage holdFullMessage;

    public float ore = 0f;
    public float capacity = 1000f;
    public float stationRange = 80f;

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

    private float contractTime = 0f;
    private bool isContractTimerRunning = false;
    public float ContactTime => contractTime;
    private bool wasHoldFull;
    public bool IsInContractCompleteSequence => oreTransferSequence.IsRunning;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // engine rumble
        shake.Add(Acceleration);
        sfx.SetEngineRumble(Acceleration / moveSpeed);

        // track contract time
        if (contractTime == 0f && throttle > 0f) isContractTimerRunning = true;
        if (isContractTimerRunning) contractTime += Time.deltaTime;

        // check if station in range
        if (ore == capacity &&
            !oreTransferSequence.IsRunning &&
            Velocity.sqrMagnitude < 0.01f &&
            (station.position - Position).sqrMagnitude < stationRange * stationRange)
        {
            isContractTimerRunning = false;
            oreTransferSequence.StartSequence();
            target = null;
        }

        // flash "hold full" message
        if (!oreTransferSequence.IsRunning && !wasHoldFull && ore >= capacity)
        {
            holdFullMessage.Show();
            wasHoldFull = true;
            target = station.position;
        }
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

    public void ResetContract()
    {
        contractTime = 0f;
        isContractTimerRunning = false;
        wasHoldFull = false;
    }

    public void CancelPopup()
    {
        holdFullMessage.Cancel();
        oreTransferSequence.Cancel();
    }
}
