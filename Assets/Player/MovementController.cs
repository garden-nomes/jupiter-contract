using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    public float movementSmoothing = .05f;
    public float moveSpeed = 8f;
    public float floatingMoveSpeed = 2f;
    public float floatingMoveSmoothing = .75f;
    public float pushOffSpeed = 1f;
    public LayerMask groundMask;
    public Transform groundCheckMarker;
    public bool isMagBootsOn = false;
    public float magBootForce = 500f;
    public float magBootSpeed = .75f;
    public bool IsOverLadder => isOverLadder;
    public bool IsGrounded => isGrounded;

    const float GroundedRadius = 1f / 16f;
    private bool isGrounded;
    private bool isOverLadder;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
    private PhysicsMaterial2D bouncyMaterial;
    private PhysicsMaterial2D nonBouncyMaterial;

    bool CanStayOnGround => Mathf.Abs(Physics2D.gravity.y) >.1f || isMagBootsOn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        bouncyMaterial = new PhysicsMaterial2D();
        bouncyMaterial.bounciness = 1f;
        bouncyMaterial.friction = 0f;

        nonBouncyMaterial = new PhysicsMaterial2D();
        nonBouncyMaterial.bounciness = 0f;
        nonBouncyMaterial.friction = 0f;
    }

    private void FixedUpdate()
    {
        isGrounded = false;
        isOverLadder = false;

        var colliders = Physics2D.OverlapCircleAll(groundCheckMarker.position, GroundedRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            bool isWall = ((1 << colliders[i].gameObject.layer) & groundMask) != 0;

            if (!colliders[i].isTrigger && isWall && colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
            else if (colliders[i].CompareTag("ladder"))
            {
                isOverLadder = true;
            }
        }

        if (isGrounded && CanStayOnGround)
        {
            rb.sharedMaterial = nonBouncyMaterial;
        }
        else
        {
            rb.sharedMaterial = bouncyMaterial;
        }
    }

    public void Move(float horizontal, float vertical)
    {
        if (isGrounded || isOverLadder)
        {
            var speed = isMagBootsOn ? moveSpeed * magBootSpeed : moveSpeed;

            Vector3 targetVelocity = new Vector2(horizontal * speed, rb.velocity.y);

            if (isOverLadder)
            {
                targetVelocity.y = vertical * speed;
            }

            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

            if (!CanStayOnGround)
            {
                var velocity = rb.velocity;
                velocity.y += Mathf.Abs(velocity.x) * pushOffSpeed * Time.deltaTime;
                rb.velocity = velocity;
            }

            if (horizontal > 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontal < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            var targetVelocity = new Vector2(horizontal, vertical) * floatingMoveSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, floatingMoveSmoothing);

            if (isMagBootsOn)
            {
                float minDistance = float.PositiveInfinity;

                var hits = Physics2D.RaycastAll(transform.position, Vector2.down, 100);
                foreach (var hit in hits)
                {
                    if (!hit.collider.isTrigger &&
                        hit.collider.gameObject != gameObject &&
                        hit.distance < minDistance)
                    {
                        minDistance = hit.distance;
                    }
                }

                rb.velocity += Vector2.down * (magBootForce / minDistance);
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
