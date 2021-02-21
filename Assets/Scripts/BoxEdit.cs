using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoxEdit : MonoBehaviour
{
    public Vector2 a;
    public Vector2 b;
    public float pixelsPerUnit = 8f;

    void Awake()
    {
        var extent = new Vector2(
            (transform.localScale.x - 1f / 8f) / 2f,
            (transform.localScale.y - 1f / 8f) / 2f
        );

        a = (Vector2) transform.position - extent;
        b = (Vector2) transform.position + extent;
    }

    public void Move()
    {
        var height = Mathf.Abs(a.y - b.y) + 1 / 8f;
        var width = Mathf.Abs(a.x - b.x) + 1 / 8f;

        transform.position = new Vector3(0f, 0f, transform.position.z) + (Vector3) (a + b) / 2f;
        transform.localScale = new Vector3(width, height, 1f);
    }
}
