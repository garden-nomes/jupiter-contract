using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunksController : MonoBehaviour, IInteractible
{
    public PlayerController topBunkOwner;
    public PlayerController bottomBunkOwner;

    public BunkAnimation topBunk;
    public BunkAnimation bottomBunk;

    private bool isTopBunkOccupied = false;
    private bool isBottomBunkOccupied = false;

    public bool CanInteract() => true;
    public string GetActionText(PlayerController player) => "rest";

    public void Interact(PlayerController player)
    {
        if (player == topBunkOwner)
        {
            isTopBunkOccupied = true;
        }

        if (player == bottomBunkOwner)
        {
            isBottomBunkOccupied = true;
        }

        player.instructionText.text = $"{Icons.IconText(player.input.inputScheme.btn2)} cancel";
        player.gameObject.SetActive(false);
    }

    void Update()
    {
        topBunk.isOpen = !isTopBunkOccupied;
        bottomBunk.isOpen = !isBottomBunkOccupied;

        if (isTopBunkOccupied && topBunkOwner.input.GetBtnDown(2))
        {
            isTopBunkOccupied = false;
            topBunkOwner.gameObject.SetActive(true);
        }

        if (isBottomBunkOccupied && bottomBunkOwner.input.GetBtnDown(2))
        {
            isBottomBunkOccupied = false;
            bottomBunkOwner.gameObject.SetActive(true);
        }
    }
}
