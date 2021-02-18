using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleMeter : MonoBehaviour
{
    public EngineController engine;

    void Update()
    {
        var scale = transform.localScale;
        scale.y = engine.IsBroken ? 0f : engine.throttle;
        transform.localScale = scale;
    }
}
