using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knife : MonoBehaviour
{

    [SerializeField] private GameObject player;             // The knife holder
    [SerializeField] private Transform arm;                 // The arm for rotation of knife
    [SerializeField] private Transform hand;                // The hand for position of knife
    [SerializeField] private Transform landCheck;           // Where the knife detects the ground
    [SerializeField] private float landCheckRadius;         // Radius around the land check location
    [SerializeField] private LayerMask whatIsStickable;     // What the knife considers landable

    private Rigidbody2D rb;     // Knife Rigidbody
    private bool thrown;        // If the knife has been thrown
    private bool landed;        // If the knife has landed

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //if no event, make a new one
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void FixedUpdate()
    {
        bool wasLanded = landed;

        //find all colliders within a certain radius of landCheck and add those to colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(landCheck.position, landCheckRadius, whatIsStickable);

        //change value of landed
        if (colliders.Length > 0)
        {
            landed = true;
            if (!wasLanded)
            OnLandEvent.Invoke();
        }
        else
        {
            landed = false;
        }
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
        else if (thrown)
        {
            float velDir = (Mathf.Asin(rb.velocity.normalized.y) / (2 * Mathf.PI) * 360) - 90;
            Debug.Log(velDir);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, velDir));
        }
        else if (thrown && landed)
        {
            rb.simulated = false;
        }
    }

    //adds a delay to the actual throw to account for the animation
    public IEnumerator KnifeThrow(Vector2 throwVelocity)
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = throwVelocity;
        thrown = true;
    }
    
}
