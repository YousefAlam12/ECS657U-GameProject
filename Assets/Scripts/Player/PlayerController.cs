using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
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
    public float dashPower = 30f;
    public float dashCooldown = 0.5f;
    private float nextDashTime = 0f;
    private PlayerInventory inventory;

    // animator reference
    public Animator playerAnimator;

    // pause switch
    private bool isPaused;

    // model for player
    public GameObject playerModel;

    void Start()
    {
        // using CharacterController
        controller = GetComponent<CharacterController>();
        moveSpeed = walkSpeed;
        o2Bar = FindAnyObjectByType<OxygenBar>();
        camera = FindAnyObjectByType<CameraController>();
        inventory = GetComponent<PlayerInventory>();
        isPaused = false;

        // change jump stats to make game easier (adds more leniency to timed jumps)
        if (MainMenuManager.isEasy())
        {
            Debug.Log("easy mode");
            jumpForce = 10f;
        }
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
                PlayerSoundManager soundManager = UnityEngine.Object.FindAnyObjectByType<PlayerSoundManager>();
                if (soundManager != null)
                {
                    soundManager.PlayJumpSound();
                }
            }
            else
            {
                moveDirection.y = 0f;
            }
        }

        if(isWallJump) {
            moveDirection.y = 1;
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
                pickingUp.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward * 50000f, ForceMode.Impulse);

                // Set the thrown state to true
                pickingUp.GetComponent<PickupProjectile>().SetThrownState(true);
                
                canDrop = false;
                pickingUp = null;
                o2Bar.oxygen -= 3f;
            }
        }
    }

    // activates the players powerup ability
    public void OnFire(InputValue value)
    {
        if (value.isPressed && inventory.isPoweredup)
        {
            // inventory.ability();
            inventory.usePowerUp();
        }
    }

    // Pauses the game
    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            isPaused = !isPaused;
            Debug.Log(isPaused);
            FindAnyObjectByType<GameManager>().Pause(isPaused);
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

            // Apply dash if active (for Dash powerup)
            moveDirection += dashDirection;

            // Decay the dash direction over time
            dashDirection = Vector3.Lerp(dashDirection, Vector3.zero, dashDecay * Time.deltaTime);

            moveDirection += conveyorForce;

            // prevents you from being unable to do actions while touching another obj and stops unwanted movement of obj
            if (pickingUp != null)
            {
                grabable = pickingUp;
                float objectHeight = pickingUp.GetComponent<Collider>().bounds.size.y;
                float minY = transform.position.y + 1.2f;
                float maxY = transform.position.y + 0.2f + objectHeight;

                float clampedY = Mathf.Clamp(pickingUp.transform.position.y, minY, maxY);

                pickingUp.transform.position = new Vector3(pickingUp.transform.position.x, clampedY, pickingUp.transform.position.z);
            }

            if(controller.isGrounded)
            {
                if(isSprinting) {
                    moveSpeed = sprintSpeed;
                    
                    // only lose O2 when actually moving
                    if (moveInput.x != 0 || moveDirection.z != 0)
                    {
                        // Hard mode results in more o2 being drained
                        if (MainMenuManager.isHard())
                        {
                            o2Bar.oxygen -= 15f * Time.deltaTime;
                        }
                        else 
                        {
                            o2Bar.oxygen -= 10f * Time.deltaTime;
                        }
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

        // move player based on direction in which the camera is looking
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            // transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, 15f * Time.deltaTime);
        }
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

            // prevents picked up obj from moving
            if (pickingUp != null) {
                // grabable = pickingUp;
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

    // Knocks player in given direction
    public void Knockback(Vector3 direction) {
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
    }

    // Set the vertical movement to the bounce force
    public void Bounce(float force)
    {
        moveDirection.y = force;
    }

    // For swamp waters - slows down movement of the player
    private Vector3 conveyorForce = Vector3.zero;
    public void SetConveyorForce(Vector3 force)
    {
        conveyorForce = force;
    }

    // sets the pickup obj to follow the players movement
    void Pickup(GameObject obj) {
        obj.transform.position = new Vector3(transform.position.x, transform.position.y+3.2f, transform.position.z);
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

    // dash power allows player to dash in the direction that they are currently moving in
    public void dash()
    {
        Debug.Log("dashing");

        // checks cooldown
        if (Time.time >= nextDashTime) {
            if (moveInput != Vector2.zero) {
                // Calculate dash direction based on current movement direction
                dashDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized * dashPower;
            }
            else {
                // when not moving defualt dash will go forward
                dashDirection = (transform.forward * 1 + transform.right * moveInput.x).normalized * dashPower;
            }
            // Update next dash time
            nextDashTime = Time.time + dashCooldown;
        }
    }

    void Update()
    {
        MovePlayer();

        // Update Animator with movement intensity
        float movementIntensity = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude / sprintSpeed; // Normalised movement intensity
        playerAnimator.SetFloat("Speed", movementIntensity);
        playerAnimator.SetBool("isGrounded", controller.isGrounded);
    }


}
