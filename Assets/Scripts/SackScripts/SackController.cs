using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SackController : MonoBehaviour
{
    public float stabForce;
    public float hitForce;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Knife"))
        {
            if (other.transform.position.x > transform.position.x)
            {
                rb.AddForce(new Vector2(-stabForce, 0));
            }
            else
            {
                rb.AddForce(new Vector2(stabForce, 0));
            }
        }
    }
}
