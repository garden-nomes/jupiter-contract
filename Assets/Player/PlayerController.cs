using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMagBootsOn = false;
    public Sprite[] frames;
    public bool hasControl = true;
    public PlayerInput input => _input;
    public StationBehaviour station;
    public SfxController sfx;

    public float tiredness = 0f;
    public float tirednessTime = 60f;
    public float hunger = 0f;
    public float hungerTime = 40f;

    public bool IsExhausted => tiredness >= 1f || hunger >= 1f;

    [HideInInspector] public string instructionText = "";
    [HideInInspector] public string actionText = null;
    [HideInInspector] public float? progress = null;

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
        hunger += Time.deltaTime / hungerTime;
        hunger = Mathf.Clamp01(hunger);

        if (hasControl)
        {
            instructionText = input.GetInstructionText();

            movementController.isMagBootsOn = isMagBootsOn;
            movementController.Move(input.horizontal, input.vertical);

            if (input.GetBtnDown(0))
            {
                sfx.Blip();
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
