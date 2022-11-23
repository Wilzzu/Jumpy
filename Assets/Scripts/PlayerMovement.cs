using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Initializing needed variables for movement
    [SerializeField] private float jumpForce;
    private float direction;
    private Rigidbody2D rb;

    private bool hasLanded;

    // Get player rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Jumping
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && hasLanded)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
        }
    }

    // Check when player lands on a platform
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            hasLanded = true;
        }
    }

    // Check when player leaves a platform
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            hasLanded = false;
        }
    }


}
