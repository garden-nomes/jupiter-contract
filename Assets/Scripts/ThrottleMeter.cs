using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleMeter : MonoBehaviour
{
    public ShipController ship;

    void Update()
    {
        var scale = transform.localScale;
        scale.y = ship.throttle;
        transform.localScale = scale;
    }
}
