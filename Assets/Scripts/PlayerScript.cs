using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Variables for jumping
    private bool jumpDirectionPhase = false;
    private bool jumpForcePhase = false;
    private Rigidbody2D rb;
    private bool firstTimeLanding = true;

    // Variables for aiming
    [SerializeField] private Transform aim;
    [SerializeField] private float rotationSpeed;
    private bool changeJumpRotation = false;
    private bool changeJumpForce = false;
    private IEnumerator jumpForceTimer;
    private float jumpDirection;
    private float jumpForce = 0;


    // Variables for landing
    [SerializeField] private int neededLandingTime;
    private int timeOnPlatform = 999;
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
            timeOnPlatform = 0;

            // After this the arrow will start rotating and you can choose jump direction
            aim.gameObject.SetActive(true);
            aim.localRotation = Quaternion.Euler(0, 0, -5f);
            aim.localScale = new Vector3(1, 1, 1);
            jumpDirectionPhase = true;
        }

        if (jumpDirectionPhase)
        {
            // Rotate arrow
            float angle = aim.eulerAngles.z;
            if (angle >= 355) changeJumpRotation = true;
            if (angle <= 185) changeJumpRotation = false;

            if (changeJumpRotation) aim.Rotate(new Vector3(0, 0, -1 * rotationSpeed) * Time.deltaTime);
            else aim.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
        }

        // Get jump force
        if (jumpForcePhase)
        {
            if (jumpForce >= 100) changeJumpForce = true;
            if (jumpForce <= 0) changeJumpForce = false;
            if (changeJumpForce) jumpForce--;
            else jumpForce++;

            aim.localScale = new Vector3(jumpForce / 100, 1, 1);
        }


        if (Input.GetButtonDown("Jump"))
        {

            // Set jump force on second jump press and jump
            if (jumpForcePhase)
            {
                // Making the player jump to right direction with right force
                float force = Mathf.Clamp(jumpForce / 4, 0.5f, 25);
                Vector3 direction = new Vector3((jumpDirection - 270) * -1, force, 1).normalized;
                rb.AddForce(direction * force, ForceMode2D.Impulse);

                Debug.Log("Dir: " + ((jumpDirection - 270) * -1) + " Force: " + force);

                // After jumping reset variables
                aim.gameObject.SetActive(false);
                jumpForcePhase = false;
                timeOnPlatform = 0;
                jumpForce = 0;
            }

            // Set a jump direction on first jump press
            if (jumpDirectionPhase)
            {
                jumpDirection = aim.eulerAngles.z;
                jumpDirectionPhase = false;
                jumpForcePhase = true;
            }
        }
    }


    // Check when player lands on a platform
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (firstTimeLanding)
        {
            firstTimeLanding = false;
            return;
        }
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
