using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedMeter : MonoBehaviour
{
    public float pixelSize = 3f;
    [Range(0f, 1f)] public float value = 0f;
    [Range(0f, 1f)] public float startBlinking = 0.25f;
    public Image icon;

    private RectTransform rect;
    private Image image;
    private float maxWidth;

    void Start()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        var canvas = rect.GetComponentInParent<Canvas>();
        maxWidth = RectTransformUtility.PixelAdjustRect(rect, canvas).width;
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Clamp01(value);
        var width = Mathf.Round(value * maxWidth / pixelSize) * pixelSize;

        var position = rect.localPosition;
        position.x = (width - maxWidth) / 2f;
        rect.localPosition = position;

        var size = rect.sizeDelta;
        size.x = -(maxWidth - width);
        rect.sizeDelta = size;

        image.enabled = value > startBlinking || (Time.time % 0.5f < 2f / 6f);
        icon.enabled = value > startBlinking || (Time.time % 0.5f < 2f / 6f);
    }
}
