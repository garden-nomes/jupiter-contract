﻿using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMagBootsOn = false;
    public Sprite[] frames;
    public bool hasControl = true;
    public PlayerInput input => _input;
    public ProgressBar progressBar;
    public StationBehaviour station;

    public float tiredness = 0f;
    public float tirednessTime = 60f;

    public bool IsExhausted => tiredness >= 1f;

    [HideInInspector] public string instructionText = "";

    public float? Progress
    {
        get => progressBar.gameObject.activeSelf ? progressBar.progress : (float?) null;
        set
        {
            if (value == null)
            {
                progressBar.gameObject.SetActive(false);
            }
            else
            {
                progressBar.gameObject.SetActive(true);
                progressBar.progress = value.Value;
            }
        }
    }

    private MovementController movementController;
    private SpriteRenderer spriteRenderer;
    private PlayerInput _input;

    void Start()
    {
        movementController = GetComponent<MovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        instructionText = "";

        tiredness += Time.deltaTime / tirednessTime;
        tiredness = Mathf.Clamp01(tiredness);

        if (hasControl)
        {
            instructionText = input.GetInstructionText();

            movementController.isMagBootsOn = isMagBootsOn;
            movementController.Move(input.horizontal, input.vertical);

            if (input.GetBtnDown(0))
            {
                isMagBootsOn = !isMagBootsOn;
            }

            foreach (var collider in Physics2D.OverlapCircleAll(transform.position, .1f))
            {
                var interactible = collider.GetComponent<IInteractible>();

                if (interactible != null && interactible.CanInteract())
                {
                    string actionText = interactible.GetActionText(this);
                    instructionText = $"{Icons.IconText(input.inputScheme.btn2)} {actionText}";

                    if (input.GetBtnDown(2))
                    {
                        interactible.Interact(this);
                        break;
                    }
                }
            }
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (this.isMagBootsOn)
        {
            int frame = Mathf.FloorToInt(Time.time * 8) % frames.Length;
            spriteRenderer.sprite = frames[frame];
        }
        else
        {
            spriteRenderer.sprite = frames[0];
        }
    }
}
