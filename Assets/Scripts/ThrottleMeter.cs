using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleMeter : MonoBehaviour
{
    public Sprite squareSprite;
    public Color color = Color.white;
    public int sortingOrder = 5;

    public float width;
    [Range(0f, 1f)] public float value = 0f;

    private SpriteRenderer[] dots;
    private int dotCount;

    void Start()
    {
        dotCount = Mathf.FloorToInt(width * 4f);
        dots = new SpriteRenderer[dotCount];

        for (int i = 0; i < dotCount; i++)
        {
            float x = 1f / 16f + i * 0.25f;

            var dot = new GameObject();
            dot.transform.SetParent(transform);
            dot.transform.position = transform.position + Vector3.right * x;
            dot.transform.localScale = Vector3.one / 8f;

            var renderer = dot.AddComponent<SpriteRenderer>();
            renderer.sprite = squareSprite;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;
            renderer.enabled = false;

            dots[i] = renderer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * width);
    }

    void Update()
    {
        float visibleDots = Mathf.RoundToInt(value * (dotCount + 1f));

        for (int i = 0; i < dotCount; i++)
        {
            dots[i].enabled = i < visibleDots;
        }
    }
}
