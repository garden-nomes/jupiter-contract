using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavMarkerBlink : MonoBehaviour
{
    [Min(0f)] public float rate = 1f;
    public Color color1 = Color.white;
    public Color color2 = Color.black;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.color = (Time.time % rate < rate * 0.5f) ? color1 : color2;
    }
}
