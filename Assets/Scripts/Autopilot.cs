using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    public enum AutopilotState
    {
        Orienting,
        Accelerating,
        Flip,
        Decelerating,
        Arrived
    }

    public ShipController ship;
    public Helm helm;
    public float defaultThrottle = 0.5f;
    public float targetDistance = 45f;

    private AutopilotState? state;
    public AutopilotState? State => state;

    private float accelerationTimer;
    private Vector3 target;

    public float transitTimer = 0f;
    public float TransitTimer => transitTimer;

    void Update()
    {
        if (state != null)
        {
            if (state != Autopilot.AutopilotState.Arrived)
            {
                transitTimer -= Time.deltaTime;
            }

            if (state.Value == AutopilotState.Orienting)
            {
                var toTarget = (target - ship.Position).normalized;

                if (TurnTowards(toTarget))
                {
                    state = AutopilotState.Accelerating;
                    accelerationTimer = CalculateAccelerationTime(target);
                }
            }
            else if (state.Value == AutopilotState.Accelerating)
            {
                accelerationTimer -= Time.deltaTime;
                SetThrottle(accelerationTimer);
                if (accelerationTimer <= 0f)
                {
                    ship.throttle = 0f;
                    state = AutopilotState.Flip;
                }
            }
            else if (state.Value == AutopilotState.Flip)
            {
                var brakingDirection = ship.Velocity.normalized * -1f;

                if (TurnTowards(brakingDirection))
                {
                    state = AutopilotState.Decelerating;
                    accelerationTimer = CalculateDecelerationTime(ship.Velocity.magnitude);
                }
            }
            else if (state.Value == AutopilotState.Decelerating)
            {
                accelerationTimer -= Time.deltaTime;
                SetThrottle(accelerationTimer);
                if (accelerationTimer <= 0f)
                {
                    state = AutopilotState.Arrived;
                    ship.throttle = 0f;
                    ship.ActivateStabilizers();
                }
            }
        }
    }

    public void Engage(Vector3 target)
    {
        if (ship.Velocity.sqrMagnitude == 0f)
        {
            state = AutopilotState.Orienting;
            this.target = target;
            transitTimer = CalculateTransitTime(target);
        }
    }

    public void Disengage()
    {
        state = null;
    }

    public float CalculateTransitTime(Vector3 target)
    {
        // calculate orientation time
        var toTarget = (target - ship.Position).normalized;
        float angle = Vector3.Angle(ship.Heading, toTarget);
        float orientationTime = angle / helm.rotationSpeed;

        float acceleratingTime = CalculateAccelerationTime(target);
        float flipTime = 180f / helm.rotationSpeed;

        float maxVelocity = acceleratingTime * ship.moveSpeed * defaultThrottle;
        float deceleratingTime = CalculateDecelerationTime(maxVelocity);

        return orientationTime + acceleratingTime + flipTime + deceleratingTime;
    }

    private float CalculateAccelerationTime(Vector3 target)
    {
        float a = ship.moveSpeed * defaultThrottle; // acceleration
        float ft = 180f / helm.rotationSpeed; // flip time
        float d = (target - ship.Position).magnitude - targetDistance; // distance

        // quadratic equation
        return (-ft * a + Mathf.Sqrt(ft * ft * a * a - 4f * a * -d)) / (2f * a);
    }

    private float CalculateDecelerationTime(float velocity)
    {
        float a = ship.moveSpeed * defaultThrottle;
        return velocity / a;
    }

    private bool TurnTowards(Vector3 targetDirection)
    {
        var toRotation = Quaternion.FromToRotation(Vector3.up, targetDirection);
        var fromRotation = ship.transform.rotation;

        ship.transform.rotation = Quaternion.RotateTowards(
            fromRotation, toRotation, helm.rotationSpeed * Time.deltaTime);

        return ship.Heading == targetDirection;
    }

    private void SetThrottle(float timer)
    {
        var throttleTime = defaultThrottle / helm.throttleSpeed;

        if (timer > throttleTime)
        {
            ship.throttle += helm.throttleSpeed * Time.deltaTime;
            if (ship.throttle > defaultThrottle) ship.throttle = defaultThrottle;
        }
        else
        {
            ship.throttle -= helm.throttleSpeed * Time.deltaTime;
            if (ship.throttle < 0f) ship.throttle = 0f;
        }
    }
}
