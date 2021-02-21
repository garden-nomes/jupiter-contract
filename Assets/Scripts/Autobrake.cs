using System.Collections;
using UnityEngine;

public class Autobrake : MonoBehaviour
{
    enum BrakingStage
    {
        Idle,
        Flip,
        Decelerating,
        Disengage,
    }

    public bool IsEngaged => stage != BrakingStage.Idle;
    public float BrakingDistance => estimatedDistance;

    public ShipController ship;
    public Helm helm;

    private BrakingStage stage = BrakingStage.Idle;
    private float estimatedDistance = 0f;
    private float currentEngagementEstimate = 0f;
    private Vector3 currentEngagementStart;

    void Start()
    {
        StartCoroutine(EstimateDistanceCoroutine());
    }

    public void Engage()
    {
        stage = BrakingStage.Flip;
        currentEngagementEstimate = EstimateDistance();
        currentEngagementStart = ship.Position;
    }

    public void Disengage()
    {
        stage = BrakingStage.Disengage;
    }

    private void FixedUpdate()
    {
        if (stage == BrakingStage.Flip)
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

            if (!IsFacing(ship.Velocity.normalized * -1f) || ship.Velocity.magnitude <= beginThrottleDownSpeed)
            {
                stage = BrakingStage.Disengage;
            }
            else if (ship.throttle < 1f)
            {
                ship.throttle += helm.throttleSpeed * Time.deltaTime;
                if (ship.throttle > 1f) ship.throttle = 1f;
            }
        }
        else if (stage == BrakingStage.Disengage)
        {
            ship.throttle -= helm.throttleSpeed * Time.deltaTime;

            if (ship.throttle <= 0f)
            {
                if (ship.Velocity.magnitude < 0.1f)
                {
                    ship.Stop();
                    var travelled = (currentEngagementStart - ship.Position).magnitude;
                    Debug.Log($"estimate: {currentEngagementEstimate.ToString("0.00")}");
                    Debug.Log($"travelled: {currentEngagementEstimate.ToString("0.00")}");
                }

                ship.throttle = 0f;
                stage = BrakingStage.Idle;
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
        return Vector3.Angle(ship.Heading, direction) < 0.01f;
    }

    private float EstimateDistance()
    {
        float velocity = ship.Velocity.magnitude;

        if (velocity == 0f)
        {
            return 0f;
        }

        float deltaAcceleration = helm.throttleSpeed * ship.moveSpeed;
        float maxAcceleration = ship.moveSpeed;
        return FlipDistance(velocity) + DecelerationDistance(velocity, deltaAcceleration, maxAcceleration);
    }

    private float FlipDistance(float velocity)
    {
        var targetDirection = ship.Velocity.normalized * -1;
        float angle = Vector3.Angle(ship.Heading, targetDirection);
        var flipTime = angle / helm.rotationSpeed;
        return velocity * flipTime;
    }

    private float DecelerationDistance(float velocity, float deltaA, float maxA)
    {
        // Is this the most efficient way to calculate this? Hell no. Is it the last day of a game
        // jam and I don't have time to pull out my calculus textbook? You guessed it.

        float a = 0f;
        float v = velocity;
        float d = 0f;
        float step = 1f / 60f;

        while (v > 0f)
        {
            float beginThrottleDownSpeed = a * a / (2f * deltaA);

            if (v > beginThrottleDownSpeed)
            {
                a += deltaA * step;
                if (a > maxA) a = maxA;
            }
            else
            {
                a -= deltaA * step;
                if (a < 0f) a = 0f;
            }

            v -= a * step;
            d += v * step;
        }

        return d;
    }

    private IEnumerator EstimateDistanceCoroutine()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.25f);
            estimatedDistance = EstimateDistance();
        }
    }
}
