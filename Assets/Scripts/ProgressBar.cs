using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    [Range(0f, 1f)] public float progress = 0f;

    public Transform foreground;

    void LateUpdate()
    {
        var flipped = transform.parent.localScale.x < 0f;

        var scale = transform.localScale;
        scale.x = flipped ? -1f : 1f;
        transform.localScale = scale;

        var position = transform.localPosition;
        position.x = (flipped ? 5f : -5f) / 8f;
        transform.localPosition = position;

        progress = Mathf.Clamp01(progress);
        var rounded = Mathf.Floor(progress * (9f - 0.01f)) / 8f;

        foreground.localPosition = Vector3.right * rounded * 0.5f;
        var fgScale = foreground.localScale;
        fgScale.x = rounded;
        foreground.localScale = fgScale;
    }
}
