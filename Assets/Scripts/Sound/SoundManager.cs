using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.dimension;

            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop; 
        }
    } 
    
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound:" + name + "was not found!");
            return;
        }
        s.source.Play();
    }

    public void PauseSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound:" + name + "was not found!");
            return;
        }
        s.source.Pause();
    }
}
