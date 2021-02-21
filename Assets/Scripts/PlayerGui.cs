using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGui : MonoBehaviour
{
    public PlayerController player;

    public TMPro.TextMeshProUGUI instructions;
    public GameObject needMeterContainer;
    public NeedMeter progressMeter;
    public NeedMeter tirednessMeter;
    public NeedMeter hungyMeter;

    void Update()
    {
        if (player.actionText != null && player.actionText.Length > 0)
        {
            instructions.text = player.actionText;
        }
        else if (player.station != null)
        {
            instructions.text = player.station.GetInstructionText(player);
        }
        else
        {
            instructions.text = player.instructionText;
        }

        tirednessMeter.value = 1f - player.tiredness;
        hungyMeter.value = 1f - player.hunger;

        if (player.progress != null)
        {
            needMeterContainer.SetActive(false);
            progressMeter.gameObject.SetActive(true);
            progressMeter.value = player.progress.Value;
        }
        else
        {
            progressMeter.gameObject.SetActive(false);
            needMeterContainer.SetActive(true);
        }
    }
}
