using UnityEngine;

public interface IInteractible
{
    bool CanInteract();
    void Interact(PlayerController player);
}

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

        var horizontal = controllingPlayer.input.horizontal;
        ship.transform.rotation *=
            Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.up);
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

    public bool CanInteract()
    {
        return controllingPlayer == null;
    }

    private void LockPlayer(PlayerController player)
    {
        controllingPlayer = player;
        controllingPlayer.hasControl = false;
    }

    private void ReleasePlayer()
    {
        var player = controllingPlayer;

        StartCoroutine(Helpers.DelayedAction(() =>
        {
            player.hasControl = true;
        }));

        controllingPlayer = null;
    }
}
