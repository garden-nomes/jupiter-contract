using UnityEngine;

public class Autobrake : MonoBehaviour
{
    enum BrakingStage
    {
        Disengaged,
        ThrottleDown,
        Flip,
        Decelerating,
    }

    public bool IsEngaged => stage != BrakingStage.Disengaged;
    public float BrakingDistance => 100f;

    public ShipController ship;
    public Helm helm;

    private BrakingStage stage = BrakingStage.Disengaged;

    public void Engage()
    {
        stage = BrakingStage.ThrottleDown;
    }

    public void Disengage()
    {
        stage = BrakingStage.Disengaged;
    }

    private void FixedUpdate()
    {
        if (stage == BrakingStage.ThrottleDown)
        {
            ship.throttle -= helm.throttleSpeed * Time.deltaTime;
            if (ship.throttle <= 0f)
            {
                ship.throttle = 0f;
                stage = BrakingStage.Flip;
            }
        }
        else if (stage == BrakingStage.Flip)
        {
            var targetDirection = ship.Velocity.normalized * -1f;

            TurnTowards(targetDirection);
            if (IsFacing(targetDirection))
            {
                stage = BrakingStage.Decelerating;
            }
        }
        else if (stage == BrakingStage.Decelerating)
        {
            float deltaA = helm.throttleSpeed * ship.moveSpeed;
            float a = ship.throttle * ship.moveSpeed;
            float beginThrottleDownSpeed = a * a / (2f * deltaA);

            if (IsFacing(ship.Velocity.normalized) && ship.throttle < 0.1f)
            {
                ship.throttle = 0f;
                ship.KillVelocity();
                stage = BrakingStage.Disengaged;
            }
            else if (!IsFacing(ship.Velocity.normalized * -1f))
            {
                stage = BrakingStage.ThrottleDown;
            }
            else if (ship.Velocity.magnitude <= beginThrottleDownSpeed)
            {
                ship.throttle -= helm.throttleSpeed * Time.deltaTime;
                if (ship.throttle <= 0f) ship.throttle = 0f;
            }
            else if (ship.throttle < 1f)
            {
                ship.throttle += helm.throttleSpeed * Time.deltaTime;
                if (ship.throttle > 1f) ship.throttle = 1f;
            }

        }
    }

    private void TurnTowards(Vector3 targetDirection)
    {
        var toRotation = Quaternion.FromToRotation(Vector3.up, targetDirection);
        var fromRotation = ship.transform.rotation;

        ship.transform.rotation = Quaternion.RotateTowards(
            fromRotation, toRotation, helm.rotationSpeed * Time.deltaTime);
    }

    private bool IsFacing(Vector3 direction)
    {
        return 1f - Vector3.Dot(ship.Heading, direction) < float.Epsilon;
    }
}
