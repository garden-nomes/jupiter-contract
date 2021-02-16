using System;
using UnityEngine;

public class NavStation : MonoBehaviour, IInteractible
{
    public float rotationSpeed = 90f;
    public Camera scopesCamera;
    public NavOverlay overlay;

    private PlayerController controllingPlayer;
    private Quaternion initialRotation;
    private Transform lockedTarget;
    private Transform hoveredTarget;

    public Vector3? Target => lockedTarget == null ? ((Vector3?) null) : lockedTarget.position;
    public Vector3? HoveredTarget => hoveredTarget == null ? (Vector3?) null : hoveredTarget.position;

    private void Start()
    {
        initialRotation = scopesCamera.transform.rotation;
    }

    private void Update()
    {
        if (controllingPlayer == null)
        {
            overlay.navTarget = lockedTarget == null ? null : lockedTarget;
            return;
        }

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

        hoveredTarget = FindHoveredNavTarget();

        if (lockedTarget && controllingPlayer.input.GetBtnDown(0))
        {
            lockedTarget = null;
        }
        else if (hoveredTarget && controllingPlayer.input.GetBtnDown(0))
        {
            lockedTarget = hoveredTarget;
        }

        overlay.navTarget = lockedTarget == null ? hoveredTarget : lockedTarget;

        controllingPlayer.instructionTextOverride = GetInstructionText();
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

        var lockInstructions = "";
        if (lockedTarget == null && hoveredTarget != null)
        {
            lockInstructions += $"{Icons.IconText(scheme.btn0)} lock target\n";
        }
        else if (lockedTarget != null)
        {
            lockInstructions += $"{Icons.IconText(scheme.btn0)} release target\n";
        }

        return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} rotate array\n" +
            lockInstructions +
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
