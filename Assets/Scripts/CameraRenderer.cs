using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CameraRenderer : MonoBehaviour
{
    public new Camera camera;
    public float pixelsPerUnit = 8f;
    public int pixelScale = 3;
    public RenderTexture renderTexture;

    private RectTransform rect;
    private RawImage image;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<RawImage>();

        image.texture = renderTexture;
    }

    void LateUpdate()
    {
        Resize();
    }

    void Resize()
    {
        var screenRect = RectTransformUtility.PixelAdjustRect(rect, image.canvas);

        var textureWidth = renderTexture.width;
        var textureHeight = renderTexture.height;
        var rectWidth = screenRect.width;
        var rectHeight = screenRect.height;
        var uvWidth = rectWidth / (textureWidth * pixelScale);
        var uvHeight = rectHeight / (textureHeight * pixelScale);
        image.uvRect = new Rect(0.5f - uvWidth / 2f, 0.5f - uvHeight / 2f, uvWidth, uvHeight);
        camera.orthographicSize = textureHeight / (pixelsPerUnit * 2f);
    }
}
