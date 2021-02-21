using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldFullMessage : MonoBehaviour
{
    public float minimumShowTime = 2f;

    private float showTimer = 0f;

    void Update()
    {
        showTimer += Time.deltaTime;
    }

    [ContextMenu("Show")]
    public void Show()
    {
        gameObject.SetActive(true);
        showTimer = 0f;
    }

    [ContextMenu("Cancel")]
    public void Cancel()
    {
        if (showTimer > minimumShowTime)
        {
            gameObject.SetActive(false);
        }
    }
}
