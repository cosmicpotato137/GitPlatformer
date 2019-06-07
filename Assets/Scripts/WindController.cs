using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindController : MonoBehaviour
{
    SoundManager soundManager;
    public float minTime;
    public float maxTime;

    private float timeSincePlay = 0;
    private bool blowing = false;

    private UnityEvent OnBlowEvent;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
        soundManager.PlaySound("Wind2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSincePlay += Time.deltaTime;
        if(timeSincePlay > minTime && !blowing)
        {
            StartCoroutine(Blow());
        }
    }

    IEnumerator Blow()
    {
        blowing = true;
        soundManager.PlaySound("Wind1");
        float time = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(time);
        timeSincePlay = 0;
        blowing = false;
    }
}
