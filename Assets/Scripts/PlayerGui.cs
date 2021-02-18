using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGui : MonoBehaviour
{
    public PlayerController player;

    public TMPro.TextMeshProUGUI instructions;
    public NeedMeter tirednessMeter;

    void Update()
    {
        if (player.station != null)
        {
            instructions.text = player.station.GetInstructionText(player);
        }
        else
        {
            instructions.text = player.instructionText;
        }

        tirednessMeter.value = 1f - player.tiredness;
    }
}
