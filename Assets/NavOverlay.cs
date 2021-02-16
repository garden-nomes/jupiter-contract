using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavOverlay : MonoBehaviour
{
    public ShipController ship;
    public Transform navTarget;

    public RectTransform attitudeMarker;
    public RectTransform progradeMarker;
    public RectTransform retrogradeMarker;
    public RectTransform targetMarker;

    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        MarkHeading(ship.transform.rotation * Vector3.up, attitudeMarker);

        var velocity = ship.GetComponent<Rigidbody>().velocity;
        MarkHeading(velocity, progradeMarker);
        MarkHeading(velocity * -1, retrogradeMarker);

        var targetCanvasPosition = navTarget == null ? null :
            WorldToCanvasPosition(navTarget.position);

        if (targetCanvasPosition == null)
        {
            targetMarker.gameObject.SetActive(false);
        }
        else
        {
            targetMarker.gameObject.SetActive(true);
            targetMarker.anchoredPosition = targetCanvasPosition.Value;
        }

        // flicker
        canvas.enabled = Time.frameCount % 2 == 0;
    }

    void MarkHeading(Vector3 heading, RectTransform marker)
    {
        var position = canvas.worldCamera.transform.position + heading;
        var canvasPosition = WorldToCanvasPosition(position);

        if (canvasPosition != null)
        {
            marker.gameObject.SetActive(true);
            marker.anchoredPosition = canvasPosition.Value;
        }
        else
        {
            marker.gameObject.SetActive(false);
        }
    }

    Vector2? WorldToCanvasPosition(Vector3 world)
    {
        var viewportPosition = canvas.worldCamera.WorldToViewportPoint(world);
        var rect = canvas.GetComponent<RectTransform>();

        if (viewportPosition.z <= 0)
        {
            return null;
        }

        var canvasPosition = viewportPosition * rect.sizeDelta - rect.sizeDelta * .5f;

        return new Vector2(
            Mathf.Floor(canvasPosition.x - .5f) + .5f,
            Mathf.Floor(canvasPosition.y - .5f) + .5f
        );
    }
}
