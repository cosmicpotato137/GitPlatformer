using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -30)
        {
            transform.position = new Vector3(-5, 200, 0);
        }
    }
}
