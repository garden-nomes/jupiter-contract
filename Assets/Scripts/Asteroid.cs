using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Asteroid : MonoBehaviour
{
    public float ore = 1000f;
    public float scaleRatio = 0.01f;

    void Start() { }

    void Update()
    {
        var volume = ore * scaleRatio;
        var radius = Mathf.Pow(3f / (4f * Mathf.PI) * volume, 1f / 3f);
        transform.localScale = Vector3.one * radius;
    }
}
