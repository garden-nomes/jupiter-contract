using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float frequency = 0.1f;
    public float amplitude = 10f;

    private Vector2 defaultPosition;
    private RectTransform rect;
    private float shakeAmount = 0f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        defaultPosition = rect.anchoredPosition;
    }

    public void Add(float amount)
    {
        shakeAmount += amount;
    }

    void LateUpdate()
    {
        var noiseX = Mathf.PerlinNoise(Time.time * frequency, 1e3f) * amplitude * shakeAmount;
        var noiseY = Mathf.PerlinNoise(Time.time * frequency, 1e4f) * amplitude * shakeAmount;
        rect.anchoredPosition = defaultPosition + new Vector2(noiseX, noiseY);
        shakeAmount = 0f;
    }
}
