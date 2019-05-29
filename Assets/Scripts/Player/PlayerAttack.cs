using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //public GameObject knife;

    private CharacterController2D controller;
    private bool isThrowing = false;
    private bool isHitting = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Attack!");
            isThrowing = true;
        }
        else
        {
            isThrowing = false;
        }
    }

    void FixedUpdate()
    {
        controller.Attack(isThrowing, isHitting);
    }
}
