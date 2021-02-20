using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    public Text text;

    public void SetDistance(float distance)
    {
        text.text = FormatDistance(distance);
    }

    private string FormatDistance(float distance)
    {
        if (distance < 1000)
        {
            return $"{distance.ToString("0")}m";
        }
        else
        {
            return $"{(distance / 1000f).ToString("0.00")}km";
        }
    }
}
