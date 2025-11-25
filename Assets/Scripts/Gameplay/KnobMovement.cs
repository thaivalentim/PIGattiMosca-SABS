using UnityEngine;

public class KnobMovement : MonoBehaviour
{
    public float speed = 5f;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }
    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.velocity = movement * speed;
    }
}