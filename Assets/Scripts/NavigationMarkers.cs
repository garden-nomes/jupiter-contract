using UnityEngine;

public class NavigationMarkers : MonoBehaviour
{
    public bool hoverTargets = false;

    public ShipController ship;
    public new Camera camera;

    public RectTransform attitudeMarker;
    public RectTransform progradeMarker;
    public RectTransform retrogradeMarker;
    public RectTransform targetMarker;

    private Canvas canvas;
    private RectTransform canvasRect;

    private Vector3? hoveredTarget;
    public Vector3? HoveredTarget => hoveredTarget;
    public Vector3? Target => ship.target != null ? ship.target : hoveredTarget;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // mark heading
        PlaceMarker(attitudeMarker, camera.transform.position + ship.Heading);

        // mark target
        hoveredTarget = hoverTargets ? FindHoveredNavTarget() : null;
        PlaceMarker(targetMarker, Target);

        // mark velocity
        if (ship.Velocity.sqrMagnitude > 0f)
        {
            PlaceMarker(progradeMarker, camera.transform.position + ship.Velocity);
            PlaceMarker(retrogradeMarker, camera.transform.position - ship.Velocity);
        }
        else
        {
            PlaceMarker(progradeMarker, null);
            PlaceMarker(retrogradeMarker, null);
        }
    }

    // place a UI element at a world position, or disable it 
    private void PlaceMarker(RectTransform marker, Vector3? point)
    {
        if (point == null)
        {
            marker.gameObject.SetActive(false);
            return;
        }

        var screenPosition = camera.WorldToScreenPoint(point.Value);

        if (screenPosition.z < 0 ||
            !RectTransformUtility.RectangleContainsScreenPoint(canvasRect, screenPosition, camera))
        {
            marker.gameObject.SetActive(false);
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, screenPosition, camera, out Vector2 localPoint))
        {
            // markers seem to display best when positioned at half-pixel offsets
            localPoint.x = Mathf.Floor(localPoint.x - .5f) + .5f;
            localPoint.y = Mathf.Floor(localPoint.y - .5f) + .5f;

            marker.gameObject.SetActive(true);
            marker.anchoredPosition = localPoint;
        }
        else
        {
            marker.gameObject.SetActive(false);
        }
    }

    Vector3? FindHoveredNavTarget()
    {
        var targets = GameObject.FindGameObjectsWithTag("nav target");

        var forward = camera.transform.forward;
        var from = camera.transform.position;
        GameObject currentTarget = null;
        Vector3? currentTargetDirection = null;

        foreach (var target in targets)
        {
            if (currentTarget == null || currentTargetDirection == null)
            {
                currentTarget = target;
                currentTargetDirection = (currentTarget.transform.position - from).normalized;
                continue;
            }

            var currentDot = Vector3.Dot(forward, currentTargetDirection.Value);
            var targetDot = Vector3.Dot(forward, (target.transform.position - from).normalized);
            if (targetDot > currentDot)
            {
                currentTarget = target;
                currentTargetDirection = (currentTarget.transform.position - from).normalized;
            }
        }

        return currentTarget == null ? (Vector3?) null : currentTarget.transform.position;
    }
}
