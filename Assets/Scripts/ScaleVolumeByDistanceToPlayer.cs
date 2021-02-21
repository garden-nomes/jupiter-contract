using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleVolumeByDistanceToPlayer : MonoBehaviour
{
    public PlayerController playerA;
    public PlayerController playerB;
    public AudioSource audioSource;

    public float radius = 4f;

    void Update()
    {
        var toPlayerA = transform.position - playerA.transform.position;
        var toPlayerB = transform.position - playerB.transform.position;
        var closest = toPlayerA.sqrMagnitude < toPlayerB.sqrMagnitude ? toPlayerA : toPlayerB;
        float distance = closest.magnitude;
        audioSource.volume = Mathf.Clamp01((radius - distance) / radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
