using System;
using UnityEngine;

public class NavStation : MonoBehaviour, IInteractible
{
    public float rotationSpeed = 90f;
    public Camera scopesCamera;
    public NavigationMarkers overlay;
    public ShipController ship;

    private PlayerController controllingPlayer;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = scopesCamera.transform.rotation;
    }

    private void Update()
    {
        if (controllingPlayer == null)
        {
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

        if (ship.target != null && controllingPlayer.input.GetBtnDown(0))
        {
            ship.target = null;
        }
        else if (overlay.Target != null && controllingPlayer.input.GetBtnDown(0))
        {
            ship.target = overlay.Target;
        }

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

    Vector3? FindHoveredNavTarget()
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

        return currentTarget == null ? (Vector3?) null : currentTarget.transform.position;
    }

    private string GetInstructionText()
    {
        if (controllingPlayer == null) return "";

        var scheme = controllingPlayer.input.inputScheme;

        var lockInstructions = "";
        if (ship.target == null && overlay.Target != null)
        {
            lockInstructions += $"{Icons.IconText(scheme.btn0)} lock target\n";
        }
        else if (ship.target != null)
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
        controllingPlayer.isUsingNavStation = true;
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
        controllingPlayer.isUsingNavStation = false;
        controllingPlayer = null;
    }
}
