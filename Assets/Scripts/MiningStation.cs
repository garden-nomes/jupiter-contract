using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningStation : StationBehaviour
{
    public ShipController ship;
    public Transform miningCamera;
    public float rotationSpeed = 90f;

    public Text oreReadout;
    public Text capacityReadout;
    public GameObject targetInRangeLight;
    public GameObject holdFullLight;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = miningCamera.localRotation;
    }

    protected override void UseStation(PlayerController player)
    {
        var horizontal = player.input.horizontal;
        miningCamera.transform.rotation *=
            Quaternion.AngleAxis(horizontal * Time.deltaTime * rotationSpeed, Vector3.up);

        var vertical = player.input.vertical;
        miningCamera.transform.rotation *=
            Quaternion.AngleAxis(vertical * Time.deltaTime * rotationSpeed, Vector3.left);

        oreReadout.text = ship.ore.ToString("0.0");
        capacityReadout.text = ship.capacity.ToString("0.0");
        targetInRangeLight.gameObject.SetActive(ship.IsMining);
        holdFullLight.gameObject.SetActive(ship.ore >= ship.capacity);
    }

    public override string GetInstructionText(PlayerController player)
    {
        var scheme = player.input.inputScheme;

        return $"{Icons.VerticalAxis(scheme)}{Icons.HorizontalAxis(scheme)} target laser\n" +
            $"{Icons.IconText(scheme.btn0)} (hold) activate laser" +
            $"{Icons.IconText(scheme.btn2)} cancel";
    }

    protected override void OnRelease(PlayerController player)
    {
        miningCamera.localRotation = initialRotation;
    }

}
