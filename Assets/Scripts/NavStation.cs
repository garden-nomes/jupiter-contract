using UnityEngine;

public class NavStation : MonoBehaviour, IInteractible
{
    public float rotationSpeed = 90f;
    public Camera scopesCamera;

    private PlayerController controllingPlayer;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = scopesCamera.transform.rotation;
    }

    private void Update()
    {
        if (controllingPlayer == null) return;

        if (controllingPlayer.input.GetBtnDown(2))
        {
            ReleasePlayer();
            return;
        }

        var horizontal = controllingPlayer.input.horizontal;
        scopesCamera.transform.rotation *=
            Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.up);
        var vertical = controllingPlayer.input.vertical;
        scopesCamera.transform.rotation *=
            Quaternion.AngleAxis(vertical * Time.deltaTime * rotationSpeed, Vector3.left);
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
