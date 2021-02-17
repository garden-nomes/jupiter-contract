using UnityEngine;

public class Helm : MonoBehaviour, IInteractible
{
    public ShipController ship;
    public float rotationSpeed = 90f;
    public float throttleSpeed = .5f;

    private PlayerController controllingPlayer;

    private void Update()
    {
        if (controllingPlayer == null) return;

        if (controllingPlayer.input.GetBtnDown(2))
        {
            ReleasePlayer();
            return;
        }

        controllingPlayer.instructionTextOverride = GetInstructionText();

        var horizontal = controllingPlayer.input.horizontal;
        ship.transform.rotation *=
            Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.back);
        var vertical = controllingPlayer.input.vertical;
        ship.transform.rotation *=
            Quaternion.AngleAxis(vertical * Time.deltaTime * rotationSpeed, Vector3.left);

        if (controllingPlayer.input.GetBtn(0))
        {
            ship.throttle -= throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
        }

        if (controllingPlayer.input.GetBtn(1))
        {
            ship.throttle += throttleSpeed * Time.deltaTime;
            ship.throttle = Mathf.Clamp01(ship.throttle);
        }
    }

    public void Interact(PlayerController player)
    {
        LockPlayer(player);
    }

    public string GetActionText(PlayerController player)
    {
        return "take helm";
    }

    public bool CanInteract()
    {
        return controllingPlayer == null;
    }

    private string GetInstructionText()
    {
        if (controllingPlayer == null) return "";

        var scheme = controllingPlayer.input.inputScheme;
        return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} rotate ship\n" +
            (ship.throttle > 0 ? $"{Icons.IconText(scheme.btn0)} throttle down\n" : "") +
            (ship.throttle < 1 ? $"{Icons.IconText(scheme.btn1)} throttle up\n" : "") +
            $"{Icons.IconText(scheme.btn2)} cancel";
    }

    private void LockPlayer(PlayerController player)
    {
        player.hasControl = false;
        player.instructionTextOverride = GetInstructionText();

        // delay a frame to avoid releasing control if this Update() happens afterwards in same frame
        StartCoroutine(Helpers.DelayedAction(() =>
        {
            controllingPlayer = player;
        }));
    }

    private void ReleasePlayer()
    {
        var player = controllingPlayer;

        // delay a frame to avoid taking control back if this Update() happens afterwards in same frame
        StartCoroutine(Helpers.DelayedAction(() =>
        {
            player.hasControl = true;
        }));

        controllingPlayer.instructionTextOverride = null;
        controllingPlayer = null;
    }
}
