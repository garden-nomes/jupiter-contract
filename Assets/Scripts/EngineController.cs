using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public EngineBreakPoint[] breakPoints;

    private bool isBroken;
    public bool IsBroken => isBroken;

    void Start()
    {
        foreach (var breakPoint in breakPoints)
        {
            breakPoint.gameObject.SetActive(false);
        }

        Break();
    }

    void Update()
    {
        isBroken = false;

        foreach (var breakPoint in breakPoints)
        {
            if (breakPoint.gameObject.activeSelf && breakPoint.IsCritical)
            {
                isBroken = true;
            }
        }
    }

    public void Break()
    {
        var breakPoint = breakPoints[Random.Range(0, breakPoints.Length)];
        breakPoint.gameObject.SetActive(true);
        breakPoint.Reset();
    }

}
