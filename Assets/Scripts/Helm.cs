using UnityEngine;

public class Helm : StationBehaviour
{
    public ShipController ship;
    public Autopilot autopilot;
    public float rotationSpeed = 90f;
    public float throttleSpeed = .5f;

    protected override void UseStation(PlayerController player)
    {
        var horizontal = player.input.horizontal;
        ship.transform.rotation *=
            Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.back);
        var vertical = player.input.vertical;
        ship.transform.rotation *=
            Quaternion.AngleAxis(vertical * Time.deltaTime * rotationSpeed, Vector3.left);

        if (horizontal != 0f || vertical != 0f)
        {
            autopilot.Disengage();
        }

        if (player.input.GetBtn(1))
        {
            ship.throttle -= throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
            autopilot.Disengage();
        }

        if (player.input.GetBtn(0))
        {
            ship.throttle += throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
            autopilot.Disengage();
        }

        if (ship.throttle == 0f && ship.Velocity.sqrMagnitude > 0f && player.input.GetBtnDown(1))
        {
            autopilot.Disengage();
            if (ship.IsStabilizing)
                ship.DeactivateStabilizers();
            else
                ship.ActivateStabilizers();
        }
    }

    public override string GetActionText(PlayerController player)
    {
        return "take helm";
    }

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;

        bool canActivateStabilizers =
            ship.throttle == 0f &&
            ship.Velocity.sqrMagnitude > 0f &&
            !ship.IsStabilizing;

        bool canDeactivateStabilizers =
            ship.throttle == 0f &&
            ship.Velocity.sqrMagnitude > 0f &&
            ship.IsStabilizing;

        return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} rotate ship\n" +
            (ship.throttle > 0 ? $"{Icons.IconText(scheme.btn1)} (hold) throttle down\n" : "") +
            (ship.throttle < 1 ? $"{Icons.IconText(scheme.btn0)} (hold) throttle up\n" : "") +
            (canActivateStabilizers ? $"{Icons.IconText(scheme.btn1)} activate stabilizers\n" : "") +
            (canDeactivateStabilizers ? $"{Icons.IconText(scheme.btn1)} deactivate stabilizers\n" : "") +
            $"{Icons.IconText(scheme.btn2)} cancel";
    }
}
