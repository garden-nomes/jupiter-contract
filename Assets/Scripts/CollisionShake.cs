using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShake : MonoBehaviour
{
    public Shake shake;

    public float time = 1f;
    public float amplitude = 100f;

    private float collisionTimer = 0f;

    private void Update()
    {

        if (collisionTimer > 0f)
        {
            shake.Add((collisionTimer / time) * amplitude);
            collisionTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        collisionTimer = time;
    }

    [ContextMenu("Test")]
    private void Test()
    {
        collisionTimer = time;
    }
}
