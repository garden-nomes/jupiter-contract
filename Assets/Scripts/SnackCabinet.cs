using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackCabinet : MonoBehaviour, IInteractible
{
    public PlayerController snackingPlayer;
    public float snackTime = 3f;
    public float snackAmount = 0.5f;

    public bool CanInteract() => true;
    public string GetActionText(PlayerController player) => "(hold) eat snack";

    private float snackTimer = 0f;

    void Update()
    {
        if (snackingPlayer != null)
        {
            snackTimer += Time.deltaTime;
            snackingPlayer.progress = snackTimer / snackTime;
            snackingPlayer.actionText = "finding snack...";

            if (snackTimer >= snackTime)
            {
                snackingPlayer.sfx.Blip();
                snackingPlayer.hasControl = true;
                snackingPlayer.progress = null;
                snackingPlayer.actionText = null;
                snackingPlayer.hunger -= snackAmount;
                snackingPlayer = null;
            }
            else if (snackingPlayer.input.GetBtnUp(2))
            {
                snackingPlayer.hasControl = true;
                snackingPlayer.progress = null;
                snackingPlayer.actionText = null;
                snackingPlayer = null;
            }
        }
    }

    public void Interact(PlayerController player)
    {
        snackingPlayer = player;
        player.hasControl = false;
        snackTimer = 0f;
        snackingPlayer.sfx.Blip();
    }
}
