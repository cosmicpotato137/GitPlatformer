using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParallaxLayer1 : MonoBehaviour
{
    public float xScale = 1;
    public float yScale = 1;

    private Vector3 newVel;
    private Rigidbody2D mainCameraRB;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraRB = Camera.main.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = -mainCameraRB.velocity * new Vector2(xScale, yScale);
        Debug.Log(mainCameraRB.velocity.x);
    }
}
