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
    [SerializeField] private float landCheckRadius;         // Radius around the land check location
    [SerializeField] private Transform returnCheck;
    [SerializeField] private float returnCheckDistance;
    [SerializeField] private LayerMask whatIsStickable;     // What the knife considers landable

    [Header("Particle Systems")]
    [Space]

    [SerializeField] private ParticleSystem OnReturnParticles;
    [SerializeField] private ParticleSystem OnFlyParticles;
    [SerializeField] private ParticleSystem SandParticles;


    private Rigidbody2D rb;             // Knife Rigidbody
    private Vector3 targetDir;
    private SoundManager soundManager;  // Manages the Sound s
    private bool thrown = false;        // If the knife has been thrown
    private bool landed = false;        // If the knife has landed
    private bool returning = false;
    private SpriteMask knifeMask;
    private float returnSpeed;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;

        soundManager = GetComponentInChildren<SoundManager>();
        knifeMask = GetComponent<SpriteMask>();
        knifeMask.enabled = false;
        animator = GetComponent<Animator>();
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
            
            

            //Vector3 newPos = hand.position + handOffset;
            //transform.position += targetDir * Time.deltaTime * returnSpeed;
        }

        Debug.DrawLine(transform.position, hand.position + handOffset, Color.red);
    }

    //adds a delay to the actual throw to account for the animation
    public IEnumerator KnifeThrow(Vector2 throwVelocity)
    {
        yield return new WaitForSeconds(0.2f);
        OnFlyParticles.Play();
        soundManager.PlaySound("KnifeFly");
        rb.simulated = true;
        rb.velocity = throwVelocity;
        thrown = true;
    }

    public IEnumerator KnifeReturn(float speed)
    {
        RaycastHit2D hit = Physics2D.Raycast(returnCheck.position, hand.position - returnCheck.position, returnCheckDistance, whatIsStickable);
        if (!hit)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;

            targetDir = ((hand.position + handOffset) - transform.position).normalized;

            Instantiate(OnReturnParticles, transform.position, Quaternion.identity);
            OnFlyParticles.Stop();
            soundManager.PauseSound("KnifeFly");

            yield return new WaitForSeconds(waitTime);
            OnFlyParticles.Play();
            soundManager.PlaySound("KnifeFly");

            transform.parent = null;
            returning = true;

            transform.rotation = VectorToRotation(targetDir);
            rb.simulated = true;
            rb.velocity = (Vector2)targetDir * speed;
        }
    }

    public void Stick(Collider2D collider)
    {
        if (thrown)
        {
            OnFlyParticles.Stop();
            soundManager.PauseSound("KnifeFly");
            returning = false;
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            transform.parent = collider.transform;
            knifeMask.enabled = true;
            soundManager.PlaySound(collider.tag);

            if (collider.CompareTag("Sack"))
                Instantiate(SandParticles, landCheck);
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
        rb.simulated = false;
        OnFlyParticles.Stop();
        soundManager.PauseSound("KnifeFly");
    }

    public bool GetReturning()
    {
        return returning;
    }
    public bool GetThrown()
    {
        return thrown;
    }
    public bool GetLanded()
    {
        return landed;
    }
}
