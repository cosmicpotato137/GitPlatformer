﻿using System.Collections;
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
    private bool landed = false;        // If the knife has landed
    private bool returning = false;
    private SpriteMask knifeMask;


    //[Header("Events")]
    //[Space]

    //public UnityEvent OnLandEvent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knifeMask = GetComponent<SpriteMask>();
        knifeMask.enabled = false;


        //if no event, make a new one
        //if (OnLandEvent == null)
        //OnLandEvent = new UnityEvent();
    }


    void FixedUpdate()
    {
        bool wasLanded = landed;
        landed = false;

        //find all colliders within a certain radius of landCheck and add those to colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(landCheck.position, landCheckRadius, whatIsStickable);



        //change value of landed

        if (colliders.Length > 0)
        {
            //Debug.Log(colliders);
            landed = true;
            if (!wasLanded)
            {

            }

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
        else if (thrown && !landed)
        {
            float velDir = (Mathf.Asin(rb.velocity.normalized.y) / (2 * Mathf.PI) * 360) - 90;
            if (rb.velocity.x <= 0)
            {
                velDir *= -1;
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, velDir));
        }

        if (landed)
        {
        }
    }

    //adds a delay to the actual throw to account for the animation
    public IEnumerator KnifeThrow(Vector2 throwVelocity)
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = throwVelocity;
        thrown = true;
    }

    public IEnumerator KnifeReturn(Vector3 destination)
    {
        yield return new WaitForSeconds(.1f);
        returning = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") && thrown)
        {
            landed = true;
            rb.simulated = false;
            rb.velocity = Vector2.zero;
            knifeMask.enabled = true;
        }
    }
    public void SayLanded()
    {
        Debug.Log("Landed");
    }
    
}
