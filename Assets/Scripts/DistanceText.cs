using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceText : MonoBehaviour
{
    public NavigationMarkers overlay;
    public ShipController ship;

    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (overlay.Target != null)
        {
            var distance = (ship.transform.position - overlay.Target.Value).magnitude;
            text.text = FormatDistance(distance);
        }
        else
        {
            text.text = "";
        }
    }

    private string FormatDistance(float distance)
    {
        if (distance < 1000)
        {
            return $"{Mathf.Round(distance)}m";
        }
        else
        {
            return $"{Mathf.Round(distance / 100f) * 0.1f}km";
        }
    }
}
