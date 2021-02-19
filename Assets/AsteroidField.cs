using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    public Asteroid asteroidPrefab;

    [Min(0f)] public float radius = 3000f;
    [Min(0)] public int count = 100;
    public float minOre = 1000f;
    public float maxOre = 5000f;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var position = Random.insideUnitSphere * radius;
            var asteroid = GameObject.Instantiate(asteroidPrefab, position, Quaternion.identity, transform);
            asteroid.ore = Random.Range(minOre, maxOre);
        }
    }
}
