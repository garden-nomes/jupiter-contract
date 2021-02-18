using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedMeter : MonoBehaviour
{
    public float pixelSize = 3f;
    [Range(0f, 1f)] public float value = 0f;

    private RectTransform rect;
    private float maxWidth;

    void Start()
    {
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
    }
}
