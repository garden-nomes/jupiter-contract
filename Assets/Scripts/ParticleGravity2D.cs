using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGravity2D : MonoBehaviour
{
    private new ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var forceOverLifetime = particleSystem.forceOverLifetime;
        forceOverLifetime.x = Physics2D.gravity.x;
        forceOverLifetime.y = Physics2D.gravity.y;
    }
}
