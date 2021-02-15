using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorCameraController : MonoBehaviour
{
    public BoxCollider2D[] compartments;
    public Transform[] targets;

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
