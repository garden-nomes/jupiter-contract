using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float ore = 1000f;
    public float scaleRatio = 0.01f;

    public ParticleSystem miningParticleSystem;

    private float previousOre = 0f;

    void Update()
    {
        var volume = ore * scaleRatio;
        var radius = Mathf.Pow(3f / (4f * Mathf.PI) * volume, 1f / 3f);
        transform.localScale = Vector3.one * radius;

        var emission = miningParticleSystem.emission;
        emission.enabled = previousOre > ore;
        previousOre = ore;
    }
}
