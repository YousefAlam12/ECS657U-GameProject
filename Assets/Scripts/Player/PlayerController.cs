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

    public float knockBackForce = 5f;
    public float knockBackTime = 0.5f;
    private float knockBackCounter;
    

    // movement variables
    private Vector3 moveDirection;
    public Vector2 moveInput;
    public Vector2 lookInput;

    public CharacterController controller;


 

    void Start()
    {
        // using rigid body
        // rb = GetComponent<Rigidbody>();

        // using CharacterController
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
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
        if(isWallJump && moveDirection.y > 0) {
            moveDirection.y = 0;
            if(value.isPressed && moveInput.x == wallStore)
            {
                Knockback(new Vector3((moveDirection.x/2)*-1, 2f, (moveDirection.z/2)*-1));
                isWallJump = false;
            }
        }
    }

    // Sprint function sets the sprinting state to true when button is pressed and false when released
    void OnSprint(InputValue value) {
            if(value.isPressed) {
                // Debug.Log("pressed: " + value);
                isSprinting = true;
            }
            else {
                // Debug.Log("released: " + value);
                isSprinting = false;
            }
    }

    // for the camera to move with player
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
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

            if(controller.isGrounded) {
                if(isSprinting) {
                    moveSpeed = sprintSpeed;
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
    void OnControllerColliderHit(ControllerColliderHit other) {
        if(other.gameObject.tag == "WallJump")
        {
            // Debug.Log("hit something");
            isWallJump = true;
            wallStore = moveInput.x;
        }
        else {
            isWallJump = false;
            wallStore = 0;
        }
    }

    public void Knockback(Vector3 direction) {
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
    }

    void Update()
    {
        MovePlayer();
    }


}
