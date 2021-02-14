using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    [Range(0, .3f)] public float movementSmoothing = .05f;
    public float moveSpeed = 8f;
    public float pushOffSpeed = 1f;
    public LayerMask groundMask;
    public Transform groundCheckMarker;
    public bool canLand = false;
    public PhysicsMaterial2D wallMaterial;

    const float GroundedRadius = .2f;
    private bool grounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
    private PhysicsMaterial2D bouncyMaterial;
    private PhysicsMaterial2D nonBouncyMaterial;

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        bouncyMaterial = new PhysicsMaterial2D();
        bouncyMaterial.bounciness = 1f;
        bouncyMaterial.friction = 0f;

        nonBouncyMaterial = new PhysicsMaterial2D();
        nonBouncyMaterial.bounciness = 0f;
        nonBouncyMaterial.friction = 0f;
    }

    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckMarker.position, GroundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;

                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }

        if (grounded && canLand)
        {
            rb.sharedMaterial = nonBouncyMaterial;
        }
        else
        {
            rb.sharedMaterial = bouncyMaterial;
        }
    }

    public void Move(float move)
    {

        if (grounded)
        {
            Vector3 targetVelocity = new Vector2(move * moveSpeed, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

            if (!canLand)
            {
                var velocity = rb.velocity;
                velocity.y += velocity.x * pushOffSpeed * Time.deltaTime;
                rb.velocity = velocity;
            }

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
