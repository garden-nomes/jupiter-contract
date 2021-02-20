using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    public Text text;

    public void SetDistance(float distance)
    {
        text.text = Format.Distance(distance);
    }
}
