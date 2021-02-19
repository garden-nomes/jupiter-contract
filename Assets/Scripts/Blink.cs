using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float rate = 1f;
    public float ratio = 2f / 3f;

    private new Renderer renderer;
    private float blinkTimer = 0f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        blinkTimer += Time.deltaTime;
        renderer.enabled = blinkTimer % rate < rate * ratio;
    }

    private void OnEnable()
    {
        blinkTimer = 0f;
    }
}
