using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;

    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");

        movementController.canLand = Mathf.Abs(Physics2D.gravity.y) > 0.1f;
        movementController.Move(horizontal);
    }
}
