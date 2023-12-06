using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public ParticleSystem ps;
    public int poolSize = 10;
    private List<ParticleSystem> particles;
    void Start()
    {
        GarbageMovement.op = this; 
        particles = new List<ParticleSystem>();
        for(int i =0; i<poolSize; i++)
        {
            ParticleSystem particle = Instantiate(ps, transform);
            particle.Stop();
            particles.Add(particle);
        }
    }

    public ParticleSystem GetParticle()
    {
        for(int i = 0; i<particles.Count; i++)
        {
            if (!particles[i].isPlaying)
            {
                return particles[i];
            }
        }
        ParticleSystem particle = Instantiate(ps, transform);
        particles.Add(particle);
        return particle;
    }
}
