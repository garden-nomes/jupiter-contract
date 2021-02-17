using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CameraRenderer : MonoBehaviour
{
    public new Camera camera;
    public float pixelsPerUnit = 8f;
    public RenderTexture renderTexture;
    public PixelPerfectCamera pixelPerfectCamera;

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

#if UNITY_EDITOR
        var pixelRatio = 3f;
#else
        var pixelRatio = pixelPerfectCamera.pixelRatio;
#endif

        var textureWidth = renderTexture.width;
        var textureHeight = renderTexture.height;
        var rectWidth = screenRect.width;
        var rectHeight = screenRect.height;
        var uvWidth = rectWidth / (textureWidth * pixelRatio);
        var uvHeight = rectHeight / (textureHeight * pixelRatio);
        image.uvRect = new Rect(0.5f - uvWidth / 2f, 0.5f - uvHeight / 2f, uvWidth, uvHeight);
        camera.orthographicSize = textureHeight / (pixelsPerUnit * 2f);
    }
}
