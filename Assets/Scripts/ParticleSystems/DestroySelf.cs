using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float killTime;

    ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(Destroy());
    }


    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(killTime);
        Destroy(gameObject);
    }
}
