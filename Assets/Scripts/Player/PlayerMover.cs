using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float speed = 40.0f;

    private float horizontalMove = 0f;              // Controlls the movement spped of the character
    private bool jump = false;                      // If the character is jumping
    private bool crouch = false;                    // If the character is crouching
    private Vector3 moveDirection = Vector3.zero;   // The direction in which the character is moving
    CharacterController2D playerController;         // The player controller

    void Start()
    {
        playerController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        //controlls the speed of the character
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        //if space is pressed, jump and incriment jumps
        if (Input.GetButtonDown("Jump") && !jump)
        {
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }
        //if space is released, stop jumping

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
            crouch = false;
    }

    void FixedUpdate()
    {
        // Move the character
        playerController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
    }
}