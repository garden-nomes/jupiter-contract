using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MiningLaser : MonoBehaviour
{
    public Vector3 target;
    public LineRenderer[] lineRenderers;

    void Update()
    {
        // set endpoints of all LineRenderers to the given target in world space
        foreach (var line in lineRenderers)
        {
            line.SetPosition(1, transform.InverseTransformPoint(target) - line.transform.position);
        }
    }
}
