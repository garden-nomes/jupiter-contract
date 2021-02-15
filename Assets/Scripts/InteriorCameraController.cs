using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorCameraController : MonoBehaviour
{
    public Transform[] targets;

    private BoxCollider2D[] compartments;

    void Start()
    {
        var compartmentsObjects = GameObject.FindGameObjectsWithTag("compartment");
        compartments = compartmentsObjects.Select(c => c.GetComponent<BoxCollider2D>()).ToArray();
    }

    void Update()
    {
        foreach (var compartment in compartments)
        {
            compartment.GetComponent<SpriteMask>().enabled = false;
        }

        var center = Vector2.zero;

        foreach (var target in targets)
        {
            var activeCompartment = compartments[0];

            foreach (var compartment in compartments)
            {
                if (compartment.bounds.Contains(target.position))
                {
                    activeCompartment = compartment;
                    break;
                }
            }

            activeCompartment.GetComponent<SpriteMask>().enabled = true;
            center += (Vector2) activeCompartment.bounds.center;
        }

        center /= targets.Length;

        CenterCamera(center);
    }

    void CenterCamera(Vector2 position)
    {
        var transformPosition = transform.position;
        transformPosition.x = position.x;
        transformPosition.y = position.y;
        transform.position = transformPosition;
    }
}
