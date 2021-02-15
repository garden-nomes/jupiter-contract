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

        var center = Vector2.zero;
        foreach (var compartment in compartments)
        {
            compartment.GetComponent<SpriteRenderer>().enabled = true;

            foreach (var target in targets)
            {
                if (compartment.bounds.Contains(target.position))
                {
                    compartment.GetComponent<SpriteRenderer>().enabled = false;
                    center += (Vector2) compartment.bounds.center;
                    break;
                }
            }
        }

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
