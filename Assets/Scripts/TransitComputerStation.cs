using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitComputerStation : StationBehaviour
{
    public override string GetActionText(PlayerController player)
    {
        return "check transit computer";
    }
}
