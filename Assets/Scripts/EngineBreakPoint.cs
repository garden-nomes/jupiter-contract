using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBreakPoint : MonoBehaviour, IInteractible
{
    public PlayerController fixingPlayer;
    public float fixedTimer = 0f;
    public float timeToFix = 3f;

    public bool CanInteract() => true;
    public string GetActionText(PlayerController player) => "(hold) fix engine";

    void Update()
    {
        if (fixingPlayer != null)
        {
            fixedTimer += Time.deltaTime;
            fixingPlayer.Progress = fixedTimer / timeToFix;

            if (fixedTimer >= timeToFix)
            {
                fixingPlayer.hasControl = true;
                fixingPlayer.Progress = null;
                fixingPlayer = null;
                gameObject.SetActive(false);
            }
            else if (fixingPlayer.input.GetBtnUp(2))
            {
                fixingPlayer.hasControl = true;
                fixingPlayer.Progress = null;
                fixingPlayer = null;
            }
        }
    }

    public void Interact(PlayerController player)
    {
        fixingPlayer = player;
        player.hasControl = false;
        fixedTimer = 0f;
    }
}
