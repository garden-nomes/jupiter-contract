using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLayout : MonoBehaviour
{
    public Transform player1Camera;
    public Transform player2Camera;

    public RectTransform player1CameraDisplay;
    public RectTransform player2CameraDisplay;
    public RectTransform player1Gui;
    public RectTransform player2Gui;

    void Update()
    {
        if (player1Camera.position == player2Camera.position)
        {
            player2CameraDisplay.gameObject.SetActive(false);
            player2Camera.GetComponent<InteriorCameraController>().isVisible = false;

            player1CameraDisplay.anchorMin = Vector2.zero;
            player1CameraDisplay.anchorMax = Vector2.one;

            player1Gui.anchorMin = new Vector2(0f, 0f);
            player1Gui.anchorMax = new Vector2(1f / 3f, 1f);
            player2Gui.anchorMin = new Vector2(2f / 3f, 0f);
            player2Gui.anchorMax = new Vector2(1f, 1f);

        }
        else
        {
            player2CameraDisplay.gameObject.SetActive(true);
            player2Camera.GetComponent<InteriorCameraController>().isVisible = true;

            player1CameraDisplay.anchorMin = new Vector2(1f / 3f, 0.5f);
            player1CameraDisplay.anchorMax = new Vector2(1f, 1f);
            player1Gui.anchorMin = new Vector2(0f, 0.5f);
            player1Gui.anchorMax = new Vector2(2f / 3f, 1f);

            player2CameraDisplay.anchorMin = new Vector2(0f, 0f);
            player2CameraDisplay.anchorMax = new Vector2(2f / 3f, .5f);
            player2Gui.anchorMin = new Vector2(1f / 3f, 0f);
            player2Gui.anchorMax = new Vector2(1f, .5f);
        }
    }
}
