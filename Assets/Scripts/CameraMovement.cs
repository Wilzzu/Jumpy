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

    private void Start()
    {
        // Get important components
        cam = gameObject.GetComponent<Camera>();
        rb = player.GetComponent<Rigidbody2D>();
        isMobile = GameManager.instance.isMobile;
    }


    private void FixedUpdate()
    {
        /*
        // Assign camera values depending on if player has zoomed or not
        if (isMobile)
        {
            if (zoomed) zoomAmount = new float[] { 8, 2 };
            else zoomAmount = new float[] { 14, 12 };
        }
        else
        {
            if (zoomed) zoomAmount = new float[] { 6, 3 };
            else zoomAmount = new float[] { 11, 10 };
        }
*/


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
        if (!isMobile) playerPos = player.position + offset; // follow player on x and y axis HUUTIS POIS
        else playerPos = new Vector3(0, player.position.y, 0) + offset;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomAmount, zoomSpeed);
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothAmount);
    }
}
