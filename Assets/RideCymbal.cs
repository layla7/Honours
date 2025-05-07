using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideCymbal : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    bool enableParticles;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (enableParticles)
        {
            foreach (ParticleSystem particleElement in particleSystems)
            {
                
                particleElement.Play();
            }
            enableParticles = false;
        }
    }

    public void activate(int strength)
    {
        //Debug.Log("running");
        enableParticles = true;
    }
}
