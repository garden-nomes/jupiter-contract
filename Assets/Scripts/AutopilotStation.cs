using UnityEngine;
using UnityEngine.UI;

public static class Format
{
    public static string Duration(float seconds)
    {
        var isNegative = seconds < 0f;
        seconds = Mathf.Abs(seconds);

        int displayMinutes = Mathf.FloorToInt(seconds / 60);
        int displaySeconds = Mathf.RoundToInt(seconds % 60);
        return $"{(isNegative ? "-" : "")}{displayMinutes}:{displaySeconds.ToString("D2")}";
    }

    public static string Distance(float distance)
    {
        if (distance < 1000)
        {
            return $"{distance.ToString("0")}m";
        }
        else
        {
            return $"{(distance / 1000f).ToString("0.00")}km";
        }
    }
}

public class AutopilotStation : StationBehaviour
{
    public Autopilot autopilot;
    public ShipController shipController;

    public Text estimatedTime;
    public GameObject orientationLight;
    public GameObject acceleratingLight;
    public GameObject flipLight;
    public GameObject deceleratingLight;
    public GameObject arrivedLight;

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;
        return (CanEngage() ? $"{Icons.IconText(scheme.btn0)} engage\n" : "") +
            $"{Icons.IconText(scheme.btn2)} back";
    }

    protected override void UseStation(PlayerController player)
    {
        if (CanEngage() && player.input.GetBtnDown(0))
        {
            autopilot.Engage(shipController.target.Value);
        }

        if (autopilot.State != null)
        {
            estimatedTime.text = Format.Duration(-autopilot.transitTimer);
        }
        else if (shipController.target != null)
        {
            var time = autopilot.CalculateTransitTime(shipController.target.Value);
            estimatedTime.text = Format.Duration(-time);
        }
        else
        {
            estimatedTime.text = "";
        }

        orientationLight.SetActive(autopilot.State == Autopilot.AutopilotState.Orienting);
        acceleratingLight.SetActive(autopilot.State == Autopilot.AutopilotState.Accelerating);
        flipLight.SetActive(autopilot.State == Autopilot.AutopilotState.Flip);
        deceleratingLight.SetActive(autopilot.State == Autopilot.AutopilotState.Decelerating);
        arrivedLight.SetActive(autopilot.State == Autopilot.AutopilotState.Arrived);
    }

    private bool CanEngage()
    {
        return shipController.target != null && shipController.Velocity.sqrMagnitude == 0f;
    }
}
