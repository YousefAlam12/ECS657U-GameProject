using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    // PlayerInput playerInput;
    // InputAction moveAction;
    // private Rigidbody rb;

    // speed variables
    public float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    private bool isSprinting = false;

    // jump variables 
    public float jumpForce = 8f;
    public float gravityMultiplier = 1.4f;
    private bool isWallJump = false;
    private float wallStore;

    // knockback variables 
    public float knockBackForce = 5f;
    public float knockBackTime = 0.5f;
    private float knockBackCounter;
    

    // movement variables
    private Vector3 moveDirection;
    public Vector2 moveInput;
    public Vector2 lookInput;

    public CharacterController controller;
    public OxygenBar o2Bar;

    // pickup variables
    private bool canDrop = false;
    public GameObject grabable;
    public GameObject pickingUp;
    private new CameraController camera;


 

    void Start()
    {
        // using rigid body
        // rb = GetComponent<Rigidbody>();

        // using CharacterController
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
        o2Bar = FindAnyObjectByType<OxygenBar>();
        camera = FindAnyObjectByType<CameraController>();
    }

    // Moving function which reads the value of the direction moving in on button press
    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();

        if(controller.isGrounded) {
            moveDirection.y = 0;
        }
    }

    // Jump function which reads when jump button is pressed, applies jump when player is grounded
    // Handles wall jumps
    void OnJump(InputValue value)
    {
        if (controller.isGrounded)
        {
            if(value.isPressed)
            {
                moveDirection.y = jumpForce;
                isWallJump = false;
            }
            else
            {
                moveDirection.y = 0f;
            }
        }
        // checks if gravity is acting to prevent heavily delaying walljump
        // if(isWallJump && moveDirection.y > 0) {
        //     moveDirection.y = 0;
        //     if(value.isPressed && moveInput.x == wallStore)
        //     {
        //         Knockback(new Vector3((moveDirection.x/2)*-1, 2f, (moveDirection.z/2)*-1));
        //         isWallJump = false;
        //     }
        // }

        if(isWallJump) {
            moveDirection.y = 0;
            if(value.isPressed)
            {
                Knockback(new Vector3((moveDirection.x/2)*-1, 2f, (moveDirection.z/2)*-1));
                isWallJump = false;
            }
        }
    }

    // Sprint function sets the sprinting state to true when button is pressed and false when released
    void OnSprint(InputValue value) {
            if(value.isPressed) {
                isSprinting = true;
            }
            else {
                isSprinting = false;
            }
    }

    // for the camera to move with player
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    // player grabs pickup objects and is also able to drop them
    public void OnGrab(InputValue value) {
        if(value.isPressed && grabable != null && !canDrop) {
            pickingUp = grabable;
            Pickup(pickingUp);
        }
        else {
            Drop(pickingUp);
        }
    }



    void MovePlayer()
    {       
        // using rigid body
        // rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y, Input.GetAxis("Vertical") * moveSpeed);

        // if(Input.GetButtonDown("Jump")) {
        //     rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        // }


        // using CharacterController for 3D movement 

        // moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);


        if(knockBackCounter <= 0 ) 
        {
            // stores the y position of the player before calculating the movement of player with mouse in order allow jumps
            float yStore = moveDirection.y;

            // using the mouse to move the player in chosen directions
            // moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            // prevents grabbed object from going beneath the player
            if(grabable == pickingUp && pickingUp != null && pickingUp.transform.position.y < transform.position.y+1)
            {
                pickingUp.transform.position = new Vector3(camera.pivot.position.x, transform.position.y+3, camera.pivot.position.z);
            }

            // // preventing infinite jumps
            // if (controller.isGrounded)
            // {
            //     //moveDirection.y = 0f;

            //     // if(Input.GetButtonDown("Jump")) {
            //     //if (Keyboard.current.spaceKey.wasPressedThisFrame)
            //     //{
            //     //    moveDirection.y = jumpForce;
            //     //}

            //     // sprint button
            //     // if(Input.GetButton("Fire3")) {
            //     if (Keyboard.current.leftShiftKey.isPressed)
            //     {
            //         moveSpeed = sprintSpeed;
            //     }
            //     else
            //     {
            //         moveSpeed = walkSpeed;
            //     }
            // }

            if(controller.isGrounded)
            {
                if(isSprinting) {
                    moveSpeed = sprintSpeed;
                    // o2Bar.oxygen -= 5f * Time.deltaTime;
                }
                else {
                    moveSpeed = walkSpeed;
                }
            }
        } 
        else 
        {
            knockBackCounter -= Time.deltaTime;
        }

        // adding gravity to player
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityMultiplier * Time.deltaTime);

        // prevents movement from scaling off of frame rate
        controller.Move(moveDirection * Time.deltaTime);
    }

    // Allows wall jumps to be done when touching an object with the WallJump tag
    // void OnControllerColliderHit(ControllerColliderHit other) {
    //     if(other.gameObject.tag == "WallJump")
    //     {
    //         // Debug.Log("hit something");
    //         isWallJump = true;
    //         wallStore = moveInput.x;
    //     }
    //     else {
    //         isWallJump = false;
    //         wallStore = 0;
    //     }   
    // }

    // checks collisions for different types of environment objects
    void OnCollisionEnter(Collision other) {

        // checks collision with walls that are able to be walljumped
        if(other.gameObject.tag == "WallJump")
        {
            isWallJump = true;
            wallStore = moveInput.x;
        }

        // checks collisions with objects that are able to be picked up
        if(other.gameObject.tag == "Pickup") {
            grabable = other.gameObject;
        }
    }

    // when exiting collisions with environment objects
    void OnCollisionExit(Collision other) {

        if(other.gameObject.tag == "WallJump")
        {
            isWallJump = false;
            wallStore = 0;
        }

        if(other.gameObject.tag == "Pickup") {
            grabable = null;
        }
    }

    public void Knockback(Vector3 direction) {
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
    }

    // sets the pickup obj to follow the players movement
    void Pickup(GameObject obj) {
        // obj.transform.position = new Vector3(camera.pivot.position.x, 5f, camera.pivot.position.z+0.4f);
        obj.transform.position = new Vector3(camera.pivot.position.x, transform.position.y+3, camera.pivot.position.z);
        obj.transform.parent = transform;
        canDrop = true;
    }

    // stops the pickup obj from following the player
    void Drop(GameObject obj) {
        if(pickingUp != null && grabable == pickingUp) {
            obj.transform.parent = null;
            canDrop = false;
            pickingUp = null;
        }
    }

    void Update()
    {
        MovePlayer();
    }


}
