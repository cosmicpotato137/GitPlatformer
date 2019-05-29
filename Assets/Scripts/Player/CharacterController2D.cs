using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpVel = .13f;                            // Upward jumping force
    [SerializeField] private float m_JumpHeight = 10;                           // How high to jump **note this is an inverse value. the higher the m_JumpHeight
    [SerializeField] private float m_AirStop = 1;                               // How quickly the player stops going up after space is released
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    private Animator animator;
               
    const float k_GroundRadius = .2f;               // Radius of the overlap circle to determine if the player is grounded
    const float k_CeilingRadius = .2f;              // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;              // Player rigid body
    private bool m_FacingRight = true;              // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;      // Velocity of player
    private bool m_Attacking;                       // If the player is in an attack animation
    private bool m_Grounded;                        // Whether or not the player is grounded.
    private float jumpTime = 0;                     // How long the player has been in the air
    

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundRadius, m_WhatIsGround);

        if (colliders.Length > 0)
        {
            m_Grounded = true;
            if (!wasGrounded)
                Debug.Log("Landed");
                OnLandEvent.Invoke();
        }
        else
        {
            m_Grounded = false;
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch && m_Grounded)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }


            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            if ((m_Attacking || Input.GetButton("Fire1")) && m_Grounded)
            {
                targetVelocity = Vector2.zero;   
            }

            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // Lots of conditions for the player to go up
        if (jump)
        {
            jumpTime += Time.fixedDeltaTime;

            float jumpVel = LinearVelDec(jumpTime, m_JumpHeight);

            //controlls the upward jumping force
            if (jumpVel > 0 && m_Rigidbody2D.velocity.y > -0.1)
            {
                m_Rigidbody2D.AddForce(new Vector2(0, jumpVel * m_JumpVel));
            }
            else
            {
                m_Rigidbody2D.AddForce(Vector2.zero);
            }
        }

        //snappier falls
        else if (!jump && m_Rigidbody2D.velocity.y > .01)
        {
            m_Rigidbody2D.AddForce(new Vector2(0, -m_AirStop));
        }

        //reset jumpTime
        if(m_Grounded && !jump)
        {
            jumpTime = 0;
        }

        //Controlls player animations
        animator.SetFloat("velocityY", m_Rigidbody2D.velocity.y);
        animator.SetBool("isGrounded", m_Grounded);

        if (Mathf.Abs(m_Rigidbody2D.velocity.x) > .1)
        {
            animator.SetBool("isRunning", true);
        }
        else if (Mathf.Abs(m_Rigidbody2D.velocity.x) < .1)
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void Attack(bool isThrowing, bool isHitting)
    {
        m_Attacking = animator.GetCurrentAnimatorStateInfo(0).IsName("KnifeThrow");
        Debug.Log(m_Attacking);

        if (isThrowing && !m_Attacking)
        {
            animator.SetTrigger("attack");
        }
        else
        {
            animator.ResetTrigger("attack");
        }
    }

    //
    //functins for controlling air motion
    //

    //Linear foce decrease
    float LinearVelDec(float time, float slope)
    {
        return -slope * time + 1;
    }
    //sigmoid force decrease
    float SigmoidVelDec(float time)
    {
        return -1 / (1 + Mathf.Pow(20000, -time + .5f)) + 1;
    }

    // Switch the way the player is labelled as facing.
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
