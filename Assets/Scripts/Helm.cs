using UnityEngine;

public class Helm : StationBehaviour
{
    public ShipController ship;
    public Autobrake autobrake;
    public float rotationSpeed = 90f;
    public float throttleSpeed = .5f;

    protected override void UseStation(PlayerController player)
    {
        if (autobrake.IsEngaged)
        {
            if (player.input.GetBtnDown(1))
            {
                sfx.Blip();
                autobrake.Disengage();
            }
        }
        else
        {
            // rotate ship
            var horizontal = player.input.horizontal;
            ship.transform.rotation *=
                Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.back);
            var vertical = player.input.vertical;
            ship.transform.rotation *=
                Quaternion.AngleAxis(vertical * Time.deltaTime * rotationSpeed, Vector3.left);

            // throttle up/down
            if (player.input.GetBtn(0)) ship.throttle += throttleSpeed * Time.deltaTime;
            else ship.throttle -= throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);

            if (ship.throttle == 0f && ship.Velocity.sqrMagnitude > 0f && player.input.GetBtnDown(1))
            {
                sfx.Blip();
                autobrake.Engage();
            }
        }
    }

    protected override void Update()
    {
        if (!HasControl && !autobrake.IsEngaged)
        {
            ship.throttle -= throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
        }

        base.Update();
    }

    public override string GetActionText(PlayerController player)
    {
        return "take helm";
    }

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;

        if (autobrake.IsEngaged)
        {
            return $"{Icons.IconText(scheme.btn1)} disengage autobrake\n" +
                $"{Icons.IconText(scheme.btn2)} back";
        }
        else
        {
            bool canEngageAutobrake = ship.throttle == 0f && ship.Velocity.sqrMagnitude > 0f;

            return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} rotate ship\n" +
                $"{Icons.IconText(scheme.btn0)} (hold) throttle\n" +
                (canEngageAutobrake ? $"{Icons.IconText(scheme.btn1)} engage autobrake\n" : "") +
                $"{Icons.IconText(scheme.btn2)} back";
        }
    }
}
