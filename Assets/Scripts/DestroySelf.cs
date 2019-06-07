using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(Destroy());
    }


    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(particleSystem.main.duration + 3);
        Destroy(gameObject);
    }
}
