using UnityEngine;

public class NavStation : MonoBehaviour, IInteractible
{
    public float rotationSpeed = 90f;
    public Camera scopesCamera;
    public NavOverlay overlay;

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

        overlay.navTarget = FindHoveredNavTarget();
    }

    public void Interact(PlayerController player)
    {
        LockPlayer(player);
    }

    public bool CanInteract()
    {
        return controllingPlayer == null;
    }

    public string GetActionText(PlayerController player)
    {
        return "check nav array";
    }

    Transform FindHoveredNavTarget()
    {
        var targets = GameObject.FindGameObjectsWithTag("nav target");

        var forward = scopesCamera.transform.forward;
        var from = scopesCamera.transform.position;

        GameObject currentTarget = null;

        foreach (var target in targets)
        {
            if (currentTarget == null)
            {
                currentTarget = target;
                continue;
            }

            var currentDot = Vector3.Dot(forward, (currentTarget.transform.position - from).normalized);
            var targetDot = Vector3.Dot(forward, (target.transform.position - from).normalized);

            if (targetDot > currentDot)
            {
                currentTarget = target;
            }
        }

        return currentTarget == null ? null : currentTarget.transform;
    }

    private string GetInstructionText()
    {
        if (controllingPlayer == null) return "";

        var scheme = controllingPlayer.input.inputScheme;
        return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} rotate array\n" +
            $"{Icons.IconText(scheme.btn2)} cancel";
    }

    private void LockPlayer(PlayerController player)
    {
        controllingPlayer = player;
        controllingPlayer.hasControl = false;
        controllingPlayer.instructionTextOverride = GetInstructionText();
    }

    private void ReleasePlayer()
    {
        var player = controllingPlayer;

        StartCoroutine(Helpers.DelayedAction(() =>
        {
            player.hasControl = true;
        }));

        controllingPlayer.instructionTextOverride = null;
        controllingPlayer = null;
    }
}
