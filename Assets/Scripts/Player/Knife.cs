using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knife : MonoBehaviour
{

    [SerializeField] private GameObject player;             // The knife holder
    [SerializeField] private Transform arm;                 // The arm for rotation of knife
    [SerializeField] private Transform hand;                // The hand for position of knife
    [SerializeField] private Vector3 handOffset;
    [SerializeField] private float catchDistance;
    [SerializeField] private Transform landCheck;           // Where the knife detects the ground
    [SerializeField] private float landCheckRadius;         // Radius around the land check location
    [SerializeField] private LayerMask whatIsStickable;     // What the knife considers landable

    public Vector3 targetDir;

    private Rigidbody2D rb;     // Knife Rigidbody
    private bool thrown = false;        // If the knife has been thrown
    private bool landed = false;        // If the knife has landed
    private bool returning = false;
    private SpriteMask knifeMask;
    private float returnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knifeMask = GetComponent<SpriteMask>();
        knifeMask.enabled = false;
    }

    void Update()
    {
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

        targetDir = (hand.position + handOffset) - transform.position;
        if (returning)
        {
            if (!thrown)
            {
                Debug.Log("error");
            }

            transform.rotation = VectorToRotation(targetDir);

            Vector3 newPos = hand.position;
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * returnSpeed);
        }
        Debug.DrawLine(transform.position, hand.position, Color.red);

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
        yield return new WaitForSeconds(.1f);
        returnSpeed = speed;
        returning = true;
        landed = false;
        knifeMask.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") && thrown)
        {
            Debug.Log("landed");
            landed = true;
            StopAllCoroutines();
            rb.simulated = false;
            rb.velocity = Vector2.zero;
            knifeMask.enabled = true;
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
        rb.simulated = true;
        Debug.Log(thrown);
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
