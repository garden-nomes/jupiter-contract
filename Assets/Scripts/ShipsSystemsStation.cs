using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsSystemsStation : StationBehaviour
{
    public override string GetActionText(PlayerController player)
    {
        return "check systems";
    }
}
