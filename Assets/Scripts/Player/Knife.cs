using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knife : MonoBehaviour
{

    [SerializeField] private GameObject player;             // The knife holder
    [SerializeField] private float waitTime;
    [SerializeField] private Transform arm;                 // The arm for rotation of knife
    [SerializeField] private Transform hand;                // The hand for position of knife
    [SerializeField] private Vector3 handOffset;
    [SerializeField] private Transform landCheck;           // Where the knife detects the ground
    [SerializeField] private Transform knifeTip;
    [SerializeField] private float landCheckRadius;         // Radius around the land check location
    [SerializeField] private LayerMask whatIsStickable;     // What the knife considers landable

    [Header("Particle Systems")]
    [Space]

    [SerializeField] private ParticleSystem OnReturnParticles;
    [SerializeField] private GameObject OnFlyParticles;



    private Rigidbody2D rb;             // Knife Rigidbody
    private Vector3 targetDir;
    private SoundManager soundManager;  // Manages the Sound s
    private bool thrown = false;        // If the knife has been thrown
    private bool landed = false;        // If the knife has landed
    private bool returning = false;
    private SpriteMask knifeMask;
    private float returnSpeed;
    private Animator animator;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soundManager = GetComponentInChildren<SoundManager>();
        knifeMask = GetComponent<SpriteMask>();
        animator = GetComponent<Animator>();

        knifeMask.enabled = false;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void Update()
    {
        bool wasLanded = landed;

        Collider2D collider = Physics2D.OverlapCircle(landCheck.position, landCheckRadius, whatIsStickable);
        Debug.DrawLine(landCheck.position, landCheck.position + new Vector3(landCheckRadius, 0, 0));

        if (collider != null)
        {
            landed = true;

            if (!wasLanded)
            {
                Stick(collider);
            }
        }
        else
        {
            knifeMask.enabled = false;
            landed = false;
        }

        //if knife is in player's hand, reset it's position and rotation
        if (!thrown)
        {
            transform.rotation = arm.rotation;
            transform.position = hand.position;
        }
        //if the knife is thrown, set it's rotation to the velocity direction
        else if (thrown && !landed)
        {
            transform.rotation = VectorToRotation(rb.velocity);
        }

        if (returning)
        {
            targetDir = (hand.position + handOffset) - transform.position;
            transform.rotation = VectorToRotation(targetDir);

            Vector3 newPos = hand.position + handOffset;
            transform.position -= (transform.position - newPos) * Time.deltaTime * returnSpeed;
        }

        Debug.DrawLine(transform.position, hand.position + handOffset, Color.red);
    }

    //adds a delay to the actual throw to account for the animation
    public IEnumerator KnifeThrow(Vector2 throwVelocity)
    {
        yield return new WaitForSeconds(0.2f);
        rb.simulated = true;
        rb.velocity = throwVelocity;
        thrown = true;
    }

    public IEnumerator KnifeReturn(float speed)
    {
        Instantiate(OnReturnParticles, transform.position, Quaternion.identity);
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Boop");
        

        transform.parent = null;
        returnSpeed = speed;
        returning = true;
        GameObject flyParticles = Instantiate(OnFlyParticles, knifeTip.position, Quaternion.identity) as GameObject;
        flyParticles.transform.parent = knifeTip;
    }

    public void Stick(Collider2D collider)
    {
        if (thrown)
        {
            returning = false;
            rb.simulated = false;
            transform.parent = collider.transform;
            knifeMask.enabled = true;
            soundManager.PlaySound(collider.tag);
        }
    }

    private Quaternion VectorToRotation(Vector2 vector)
    {
        float velDir = (Mathf.Asin(vector.normalized.y) / (2 * Mathf.PI) * 360) - 90;
        if (vector.x < 0)
        {
            velDir = -velDir;
        }

        return Quaternion.Euler(new Vector3(0, 0, velDir));
    }

    public void ResetThrow()
    {
        returning = false;
        thrown = false;
    }

    public bool GetReturning()
    {
        return returning;
    }
    public bool GetThrown()
    {
        return thrown;
    }
}
