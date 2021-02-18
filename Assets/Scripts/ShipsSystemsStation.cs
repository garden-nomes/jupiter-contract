using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsSystemsStation : StationBehaviour
{
    public StatusIndicator portEngineIndicator;
    public StatusIndicator stbdEngineIndicator;

    public ShipController ship;

    public override string GetActionText(PlayerController player)
    {
        return "check systems";
    }

    protected override void UseStation(PlayerController player)
    {
        portEngineIndicator.isOk = !ship.portEngine.IsBroken;
        stbdEngineIndicator.isOk = !ship.stbdEngine.IsBroken;
    }
}
