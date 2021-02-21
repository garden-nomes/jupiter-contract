using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{
    public AudioClip blip;
    public AudioClip crash;
    public AudioSource laserSound;
    public AudioSource engineRumble;

    public void Blip()
    {
        AudioSource.PlayClipAtPoint(blip, Vector3.zero);
    }

    public void Crash()
    {
        AudioSource.PlayClipAtPoint(crash, Vector3.zero);
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
