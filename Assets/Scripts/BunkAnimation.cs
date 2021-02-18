using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BunkAnimation : MonoBehaviour
{
    public Sprite[] openFrames;
    public Sprite[] closedFrames;

    public bool isOpen = true;
    private bool wasOpen = true;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isOpen && !wasOpen)
        {
            spriteRenderer.sprite = openFrames[Random.Range(0, openFrames.Length)];
            wasOpen = isOpen;
        }
        else if (!isOpen && wasOpen)
        {
            spriteRenderer.sprite = closedFrames[Random.Range(0, openFrames.Length)];
            wasOpen = isOpen;
        }
    }
}
