using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public EngineBreakPoint[] breakPoints;

    public float throttle = 0f;
    public float minBreakTime = 30f;
    public float maxBreakTime = 60f;

    private bool isBroken;
    public bool IsBroken => isBroken;

    public float Thrust => IsBroken ? 0f : throttle;

    private float breakTimer;

    void Start()
    {
        foreach (var breakPoint in breakPoints)
        {
            breakPoint.gameObject.SetActive(false);
        }

        ResetBreakTimer();
    }

    void Update()
    {
        // update isBroken
        isBroken = false;
        foreach (var breakPoint in breakPoints)
        {
            if (breakPoint.gameObject.activeSelf && breakPoint.IsCritical)
            {
                isBroken = true;
            }
        }

        breakTimer -= Time.deltaTime * throttle;
        if (breakTimer <= 0f)
        {
            Break();
            ResetBreakTimer();
        }
    }

    void ResetBreakTimer()
    {
        breakTimer = Random.Range(minBreakTime, maxBreakTime);
    }

    public void Break()
    {
        var availableBreakpoints = breakPoints.Where(bp => !bp.gameObject.activeSelf).ToArray();
        if (availableBreakpoints.Length == 0)
        {
            return;
        }

        var breakPoint = availableBreakpoints[Random.Range(0, availableBreakpoints.Length)];
        breakPoint.gameObject.SetActive(true);
        breakPoint.Reset();
    }

}
