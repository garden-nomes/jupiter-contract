using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMagBootsOn = false;
    public Sprite[] frames;
    public bool hasControl = true;
    public PlayerInput input => _input;

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
        if (hasControl)
        {
            movementController.isMagBootsOn = isMagBootsOn;
            movementController.Move(input.horizontal, input.vertical);

            if (input.GetBtnDown(0))
            {
                isMagBootsOn = !isMagBootsOn;
            }

            if (input.GetBtnDown(2))
            {
                foreach (var collider in Physics2D.OverlapCircleAll(transform.position, .1f))
                {
                    var interactible = collider.GetComponent<IInteractible>();

                    if (interactible != null && interactible.CanInteract())
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
