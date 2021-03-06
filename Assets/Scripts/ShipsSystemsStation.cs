using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsSystemsStation : StationBehaviour
{
    public Blink bridgeLightBlinker;

    public StatusIndicator portEngineIndicator;
    public StatusIndicator stbdEngineIndicator;

    public ShipController ship;

    public override string GetActionText(PlayerController player)
    {
        return "check systems";
    }

    protected override void Update()
    {
        portEngineIndicator.isOk = !ship.portEngine.IsFaulty;
        stbdEngineIndicator.isOk = !ship.stbdEngine.IsFaulty;
        bridgeLightBlinker.enabled = !portEngineIndicator.isOk || !stbdEngineIndicator.isOk;
        base.Update();
    }
}
