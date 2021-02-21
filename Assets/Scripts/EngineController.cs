using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public EngineBreakPoint[] breakPoints;
    public Shake shake;
    public SfxController sfx;

    public float throttle = 0f;
    public float minBreakTime = 30f;
    public float maxBreakTime = 60f;

    private bool isBroken;
    public bool IsBroken => isBroken;
    private bool isFaulty;
    public bool IsFaulty => isFaulty;

    public float Thrust => IsBroken ? 0f : throttle;

    public float explosionTime = 1f;
    public float explosionAmplitude = 1f;

    private float breakTimer = 0f;
    private bool wasBorken = false; // deal with it
    private float explosionTimer = 0f;

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
        // update isBroken/isFaulty
        isBroken = false;
        isFaulty = false;
        foreach (var breakPoint in breakPoints)
        {
            if (breakPoint.gameObject.activeSelf)
            {
                isFaulty = true;
                if (breakPoint.IsCritical) isBroken = true;
            }
        }

        breakTimer -= Time.deltaTime * throttle;
        if (breakTimer <= 0f)
        {
            Break();
            ResetBreakTimer();
        }

        // assplodey
        if (!wasBorken && isBroken)
        {
            explosionTimer = explosionTime;
            sfx.Crash();
            wasBorken = true;
        }
        else if (!isBroken)
        {
            wasBorken = false;
        }

        if (explosionTimer > 0f)
        {
            shake.Add((explosionTimer / explosionTime) * explosionAmplitude);
            explosionTimer -= Time.deltaTime;
        }
    }

    void ResetBreakTimer()
    {
        breakTimer = Random.Range(minBreakTime, maxBreakTime);
    }

    [ContextMenu("Break")]
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
