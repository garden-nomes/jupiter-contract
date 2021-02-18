using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float rate = 1f;
    public float ratio = 2f / 3f;

    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        renderer.enabled = Time.time % rate < rate * ratio;
    }
}
