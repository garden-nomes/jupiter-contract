using UnityEngine;
using UnityEngine.UI;

public class TransitComputer : MonoBehaviour
{

    public ShipController ship;
    public Autobrake autobrake;

    [Header("UI Elements")]
    public Text alignText;
    public Text distText;
    public Text speedText;
    public Text accText;
    public Text brakingDistText;
    public GameObject targetLockLight;
    public GameObject autobrakeEngagedLight;
    public GameObject engFailureLight;

    public ThrottleMeter portThrottleMeter;
    public ThrottleMeter stbdThrottleMeter;

    private void Update()
    {
        UpdateDist();
        UpdateAlignment();
        UpdateAcc();
        UpdateSpeed();
        UpdateBrakingDistance();

        targetLockLight.SetActive(ship.target != null);
        autobrakeEngagedLight.SetActive(autobrake.IsEngaged);
        engFailureLight.SetActive(ship.portEngine.IsBroken || ship.stbdEngine.IsBroken);

        portThrottleMeter.value = ship.portEngine.IsBroken ? 0f : ship.portEngine.throttle;
        stbdThrottleMeter.value = ship.stbdEngine.IsBroken ? 0f : ship.stbdEngine.throttle;
    }

    private void UpdateDist()
    {
        if (ship.target == null)
        {
            distText.text = "";
            return;
        }

        var distance = (ship.Position - ship.target.Value).magnitude;
        distText.text = distance < 1000f ? distance.ToString("0.0") : distance.ToString("0");
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
        alignText.text = degrees.ToString("0.00");
    }

    private void UpdateSpeed()
    {
        var speed = ship.Velocity.magnitude;

        if (speed == 0f)
        {
            speedText.text = "";
            return;
        }

        string formatted = speed.ToString(speed < 10f ? "0.00" : "0.0");
        speedText.text = $"{formatted}";
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

    private void UpdateBrakingDistance()
    {
        var dist = autobrake.BrakingDistance;

        if (dist < 0.1f)
            brakingDistText.text = "";
        else
            brakingDistText.text = dist < 100f ? dist.ToString("0.0") : dist.ToString("0");
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
