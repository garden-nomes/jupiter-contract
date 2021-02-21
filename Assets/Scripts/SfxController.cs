using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{
    public AudioClip blip;
    public float blipVolume = 1f;
    public AudioClip crash;
    public float crashVolume = 1f;
    public AudioSource laserSound;
    public AudioSource engineRumble;

    [ContextMenu("Blip")]
    public void Blip()
    {
        AudioSource.PlayClipAtPoint(blip, transform.position, blipVolume);
    }

    [ContextMenu("Crash")]
    public void Crash()
    {
        AudioSource.PlayClipAtPoint(crash, transform.position, crashVolume);
    }

    public void SetEngineRumble(float value)
    {
        engineRumble.volume = value;
    }

    public void SetLaserActive(bool active)
    {
        laserSound.volume = active ? 1f : 0f;
    }
}
