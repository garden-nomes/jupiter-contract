using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineParticleController : MonoBehaviour
{
    public EngineController engine;
    public float maxParticleSpeed;
    public float minParticleSpeed;

    private new ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var emission = particleSystem.emission;
        if (engine.IsBroken || engine.throttle == 0f)
        {
            emission.enabled = false;
        }
        else
        {
            emission.enabled = true;

            var main = particleSystem.main;
            main.startSpeed = Mathf.Max(engine.throttle * maxParticleSpeed, minParticleSpeed);
        }
    }
}
