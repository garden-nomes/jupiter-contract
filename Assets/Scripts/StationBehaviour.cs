using UnityEngine;

public abstract class StationBehaviour : MonoBehaviour, IInteractible
{
    public Transform cameraPositionOverride;

    private PlayerController controllingPlayer;
    public PlayerController ControllingPlayer => controllingPlayer;
    public bool HasControl => controllingPlayer != null;

    public bool CanInteract() => controllingPlayer == null && CanUseStation();
    public abstract string GetActionText(PlayerController player);
    public virtual string GetInstructionText(PlayerController player)
    {
        return $"{Icons.IconText(player.input.inputScheme.btn2)} cancel";
    }

    protected virtual bool CanUseStation() => true;
    protected virtual void OnLock(PlayerController player) { }
    protected virtual void OnRelease(PlayerController player) { }
    protected virtual void UseStation(PlayerController player) { }

    protected virtual void Update()
    {
        if (controllingPlayer != null)
        {
            if (controllingPlayer.input.GetBtnDown(2))
            {
                ReleasePlayer();
                return;
            }

            UseStation(controllingPlayer);
        }
    }

    public void Interact(PlayerController player)
    {
        LockPlayer(player);
    }

    private void LockPlayer(PlayerController player)
    {
        OnLock(player);

        player.hasControl = false;
        // delay a frame to avoid releasing control if this Update() happens afterwards in same frame
        StartCoroutine(Helpers.DelayedAction(() =>
        {
            controllingPlayer = player;
            player.station = this;
        }));
    }

    private void ReleasePlayer()
    {
        OnRelease(controllingPlayer);

        // delay a frame to avoid taking control back if this Update() happens afterwards in same frame
        var player = controllingPlayer;
        StartCoroutine(Helpers.DelayedAction(() =>
        {
            player.hasControl = true;
        }));

        controllingPlayer = null;
        player.station = null;
    }
}
