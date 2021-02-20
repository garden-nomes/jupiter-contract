using UnityEngine;

public class NavStation : StationBehaviour
{
    public float rotationSpeed = 90f;
    public Camera scopesCamera;
    public NavigationMarkers overlay;
    public ShipController ship;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = scopesCamera.transform.localRotation;
    }

    protected override void UseStation(PlayerController player)
    {
        var horizontal = player.input.horizontal;
        scopesCamera.transform.rotation *=
            Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.up);
        var vertical = player.input.vertical;
        scopesCamera.transform.rotation *=
            Quaternion.AngleAxis(vertical * Time.deltaTime * rotationSpeed, Vector3.left);

        if (ship.target != null && player.input.GetBtnDown(0))
        {
            ship.target = null;
        }
        else if (overlay.Target != null && player.input.GetBtnDown(0))
        {
            ship.target = overlay.Target;
        }
    }

    public override string GetActionText(PlayerController player)
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

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;

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

    protected override void Update()
    {
        base.Update();

        if (!HasControl && scopesCamera.transform.localRotation != initialRotation)
        {
            ResetRotation();
        }
    }

    private void ResetRotation()
    {
        var currentRotation = scopesCamera.transform.localRotation;

        if (Quaternion.Angle(currentRotation, initialRotation) <= rotationSpeed * Time.deltaTime)
        {
            scopesCamera.transform.localRotation = initialRotation;
        }
        else
        {
            scopesCamera.transform.localRotation = Quaternion.RotateTowards(
                currentRotation, initialRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
