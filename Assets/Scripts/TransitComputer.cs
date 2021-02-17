using TMPro;
using UnityEngine;

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
        NoTarget,
        AdjustHeading,
        OffCourse,
        Ready,
        Accelerating,
        Drifting,
        Decelerating,
        Arrived
    }

    public Transform ship;
    public NavStation navStation;
    public Helm helm;
    public TextMeshProUGUI uiText;
    public Rigidbody shipRigidbody;
    public ShipController shipController;

    public float fudgeDistance = 10f;

    private float targetDeceleration;

    private void Start() { }

    private void Update()
    {
        Debug.Log(State);
        UpdateText();
    }

    public TransitState State
    {
        get
        {

            if (navStation.Target == null)
            {
                return TransitState.NoTarget;
            }

            var target = navStation.Target.Value;
            var isMoving = shipRigidbody.velocity.sqrMagnitude > 0f;

            if (isMoving)
            {
                if (CheckCourse())
                {
                    if (shipController.throttle > 0f)
                    {
                        if (Vector3.Dot(ship.up, shipRigidbody.velocity) > 0f)
                        {
                            return TransitState.Accelerating;
                        }
                        else
                        {
                            return TransitState.Decelerating;
                        }
                    }
                    else
                    {
                        return TransitState.Drifting;
                    }
                }
                else
                {
                    return TransitState.OffCourse;
                }
            }
            else
            {
                if ((ship.position - target).sqrMagnitude < fudgeDistance * fudgeDistance)
                {
                    return TransitState.Arrived;
                }
                else if (CheckHeading())
                {
                    return TransitState.Ready;
                }
                else
                {
                    return TransitState.AdjustHeading;
                }
            }
        }
    }

    private void UpdateText()
    {
        string text;
        switch (State)
        {
            case TransitState.NoTarget:
                text = NoTargetText();
                break;
            case TransitState.AdjustHeading:
                text = "adjust heading";
                break;
            case TransitState.OffCourse:
                text = "off course";
                break;
            case TransitState.Ready:
                text = "ready";
                break;
            case TransitState.Accelerating:
                text = AcceleratingText();
                break;
            case TransitState.Drifting:
                text = DriftingText();
                break;
            case TransitState.Decelerating:
                text = DecelerationText();
                break;
            case TransitState.Arrived:
                text = "arrived";
                break;
            default:
                text = "";
                break;
        }
        uiText.text = text;
    }

    private string NoTargetText()
    {
        var hovered = navStation.HoveredTarget;

        if (hovered != null)
        {
            var distance = (ship.position - hovered.Value).magnitude;
            var flipTime = 180f / helm.rotationSpeed;
            var acceleration = shipController.moveSpeed;
            var time = TransitCalculations.TimeFromAccelerating(
                distance, acceleration, acceleration, 0f, flipTime).totalTime;
            return $"min time: {FormatDuration(time)}";
        }

        return "";
    }

    private string AcceleratingText()
    {
        // var acceleration = shipController.moveSpeed * shipController.throttle;
        // var velocity = shipRigidbody.velocity.magnitude;
        // var distance = (ship.position - navStation.Target.Value).magnitude;
        // var flipTime = 180f / helm.rotationSpeed;
        // var calculation = TransitCalculations.TimeFromAccelerating(
        //     distance, acceleration, acceleration, velocity, flipTime);
        // return FormatDuration(-calculation.accelerationTime);

        var deceleration = shipController.moveSpeed;
        var velocity = shipRigidbody.velocity.magnitude;
        var distance = (ship.position - navStation.Target.Value).magnitude;
        var calculation = TransitCalculations.TimeFromDeceleration(distance, deceleration, velocity);
        return FormatDuration(-calculation.timeToBeginDeceleration);
    }
    private string DriftingText()
    {
        var deceleration = shipController.moveSpeed;
        var velocity = shipRigidbody.velocity.magnitude;
        var distance = (ship.position - navStation.Target.Value).magnitude;
        var calculation = TransitCalculations.TimeFromDeceleration(distance, deceleration, velocity);
        return FormatDuration(-calculation.timeToBeginDeceleration);
    }

    private string DecelerationText()
    {
        Vector3 target = navStation.Target.Value;
        Vector3 position = ship.position;
        Vector3 direction = shipRigidbody.velocity.normalized;
        float velocity = shipRigidbody.velocity.magnitude;
        float deceleration = shipController.moveSpeed * shipController.throttle;

        float decelerationTime = velocity / deceleration;
        float projectedDistance = velocity * decelerationTime / 2f;
        float idealDistance = DistanceToClosestPoint(position, direction, target);

        if (projectedDistance < idealDistance - fudgeDistance)
        {
            return "decrease throttle";
        }
        else if (projectedDistance > idealDistance + fudgeDistance)
        {
            return "increase throttle";
        }

        return FormatDuration(decelerationTime);
    }

    private string FormatDuration(float seconds)
    {
        var isNegative = seconds < 0f;
        seconds = Mathf.Abs(seconds);

        int displayMinutes = Mathf.FloorToInt(seconds / 60);
        int displaySeconds = Mathf.RoundToInt(seconds % 60);
        return $"{(isNegative ? "-" : "")}{displayMinutes}:{displaySeconds.ToString("D2")}";
    }

    private bool CheckCourse()
    {
        if (navStation.Target == null)
        {
            return false;
        }

        var ray = new Ray(ship.position, shipRigidbody.velocity.normalized);
        return RayDistanceFromPoint(ray, navStation.Target.Value) <= fudgeDistance;
    }

    private bool CheckHeading()
    {
        if (navStation.Target == null)
        {
            return false;
        }

        var ray = new Ray(ship.position, ship.up);
        return RayDistanceFromPoint(ray, navStation.Target.Value) <= fudgeDistance;
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
