using UnityEngine;

public class NavStation : StationBehaviour
{
    public float rotationSpeed = 90f;
    public Camera scopesCamera;
    public NavigationMarkers overlay;
    public ShipController ship;

    public DistanceText distanceText;

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

        if (horizontal != 0f || vertical != 0f)
        {
            ship.CancelPopup();
        }

        if (ship.ore < ship.capacity) // disable changing targets when station is targeted
        {
            if (ship.target != null && player.input.GetBtnDown(0))
            {
                sfx.Blip();
                ship.target = null;
            }
            else if (overlay.HoveredTarget != null && player.input.GetBtnDown(0))
            {
                sfx.Blip();
                ship.target = overlay.HoveredTarget;
            }
        }

        if (overlay.Target != null)
        {
            var distance = (ship.Position - overlay.Target.Value).magnitude;
            distanceText.SetDistance(distance);
        }
    }

    public override string GetActionText(PlayerController player)
    {
        return "check nav array";
    }

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;

        var lockInstructions = "";

        if (ship.ore < ship.capacity)
        {
            if (ship.target == null && overlay.HoveredTarget != null)
            {
                lockInstructions += $"{Icons.IconText(scheme.btn0)} lock target\n";
            }
            else if (ship.target != null)
            {
                lockInstructions += $"{Icons.IconText(scheme.btn0)} release target\n";
            }
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

    protected override void OnLock(PlayerController player)
    {
        distanceText.gameObject.SetActive(true);
        overlay.hoverTargets = true;
    }

    protected override void OnRelease(PlayerController player)
    {
        distanceText.gameObject.SetActive(false);
        overlay.hoverTargets = false;
    }
}
