using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Variable for main camera
    private Camera cam;

    // Variables for following player
    [Header("Following player")]
    [SerializeField] private Transform player;
    [SerializeField] private float smoothAmount;
    private Vector3 velocity = Vector3.zero;

    // Variables for zooming
    [Header("Zooming")]
    [SerializeField] private bool zoomed = true;
    [SerializeField] private float zoomSpeed;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    // Check if camera is zoomed and move it
    void FixedUpdate()
    {
        if (zoomed) MoveCamera(5, 2);
        else MoveCamera(8, 7);
    }

    // Follow player with camera and apply given zoom amount and offset
    private void MoveCamera(int zoomAmount, int verticalOffset)
    {
        Vector3 offset = new Vector3(0, verticalOffset, -10);
        Vector3 playerPos = player.position + offset;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomAmount, zoomSpeed);
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothAmount);
    }
}
