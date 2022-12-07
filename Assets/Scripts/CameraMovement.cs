using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Variables for game objects
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
    [SerializeField] private float[] zoomAmount = new float[] { 14, 12 };
    private float currentPosition;
    private bool isMobile;
    private Vector3 minBoundVal, maxBoundVal;


    private void Start()
    {
        // Get important components
        cam = gameObject.GetComponent<Camera>();
        rb = player.GetComponent<Rigidbody2D>();
        isMobile = GameManager.instance.isMobile;
    }


    private void FixedUpdate()
    {

        // Assign camera values depending on if player has zoomed and if they are on mobile
        if (isMobile)
        {
            if (zoomed)
            {
                zoomAmount = new float[] { 8.5f, 3 };
                minBoundVal = new Vector3(-4, 3.5f, -10);
                maxBoundVal = new Vector3(4, 54, -10);
            }
            else
            {
                zoomAmount = new float[] { 12, 10 };
                minBoundVal = new Vector3(-2.1f, 3.5f, -10);
                maxBoundVal = new Vector3(2.1f, 50.5f, -10);
            }
        }
        else
        {
            if (zoomed) zoomAmount = new float[] { 6, 3 };
            else zoomAmount = new float[] { 11, 10 };
        }



        // Check if player is moving fast vertically and change camera position variables if needed
        bool playerLanded = player.gameObject.GetComponent<PlayerScript>().hasLanded;
        if (playerLanded) moveVert = false;
        else if (Mathf.Abs(rb.velocity.y) > 5 && !moveVert) moveVert = true;


        // Finally move the camera
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
    public void changeZoom(bool jumped)
    {
        if (jumped) zoomed = true;
        else if (zoomed) zoomed = false;
        else zoomed = true;
    }

    // Follow player with camera and apply given zoom amount and offset
    private void MoveCamera(float zoomAmount, float verticalOffset)
    {
        Vector3 offset = new Vector3(0, verticalOffset, -10);
        Vector3 playerPos;

        // If player is on mobile follow them on the x-axis as well
        if (isMobile) playerPos = player.position + offset;
        else playerPos = new Vector3(0, player.position.y, 0) + offset;

        // Make zooming smoother and add borders to mobile view
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomAmount, zoomSpeed);
        Vector3 boundPosition = new Vector3(Mathf.Clamp(playerPos.x, minBoundVal.x, maxBoundVal.x), Mathf.Clamp(playerPos.y, minBoundVal.y, maxBoundVal.y), -10);

        // Lastly move the camera
        if (isMobile) transform.position = Vector3.SmoothDamp(transform.position, boundPosition, ref velocity, smoothAmount);
        else transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothAmount);
    }
}
