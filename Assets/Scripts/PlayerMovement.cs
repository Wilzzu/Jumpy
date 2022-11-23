using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables for jumping
    private float jumpForce;
    private float jumpDirection;
    private bool jumpDirectionSet = false;
    private Rigidbody2D rb;

    // Variables for landing
    [SerializeField] private int neededLandingTime;
    private bool hasLanded;
    private int timeOnPlatform = 0;
    private IEnumerator platformTimer;

    // Get player rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Jumping
    private void Update()
    {
        // Check if player has stood on a platform long enough
        if (timeOnPlatform >= neededLandingTime)
        {
            // Stop timer
            if (platformTimer != null) StopCoroutine(platformTimer);
            hasLanded = true;

            // After this the arrows will start rotating and you can choose jump direction
        }

        if (Input.GetButtonDown("Jump") && hasLanded)
        {
            // Set a jump direction on first jump press
            if (!jumpDirectionSet)
            {

                jumpDirection = 0; // replace this with real value taken from user input
                Debug.Log("Jump direction set to: " + jumpDirection);
                jumpDirectionSet = true;
            }

            // Set jump force on second jump press and jump
            else
            {

                jumpForce = 550; // replace this with real value taken from user input
                rb.AddForce(new Vector2(jumpDirection, jumpForce));
                Debug.Log("Jumped with force of: " + jumpForce);

                // After jumping reset variables
                jumpDirectionSet = false;
                hasLanded = false;
                timeOnPlatform = 0;
            }
        }
    }


    // Check when player lands on a platform
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // Start timer to check how long player stands on a platform
            if (platformTimer != null) StopCoroutine(platformTimer);
            platformTimer = PlatformTimer();
            StartCoroutine(platformTimer);
        }
    }

    // Check when player leaves a platform
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // Stop timer and reset landing variables
            if (platformTimer != null) StopCoroutine(platformTimer);
            hasLanded = false;
            timeOnPlatform = 0;
        }
    }

    // Timer for counting how long player has stayed on a platform
    private IEnumerator PlatformTimer()
    {
        int time = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            time++;
            Debug.Log("Time: " + time + "/" + neededLandingTime);
            if (time >= neededLandingTime) Debug.Log("Jumping enabled");
            timeOnPlatform = time;
        }
    }


}
