using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class PlayerMover : MonoBehaviour
{
    CharacterController2D playerController;

    public float speed = 40.0f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        playerController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
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