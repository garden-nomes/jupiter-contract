using UnityEngine;

public class Helm : StationBehaviour
{
    public ShipController ship;
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

        if (player.input.GetBtn(0))
        {
            ship.throttle -= throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
        }

        if (player.input.GetBtn(1))
        {
            ship.throttle += throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
        }
    }

    public override string GetActionText(PlayerController player)
    {
        return "take helm";
    }

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;
        return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} rotate ship\n" +
            (ship.throttle > 0 ? $"{Icons.IconText(scheme.btn0)} throttle down\n" : "") +
            (ship.throttle < 1 ? $"{Icons.IconText(scheme.btn1)} throttle up\n" : "") +
            $"{Icons.IconText(scheme.btn2)} cancel";
    }
}
