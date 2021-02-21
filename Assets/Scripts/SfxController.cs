using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{
    public AudioClip blip;
    public float blipVolume = 1f;
    public AudioClip crash;
    public float crashVolume = 1f;
    public AudioClip startupSound;
    public float startupSoundVolume = 1f;
    public AudioSource laserSound;
    public AudioSource engineRumble;
    public AudioSource oneShotSource;

    void Start()
    {
        oneShotSource.PlayOneShot(startupSound, startupSoundVolume);
    }

    [ContextMenu("Blip")]
    public void Blip()
    {
        oneShotSource.PlayOneShot(blip, blipVolume);
    }

    [ContextMenu("Crash")]
    public void Crash()
    {
        oneShotSource.PlayOneShot(crash, blipVolume);
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
