using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject knife;          // What is the knife?
    [SerializeField] private Vector2 throwVelocity;     // What is the throw velocity?

    private CharacterController2D playerController;   // Accesses the player controller from the player
    private Knife knifeController;              // Accesses the knife controller from knife
    private bool isThrowing = false;            // If the player is saucein'
    private bool isHitting = false;             // If the player is slappin'
    private bool isThrown = false;              // If the knive is flappin'

    // Start is called before the first frame update
    void Start()
    {
        //find the player controller and knife controller
        playerController = GetComponent<CharacterController2D>();
        knifeController = knife.GetComponent<Knife>();
    }

    // Update is called once per frame
    void Update()
    {
        //determine weather the knife is being thrown
        if (Input.GetButton("Fire1") && !isThrown)
        {
            isThrowing = true;
            isThrown = true;
            playerController.ThrowKnife(throwVelocity);
        }
        else
        {
            isThrowing = false;
        }
    }

    void FixedUpdate()
    {
        //throw the knife
        //playerController.ThrowKnife(throwVelocity);
    }
}
