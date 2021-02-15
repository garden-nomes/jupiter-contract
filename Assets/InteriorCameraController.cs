using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorCameraController : MonoBehaviour
{
    public BoxCollider2D[] compartments;
    public Transform target;

    void Update()
    {
        BoxCollider2D currentCompartment = null;

        foreach (var compartment in compartments)
        {
            compartment.GetComponent<SpriteRenderer>().enabled = true;
            if (compartment.bounds.Contains(target.position))
            {
                currentCompartment = compartment;
            }
        }

        if (currentCompartment)
        {
            currentCompartment.GetComponent<SpriteRenderer>().enabled = false;
            CenterCamera((Vector2) currentCompartment.bounds.center);
        }
    }

    void CenterCamera(Vector2 position)
    {
        var transformPosition = transform.position;
        transformPosition.x = position.x;
        transformPosition.y = position.y;
        transform.position = transformPosition;
    }
}
