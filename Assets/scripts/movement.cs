using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    public float jumpForce = 10.0f; // Adjust jump force as needed
    private Rigidbody2D rb2D;
    private Animator anim;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb2D.gravityScale = 30.0f; // Set gravity scale to 1 (normal gravity)
    }

    private void Update()
    {
        UpdateState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement.Normalize();

        Vector2 velocity = movement * movementSpeed;
        velocity.y = rb2D.velocity.y; // Maintain current y velocity for jumping
        rb2D.velocity = velocity;

        // Apply gravity (assuming you want to keep using AddForce for gravity)
        rb2D.AddForce(Vector2.down * rb2D.gravityScale);
    }

    private void UpdateState()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        bool isRunning = Mathf.Abs(moveHorizontal) > 0.1f;

        anim.SetBool("isRunning", isRunning);

        if (moveHorizontal < 0) // If moving left, flip the character
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        }
        else if (moveHorizontal > 0) // If moving right, reset the scale
        {
            transform.localScale = new Vector3(1, 1, 1); // Reset scale
        }
    }

    private void Jump()
    {
        if (Mathf.Abs(rb2D.velocity.y) < 0.01f) // Check if the character is grounded
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true); // Set isJumping to true when jumping
            anim.SetTrigger("takeoff"); // Set takeoff trigger when jumping
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJumping", false); // Set isJumping to false when landing on the ground
        }
    }
}
