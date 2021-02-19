using UnityEngine;
using UnityEngine.UI;

public static class TransitCalculations
{
    public struct TimeFromAcceleratingResult
    {
        public float accelerationTime;
        public float decelerationTime;
        public float totalTime;
    }

    public static TimeFromAcceleratingResult TimeFromAccelerating(
        float distance, float acceleration, float deceleration, float initialVelocity, float flipTime)
    {
        if (acceleration <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(acceleration));
        }

        if (deceleration <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(deceleration));
        }

        if (flipTime < 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(flipTime));
        }

        var result = new TimeFromAcceleratingResult();

        // solve for acceleration time using quadratic equation
        float a = (acceleration * acceleration) / (2f * deceleration);
        float b = initialVelocity + acceleration / 2f +
            initialVelocity * acceleration / deceleration + flipTime * acceleration;
        float c = (initialVelocity * initialVelocity) / (2f * deceleration) +
            flipTime * initialVelocity;
        result.accelerationTime = (-b + Mathf.Sqrt(b * b - 4f * a * c)) / (2f * a);

        // decceleration time
        result.decelerationTime = initialVelocity / acceleration + result.accelerationTime;

        // total time
        result.totalTime = result.accelerationTime + result.decelerationTime + flipTime;

        return result;
    }

    public struct TimeFromDeceleratingResult
    {
        public float timeToBeginDeceleration;
        public float decelerationTime;
        public float totalTime;
    }

    public static TimeFromDeceleratingResult TimeFromDeceleration(
        float distance, float deceleration, float currentVelocity)
    {
        if (deceleration <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(deceleration));
        }

        float v = currentVelocity;
        float d = distance;
        float a = -deceleration;

        float t1 = -v / a;
        float d1 = v * t1 + a * t1 * t1 / 2f;
        float d0 = d - d1;
        float t0 = d0 / v;

        var result = new TimeFromDeceleratingResult();
        result.timeToBeginDeceleration = t0;
        result.decelerationTime = t1;
        return result;
    }
}

public class TransitComputer : MonoBehaviour
{
    public enum TransitState
    {
        Idle,
        Stage0,
        Stage1
    }

    public ShipController ship;

    public float fudgeDistance = 10f;
    public float targetRadius = 30f;

    [Header("UI Elements")]
    public Text alignText;
    public Text distText;
    public Text speedText;
    public Text accText;
    public Text tBrkText;
    public Text estDistText;
    public GameObject stage0Light;
    public GameObject stage1Light;
    public GameObject throttleUpLight;
    public GameObject throttleOkLight;
    public GameObject throttleDownLight;
    public GameObject targetLockLight;

    private void Update()
    {
        UpdateDist();
        UpdateAlignment();
        UpdateAcc();
        UpdateSpeed();

        stage0Light.SetActive(State == TransitState.Stage0);
        stage1Light.SetActive(State == TransitState.Stage1);
        targetLockLight.SetActive(ship.target != null);

        throttleUpLight.SetActive(false);
        throttleDownLight.SetActive(false);
        throttleOkLight.SetActive(false);
        tBrkText.text = "";

        if (State == TransitState.Stage0)
        {
            UpdateStage0Text();
        }
        else if (State == TransitState.Stage1)
        {
            UpdateStage1Lights();
        }

    }

    public TransitState State
    {
        get
        {

            if (ship.target == null)
            {
                return TransitState.Idle;
            }

            var target = ship.target.Value;
            var isMoving = ship.Velocity.sqrMagnitude > 0f;

            if (isMoving)
            {
                if (ship.throttle > 0f &&
                    Vector3.Dot(ship.Heading, ship.Velocity) < 0f)
                {
                    return TransitState.Stage1;
                }
                else
                {
                    return TransitState.Stage0;
                }
            }
            else
            {
                return TransitState.Idle;
            }
        }
    }

    private void UpdateDist()
    {
        if (ship.target == null)
        {
            distText.text = "";
            return;
        }

        var distance = (ship.Position - ship.target.Value).magnitude;
        distText.text = FormatDistance(distance);
    }

    private void UpdateAlignment()
    {
        if (ship.target == null)
        {
            alignText.text = "";
            return;
        }

        var toTarget = ship.target.Value - ship.Position;
        var degrees = Vector3.Angle(ship.Heading, toTarget);
        alignText.text = degrees.ToString("0.0");
    }

    private void UpdateSpeed()
    {
        var speed = ship.Velocity.magnitude;

        if (speed == 0f)
        {
            speedText.text = "";
            return;
        }

        string formatted = speed.ToString(speed < 1000f ? "0.0" : "0");
        speedText.text = $"{formatted}m/s";
    }

    private void UpdateAcc()
    {
        var acc = ship.Acceleration;

        if (acc == 0f)
        {
            accText.text = "";
            return;
        }

        string formatted = acc.ToString(acc < 10f ? "0.00" : "0.0");
        accText.text = formatted;
    }

    private void UpdateTBrk()
    {
        if (State != TransitState.Stage0)
        {
            tBrkText.text = "";
        }

        var deceleration = ship.moveSpeed;
        var velocity = ship.Velocity.magnitude;
        var distance = (ship.Position - ship.target.Value).magnitude;
        var calculation = TransitCalculations.TimeFromDeceleration(distance, deceleration, velocity);
        tBrkText.text = FormatDuration(-calculation.timeToBeginDeceleration);
    }

    private void UpdateStage0Text()
    {
        float deceleration = ship.moveSpeed * 0.8f;
        float speed = ship.Velocity.magnitude;
        float distance = DistanceToSphereOrClosestPoint(
            ship.Position,
            ship.Velocity.normalized,
            ship.target.Value,
            targetRadius);
        var calculation = TransitCalculations.TimeFromDeceleration(distance, deceleration, speed);
        tBrkText.text = FormatDuration(-calculation.timeToBeginDeceleration);
    }

    private void UpdateStage1Lights()
    {
        Vector3 target = ship.target.Value;
        Vector3 position = ship.Position;
        Vector3 direction = ship.Velocity.normalized;
        float velocity = ship.Velocity.magnitude;
        float deceleration = ship.Acceleration;

        float decelerationTime = velocity / deceleration;
        float projectedDistance = velocity * decelerationTime / 2f;
        float idealDistance =
            DistanceToSphereOrClosestPoint(position, direction, target, targetRadius);

        if (projectedDistance < idealDistance - fudgeDistance)
        {
            throttleDownLight.SetActive(true);
        }
        else if (projectedDistance > idealDistance + fudgeDistance)
        {
            throttleUpLight.SetActive(true);
        }
        else
        {
            throttleOkLight.SetActive(true);
        }
    }

    private string FormatDuration(float seconds)
    {
        var isNegative = seconds < 0f;
        seconds = Mathf.Abs(seconds);

        int displayMinutes = Mathf.FloorToInt(seconds / 60);
        int displaySeconds = Mathf.RoundToInt(seconds % 60);
        return $"{(isNegative ? "-" : "")}{displayMinutes}:{displaySeconds.ToString("D2")}";
    }

    private string FormatDistance(float distance)
    {
        if (distance < 1000)
        {
            return $"{Mathf.Round(distance)}m";
        }
        else
        {
            return $"{Mathf.Round(distance / 100f) * 0.1f}km";
        }
    }

    // this is important because we want to navigate the player close, but not TOO close, to their target
    private float DistanceToSphereOrClosestPoint(
        Vector3 origin, Vector3 direction, Vector3 point, float radius)
    {
        var sphereDistance = DistanceToSphereIntersection(origin, direction, point, radius);

        if (sphereDistance == null)
        {
            return DistanceToClosestPoint(origin, direction, point);
        }

        return sphereDistance.Value;
    }

    private float DistanceToClosestPoint(Vector3 origin, Vector3 direction, Vector3 point)
    {
        return Vector3.Dot(point - origin, direction);
    }

    private float? DistanceToSphereIntersection(
        Vector3 origin, Vector3 direction, Vector3 point, float radius)
    {
        var tc = DistanceToClosestPoint(origin, direction, point);
        var d = Mathf.Sqrt((point - origin).sqrMagnitude - tc * tc);

        if (d > radius)
            return null;

        var t1c = Mathf.Sqrt(radius * radius);
        return tc - t1c;
    }
}
