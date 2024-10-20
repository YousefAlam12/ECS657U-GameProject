using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public Transform focalPoint;
    public Vector3 offset;
    public float mouseSensitivity = 0.04f;
    public Transform pivot;

    public float maxViewAngle;
    public float minViewAngle;

    private Vector2 lookInput;
    public PlayerController player;


    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }


    void Start()
    {
        offset = focalPoint.position - transform.position;
        player = FindAnyObjectByType<PlayerController>();

        // pivot used as intermediate object to rotate camera vertically instead of directly the player (prevent player from rotating verticaly)
        pivot.transform.position = focalPoint.transform.position;
        pivot.transform.parent = focalPoint.transform;

        // hide mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    // use LateUpdate to prevent camera jerking
    void LateUpdate()
    {
        // get mouseX position and rotate player on its Y-axis based on the value to move horizontaly
        //float horizontal = Input.GetAxis("Mouse X") * mouseSensitivity;
         float horizontal = lookInput.x * mouseSensitivity;
        focalPoint.Rotate(0, horizontal, 0);

        // get mouseY position and rotate pivot (prevent player from rotating verticaly) on its X-axis based on the value to move verticaly
        //float vertical = Input.GetAxis("Mouse Y") * mouseSensitivity;
         float vertical = lookInput.y * mouseSensitivity;
        pivot.Rotate(-vertical, 0, 0);

        // putting limits on the pivot to prevent camera from flipping
        if(pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f) {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }

        if(pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle) {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
        }

        // rotating camera to follow the players rotation
        // converting player rotation into quaternion angle to resposition the camera
        float cameraAdjustY = focalPoint.eulerAngles.y;
        float cameraAdjustX = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(cameraAdjustX, cameraAdjustY, 0);
        transform.position = focalPoint.position - (rotation * offset);

        // stop camera from going below the stage by zooming in on player when too low
        if(transform.position.y < focalPoint.position.y) {
            transform.position = new Vector3(transform.position.x, focalPoint.position.y - .5f, transform.position.z);
        }

        // repositioning the camera with the offset
        transform.LookAt(focalPoint);
    }

    void Update() {
        lookInput = player.lookInput;
    }
}
