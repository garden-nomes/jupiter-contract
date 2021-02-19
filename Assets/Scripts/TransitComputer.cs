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

    public Transform ship;
    public Helm helm;
    public Rigidbody shipRigidbody;
    public ShipController shipController;

    public float fudgeDistance = 10f;

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
        targetLockLight.SetActive(shipController.target != null);

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

            if (shipController.target == null)
            {
                return TransitState.Idle;
            }

            var target = shipController.target.Value;
            var isMoving = shipRigidbody.velocity.sqrMagnitude > 0f;

            if (isMoving)
            {
                if (shipController.throttle > 0f &&
                    Vector3.Dot(shipController.Heading, shipRigidbody.velocity) < 0f)
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
        if (shipController.target == null)
        {
            distText.text = "";
            return;
        }

        var distance = (shipController.Position - shipController.target.Value).magnitude;
        distText.text = FormatDistance(distance);
    }

    private void UpdateAlignment()
    {
        if (shipController.target == null)
        {
            alignText.text = "";
            return;
        }

        var toTarget = shipController.Position - shipController.target.Value;
        var degrees = Vector3.Angle(shipController.Heading, toTarget);
        alignText.text = degrees.ToString("0.0");
    }

    private void UpdateSpeed()
    {
        var speed = shipRigidbody.velocity.magnitude;

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
        var acc = shipController.Acceleration;

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

        var deceleration = shipController.moveSpeed;
        var velocity = shipRigidbody.velocity.magnitude;
        var distance = (ship.position - shipController.target.Value).magnitude;
        var calculation = TransitCalculations.TimeFromDeceleration(distance, deceleration, velocity);
        tBrkText.text = FormatDuration(-calculation.timeToBeginDeceleration);
    }

    private void UpdateStage0Text()
    {
        float deceleration = shipController.moveSpeed * 0.8f;
        float speed = shipRigidbody.velocity.magnitude;
        float distance = DistanceToClosestPoint(
            shipController.Position,
            shipRigidbody.velocity.normalized,
            shipController.target.Value);
        var calculation = TransitCalculations.TimeFromDeceleration(distance, deceleration, speed);
        tBrkText.text = FormatDuration(-calculation.timeToBeginDeceleration);
    }

    private void UpdateStage1Lights()
    {
        Vector3 target = shipController.target.Value;
        Vector3 position = ship.position;
        Vector3 direction = shipRigidbody.velocity.normalized;
        float velocity = shipRigidbody.velocity.magnitude;
        float deceleration = shipController.Acceleration;

        float decelerationTime = velocity / deceleration;
        float projectedDistance = velocity * decelerationTime / 2f;
        float idealDistance = DistanceToClosestPoint(position, direction, target);

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

    private float RayDistanceFromPoint(Ray ray, Vector3 point)
    {
        var toPoint = point - ray.origin;
        var tc = Vector3.Dot(toPoint, ray.direction);

        if (tc <= 0f)
        {
            return toPoint.magnitude;
        }

        return Mathf.Sqrt(toPoint.sqrMagnitude - tc * tc);
    }

    private float DistanceToClosestPoint(Vector3 origin, Vector3 direction, Vector3 point)
    {
        return Vector3.Dot(point - origin, direction);
    }
}
