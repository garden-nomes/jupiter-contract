using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningStation : StationBehaviour
{
    public ShipController ship;

    public Text oreReadout;
    public Text capacityReadout;
    public GameObject targetInRangeLight;
    public GameObject holdFullLight;

    protected override void Update()
    {
        oreReadout.text = ship.ore.ToString("0.0");
        capacityReadout.text = ship.capacity.ToString("0.0");
        targetInRangeLight.gameObject.SetActive(ship.IsMining);
        holdFullLight.gameObject.SetActive(ship.ore >= ship.capacity);
    }
}
