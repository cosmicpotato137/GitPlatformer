using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Vector2 throwVelocity;     // What is the throw velocity?
    [SerializeField] private float returnSpeed;
    [SerializeField] private float catchDistance;       // What distance to catch the knife at

    private CharacterController2D playerController;     // Accesses the player controller from the player
    private Knife knifeController;                      // Accesses the knife controller from knife
    private bool isThrowing = false;                    // If the player is scrappin'
    private bool isReturning = false;
    private bool isHitting = false;                     // If the player is slappin'
    private bool isThrown = false;                      // If the knive is flappin'

    // Start is called before the first frame update
    void Start()
    {
        //find the player controller and knife controller
        playerController = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //determine weather the knife is being thrown
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Attack");
            isThrowing = true;
        }
        else
            isThrowing = false;

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Return");
            isReturning = true;
        }
        else
            isReturning = false;
    }

    void FixedUpdate()
    {
        //throw the knife
        playerController.Attack(throwVelocity, returnSpeed, catchDistance, isThrowing, isReturning);
    }
}
