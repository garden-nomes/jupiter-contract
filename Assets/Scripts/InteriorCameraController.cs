using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorCameraController : MonoBehaviour
{
    public PlayerController target;
    public Transform navStationScreen;
    public int maskLayer;

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
    }

    void LateUpdate()
    {
        if (target.isUsingNavStation)
        {
            CenterCamera(navStationScreen.position);
        }
        else
        {
            foreach (var compartment in compartments)
            {
                if (compartment.bounds.Contains(target.transform.position))
                {
                    compartment.GetComponent<SpriteMask>().enabled = true;
                    compartment.gameObject.layer = maskLayer;
                    CenterCamera(compartment.bounds.center);
                    break;

                }
            }
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
