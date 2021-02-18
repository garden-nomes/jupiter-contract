using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBreakPoint : MonoBehaviour, IInteractible
{
    public PlayerController fixingPlayer;
    public float timeToFix = 3f;
    public float timeUntilCritical = 60f;
    public float nonCriticalParticleRate = 5f;
    public float criticalParticleRate = 20f;
    public new ParticleSystem particleSystem;

    public bool CanInteract() => true;
    public string GetActionText(PlayerController player) => "(hold) fix engine";

    public bool IsCritical => criticalTimer > timeUntilCritical;

    private float criticalTimer = 0f;
    private float fixedTimer = 0f;

    void Update()
    {
        if (fixingPlayer != null)
        {
            fixedTimer += Time.deltaTime;
            fixingPlayer.Progress = fixedTimer / timeToFix;

            if (fixedTimer >= timeToFix)
            {
                fixingPlayer.hasControl = true;
                fixingPlayer.Progress = null;
                fixingPlayer = null;
                gameObject.SetActive(false);
            }
            else if (fixingPlayer.input.GetBtnUp(2))
            {
                fixingPlayer.hasControl = true;
                fixingPlayer.Progress = null;
                fixingPlayer = null;
            }
        }

        criticalTimer += Time.deltaTime;
        var emission = particleSystem.emission;
        emission.rateOverTime = IsCritical ? criticalParticleRate : nonCriticalParticleRate;
    }

    public void Reset()
    {
        criticalTimer = 0f;
    }

    public void Interact(PlayerController player)
    {
        fixingPlayer = player;
        player.hasControl = false;
        fixedTimer = 0f;
    }
}
