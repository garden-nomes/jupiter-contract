using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMagBootsOn = false;
    public Sprite[] frames;

    private MovementController movementController;
    private SpriteRenderer spriteRenderer;
    private PlayerInput input;

    void Start()
    {
        movementController = GetComponent<MovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        movementController.isMagBootsOn = isMagBootsOn;
        movementController.Move(input.horizontal, input.vertical);

        if (input.GetBtnDown(0))
        {
            isMagBootsOn = !isMagBootsOn;
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
