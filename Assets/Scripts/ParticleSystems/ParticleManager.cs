using System;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public Particle[] particles;

    public void StartParticles(string name)
    {
        Particle p = Array.Find(particles, particle => particle.name == name);
        if (p == null)
        {
            Debug.LogError("ParticleSystem:" + name + "was not found!");
            return;
        }
        Instantiate(p.particleSystem, p.position);
    }
}
