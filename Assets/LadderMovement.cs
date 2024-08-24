using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && !isClimbing && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
        
        
        if (!isLadder && !isClimbing)
        {
            isClimbing = false;
            rb.gravityScale = 4f;
        }

        Debug.Log($"Is Ladder: {isLadder}, Is Climbing: {isClimbing}, Vertical Input: {vertical}");
    
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else if (!isLadder && !isClimbing)
        {
            rb.gravityScale = 4f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        else if (collision.CompareTag("LadderTop"))
        {
            isClimbing = false;
            rb.gravityScale = 4f;  // Stelle die Schwerkraft nur wieder her, wenn der Spieler die Leiter wirklich verlässt
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
    
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            if (isClimbing)
            {
                isClimbing = false;
                rb.gravityScale = 4f;
            }
        }
    }    
}