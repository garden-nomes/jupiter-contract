using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMagBootsOn = false;
    public Sprite[] frames;

    private MovementController movementController;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        movementController = GetComponent<MovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        movementController.isMagBootsOn = isMagBootsOn;
        movementController.Move(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.Z))
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
