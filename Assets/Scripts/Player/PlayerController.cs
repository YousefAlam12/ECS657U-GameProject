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

    
    // variables for dash powerup
    public Vector3 dashDirection;
    public float dashDecay = 5f;
    // public float dashPower = 20f;
    // public float dashCooldown = 0.5f;
    // private float nextDashTime = 0f;
    private PlayerInventory inventory;


 

    void Start()
    {
        // using rigid body
        // rb = GetComponent<Rigidbody>();

        // using CharacterController
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
        o2Bar = FindAnyObjectByType<OxygenBar>();
        camera = FindAnyObjectByType<CameraController>();
        inventory = GetComponent<PlayerInventory>();
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

    // player throws grabbed obj 
    public void OnThrow(InputValue value) {
        if(value.isPressed) {

            // prioritise throwing an object that is being held
            if(pickingUp != null && grabable == pickingUp)
            {                
                pickingUp.transform.parent = null;

                // resets rb constraints in order to add force to object
                pickingUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                pickingUp.GetComponent<Rigidbody>().AddForce(transform.forward * 50000f, ForceMode.Impulse);

                // Set the thrown state to true
                pickingUp.GetComponent<PickupProjectile>().SetThrownState(true);
                
                canDrop = false;
                pickingUp = null;
                o2Bar.oxygen -= 3f;
            }
        }
    }

    // public void OnFire(InputValue value) {
    //     if (value.isPressed && Time.time >= nextDashTime) {  // Check cooldown
    //         if (moveInput != Vector2.zero) {
    //             // Calculate dash direction based on current movement direction
    //             dashDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized * dashPower;
    //             nextDashTime = Time.time + dashCooldown;  // Update next dash time
    //         }
    //     }
    // }

    // activates the players powerup ability
    public void OnFire(InputValue value)
    {
        if (value.isPressed && inventory.isPoweredup)
        {
            // inventory.ability();
            inventory.usePowerUp();
        }
    }



    void MovePlayer()
    {       
        if(knockBackCounter <= 0 ) 
        {
            // stores the y position of the player before calculating the movement of player with mouse in order allow jumps
            float yStore = moveDirection.y;

            // using the mouse to move the player in chosen directions
            moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            // Apply dash if active
            moveDirection += dashDirection;

            // Decay the dash direction over time
            dashDirection = Vector3.Lerp(dashDirection, Vector3.zero, dashDecay * Time.deltaTime);


            // // prevents grabbed object from going beneath the player
            // if(grabable == pickingUp && pickingUp != null && pickingUp.transform.position.y < transform.position.y+1)
            // {
            //     pickingUp.transform.position = new Vector3(camera.pivot.position.x, transform.position.y+3, camera.pivot.position.z);
            // }

            // prevents picked up object from moving around
            if (pickingUp != null)
            {
                float objectHeight = pickingUp.GetComponent<Collider>().bounds.size.y;
                float minY = transform.position.y + 1.2f;
                float maxY = transform.position.y + 0.2f + objectHeight;

                // float clampedY = Mathf.Min(pickingUp.transform.position.y, transform.position.y + objectHeight + 0.2f);
                // float clampedY = Mathf.Min(pickingUp.transform.position.y, transform.position.y + 1.2f);
                float clampedY = Mathf.Clamp(pickingUp.transform.position.y, minY, maxY);

                pickingUp.transform.position = new Vector3(pickingUp.transform.position.x, clampedY, pickingUp.transform.position.z);
            }

            if(controller.isGrounded)
            {
                if(isSprinting) {
                    moveSpeed = sprintSpeed;
                    
                    // only lose O2 when actually moving
                    if (moveInput.x != 0 || moveInput.y != 0)
                    {
                        o2Bar.oxygen -= 5f * Time.deltaTime;
                    }
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

            // prevents you from being unable to do actions while touching another obj
            if (pickingUp != null) {
                grabable = pickingUp;
                pickingUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
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
        obj.transform.position = new Vector3(camera.pivot.position.x, transform.position.y+3.2f, camera.pivot.position.z);
        // obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        obj.transform.parent = transform;
        canDrop = true;
    }

    // stops the pickup obj from following the player
    void Drop(GameObject obj) {
        if(pickingUp != null && grabable == pickingUp) {
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
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
