using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour
{
    // Variables for accessing other scripts
    private CameraMovement cam;
    public static event Action playerJumped;

    // Variables for jumping
    private bool jumpDirectionPhase = false;
    private bool jumpForcePhase = false;
    private Rigidbody2D rb;
    private bool launchPlayer = false;
    private Vector3 directionVector;
    private float force;

    // Variables for aiming
    [SerializeField] private Transform aim;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float amount;
    private bool changeJumpRotation = false;
    private bool changeJumpForceDirection = false;
    private IEnumerator jumpForceTimer;
    private float jumpDirection;
    private float jumpForce = 0;

    // Variables for landing
    [SerializeField] private int neededLandingTime;
    private int timeNotMoving = 999;
    private IEnumerator notMovingTimer;
    private bool checkingLanding = false;
    private bool firstTimeLanding = true;
    public bool hasLanded = true;
    private bool onFinish = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
    }

    // Jumping
    private void Update()
    {
        // Check if player is not moving and allow them to jump again after some time
        if (rb.velocity.magnitude == 0 && !checkingLanding && !firstTimeLanding)
        {
            // Start timer to check how long player has been stopped for
            if (notMovingTimer != null) StopCoroutine(notMovingTimer);
            notMovingTimer = NotMovingTimer();
            StartCoroutine(notMovingTimer);
        }
        // Don't start a new timer if one is being used already
        else if (rb.velocity.magnitude != 0) checkingLanding = false;

        // Check if player has been stopped for long enough
        if (timeNotMoving >= neededLandingTime)
        {
            // Stop timer
            if (notMovingTimer != null) StopCoroutine(notMovingTimer);
            timeNotMoving = 0;

            // Check if player has finished the level
            if (onFinish) GameManager.instance.LevelFinished();

            // If not add rotating arrow to choose jump direction
            else
            {

                aim.gameObject.SetActive(true);
                aim.eulerAngles = new Vector3(0, 0, 355);
                aim.localScale = new Vector3(1, 1, 1);
                jumpDirectionPhase = true;
                hasLanded = true;
            }
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
            if (jumpForce >= 100) changeJumpForceDirection = true;
            if (jumpForce <= 0) changeJumpForceDirection = false;
            if (changeJumpForceDirection) jumpForce--;
            else jumpForce++;

            aim.localScale = new Vector3(jumpForce / 100, 1, 1);
        }
    }

    // Triggers when player presses jump
    private void OnJump()
    {
        // Set jump force on second jump press and jump
        if (jumpForcePhase)
        {
            // Make the player jump to right direction with right amount of force
            force = Mathf.Clamp((jumpForce * 0.8f) / 5, 0.5f, 20);
            float finalDirection = (jumpDirection - 270) * -1;
            Debug.Log("raw: " + finalDirection + " final: " + Mathf.Clamp(100 / Mathf.Abs(finalDirection) * 10, 0.5f, 25) * 1.5f);
            float verticalAmount = Mathf.Clamp(100 / Mathf.Abs(finalDirection) * 10, 0.5f, 25) * 1.5f;

            // Finally launch the player
            directionVector = new Vector3(finalDirection, verticalAmount, 1).normalized;
            launchPlayer = true;
            playerJumped?.Invoke(); // Using event instead of a public function to show a different way to communicate with other scripts :)
        }

        // Set a jump direction on first jump press
        if (jumpDirectionPhase)
        {
            jumpDirection = aim.eulerAngles.z;
            jumpDirectionPhase = false;
            jumpForcePhase = true;
        }
    }

    // Triggers when player presses zoom button
    private void OnZoom()
    {
        cam.changeZoom();
    }

    // Triggers when player presses leave button
    private void OnExit()
    {
        GameManager.instance.ExitConfirmation();
    }

    // Call physics based jumping in FixedUpdate
    private void FixedUpdate()
    {
        if (launchPlayer)
        {
            launchPlayer = false;

            // Launch player
            rb.AddForce(directionVector * force, ForceMode2D.Impulse);

            // Add rotation to the jump
            if (directionVector.x > 0) rb.AddTorque(-0.5f, ForceMode2D.Impulse);
            else rb.AddTorque(0.5f, ForceMode2D.Impulse);

            // After jumping reset variables used for jumping
            aim.gameObject.SetActive(false);
            jumpForcePhase = false;
            timeNotMoving = 0;
            jumpForce = 0;
            firstTimeLanding = false;
            hasLanded = false;
        }
    }

    // Check when player enters the finish platform
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish") onFinish = true;
    }

    // Check when player leaves the finish platform
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Finish") onFinish = false;
    }

    // Timer for counting how long player hasn't been moving for
    private IEnumerator NotMovingTimer()
    {
        int time = 0;
        checkingLanding = true;
        Debug.Log("Started count");
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            time++;
            if (time >= neededLandingTime) Debug.Log("Jumping enabled");
            timeNotMoving = time;
        }
    }


}
