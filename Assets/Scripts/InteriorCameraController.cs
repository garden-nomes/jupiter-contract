using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteriorCameraController : MonoBehaviour
{
    public PlayerController target;
    public int maskLayer;
    public Camera camera;
    public bool isVisible = true;

    private BoxCollider2D[] compartments;
    private int? tempMaskLayer;

    void Start()
    {
        var compartmentsObjects = GameObject.FindGameObjectsWithTag("compartment");
        compartments = compartmentsObjects.Select(c => c.GetComponent<BoxCollider2D>()).ToArray();
    }

    void Update()
    {
        if (tempMaskLayer != null)
        {
            camera.cullingMask = camera.cullingMask & (~(1 << tempMaskLayer.Value));
            tempMaskLayer = null;
        }

        foreach (var compartment in compartments)
        {
            compartment.GetComponent<SpriteMask>().enabled = false;
        }
    }

    void LateUpdate()
    {
        if (target.station != null && target.station.cameraPositionOverride != null)
        {
            CenterCamera(target.station.cameraPositionOverride.position);
        }
        else
        {
            foreach (var compartment in compartments)
            {
                if (compartment.bounds.Contains(target.transform.position))
                {
                    var spriteMask = compartment.GetComponent<SpriteMask>();

                    if (spriteMask.enabled)
                    {
                        tempMaskLayer = spriteMask.gameObject.layer;
                        Debug.Log(tempMaskLayer);
                        camera.cullingMask = camera.cullingMask | (1 << tempMaskLayer.Value);
                    }
                    else
                    {
                        spriteMask.enabled = true;
                        spriteMask.gameObject.layer = maskLayer;
                    }

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
