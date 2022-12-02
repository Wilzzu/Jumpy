using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Variable for game objects
    private Camera cam;
    private Rigidbody2D rb;

    // Variables for camera attributes
    private float smoothAmount = 0.15f;

    // Variables for following player
    [SerializeField] private Transform player;
    private Vector3 velocity = Vector3.zero;
    private bool moveVert = false;

    // Variables for zooming
    [SerializeField] private float zoomSpeed;
    [SerializeField] private bool zoomed = true;
    private float[] zoomAmount = new float[] { 8, 7 };
    private float currentPosition;

    private void Start()
    {
        // Get important components
        cam = gameObject.GetComponent<Camera>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Assign camera values depending on if player has zoomed or not
        // Can be used in the future if needed
        if (zoomed) zoomAmount = new float[] { 5, 3 };
        else zoomAmount = new float[] { 8, 7 };

        // Move camera vertically when player is moving fast vertically
        bool playerLanded = player.gameObject.GetComponent<PlayerScript>().hasLanded;
        if (playerLanded) moveVert = false;
        else if (Mathf.Abs(rb.velocity.y) > 5 && !moveVert) moveVert = true;


        // Move camera vertically if needed
        if (moveVert)
        {
            currentPosition = Mathf.Lerp(currentPosition, zoomAmount[1] - 2, 1.5f * Time.deltaTime); ;
            MoveCamera(zoomAmount[0], currentPosition);
        }
        else
        {
            currentPosition = Mathf.Lerp(currentPosition, zoomAmount[1], 7 * Time.deltaTime);
            MoveCamera(zoomAmount[0], currentPosition);
        }
    }

    // Change zoom when player presses the zoom key
    public void changeZoom()
    {
        Debug.Log("Change zoom");
    }

    // Follow player with camera and apply given zoom amount and offset
    private void MoveCamera(float zoomAmount, float verticalOffset)
    {
        Vector3 offset = new Vector3(0, verticalOffset, -10);
        // Vector3 playerPos = player.position + offset; // follow player on x and y axis
        Vector3 playerPos = new Vector3(0, player.position.y, 0) + offset;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomAmount, zoomSpeed);
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothAmount);
    }
}
