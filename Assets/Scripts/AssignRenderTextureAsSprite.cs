using UnityEngine;

public class AssignRenderTextureAsSprite : MonoBehaviour
{
    public RenderTexture renderTexture;
    public float pixelsPerUnit = 8f;

    private Texture2D texture;

    void Start()
    {
        texture = new Texture2D(renderTexture.width, renderTexture.height);
        texture.filterMode = FilterMode.Point;

        var sprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(.5f, .5f),
            pixelsPerUnit);

        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Update()
    {
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
    }

}
