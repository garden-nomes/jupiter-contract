using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldFullMessage : MonoBehaviour
{
    public float minimumShowTime = 4f;
    public bool waitForCancel = true;

    private float showTimer = 0f;
    private bool isCanceled = false;

    void Update()
    {
        showTimer += Time.deltaTime;

        if (showTimer > minimumShowTime && (isCanceled || !waitForCancel))
        {
            gameObject.SetActive(false);
        }
    }

    [ContextMenu("Show")]
    public void Show()
    {
        gameObject.SetActive(true);
        showTimer = 0f;
        isCanceled = false;
    }

    [ContextMenu("Cancel")]
    public void Cancel()
    {
        isCanceled = true;
    }
}
