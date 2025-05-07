using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Melanchall.DryWetMidi.Interaction;
using System;


public class Spinner : MonoBehaviour
{
    public GameObject hihatSpike;
    public GameObject ride;
    public AudioSource metronomeSound;

    public float rideEmission = 0f;
    public float rideStartLifetime = 0.2f;
    public float rideSpiral = 1f;
    public float rideStartSpeed = 0;
    public float rideInnerDefaultScale = 0.1f;
    public float rideOuterDefaultScale = 0.3f;
    public float rideFieldDefaultScale = 1;
    public float rideInnerPartScale = 3;
    public float rideOuterPartMin = 0.4f;
    public float rideOuterPartMax = 0.8f;

    //public float rideStartSize

    public float rideSimSpeed = 5f;
    public float particlesSimSpeed = 1f;

    public float hatsSimSpeed = 2f;

    public float hatsYDefault = 0.15f;
    public float hatsXDefault = 0.6f;
    public float hatsGravDefault = 2f;

    public float hatsDefaultXScale = 0.6f;
    public float hatsDefaultYScale = 0.335f;
    public float hatsPartScale = 0.8f;

    public float hatsDefaultFieldScale = 1;

    //public float hats;

    ParticleSystem[] rideParticles;

    private float radius = 4f;
    private int opening;

    public float bpm = 120f;
    float loopTime;
    float beatTime;
    
    float speed;
    bool hats;
    bool closed;

    float rideEmissionMod;
    float rideStartLifetimeMod;
    float rideStartSpeedMod;
    float rideSpiralMod;

    float hatsGravMod;
    float hatsClosedMod;
    float hatsOpenedMod;

    public float rideSensitivity = 1f;
    public float hatsSensitivity = 1f;

    private float angle = 180f;
    //float hihatSpike = 0.5f;
    List<GameObject> hits;
    bool newSpike = false;
    float startTime;
    private TrailRenderer trail;
    private AnimationCurve curve;

    public Vector3 center;

    [Tooltip("Trail thickness multiplier at the thinnest beat")]
    public float trailMinWidthMultiplier = 2f;
    [Tooltip("Trail thickness multiplier at the thickest beat")]
    public float trailMaxWidthMultiplier = 12f;

    public float trailWidthMod = 1;

    public float pulseDuration = 0.4f;

    public bool tempoVis = true;
    public bool tempo = true;
    public bool click = true;

    void Start()
    {
        hits = new List<GameObject>();
        trail = GetComponent<TrailRenderer>();
        center = transform.position;

        loopTime = 4 * (60f / bpm);
        beatTime = Time.time;

        trail.widthMultiplier = trailMinWidthMultiplier;

        trail.time = loopTime * 0.6f;
        speed = (2 * Mathf.PI) / loopTime;
        curve = new AnimationCurve(
            new Keyframe(0.0f, 0.1f),
            new Keyframe(1.0f, 0.1f)
        );
        trail.Clear();
        rideParticles = ride.GetComponentsInChildren<ParticleSystem>();
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        if (tempo)
        {
            float now = Time.time;
            float beatInterval = 60f / bpm;

            if (now - beatTime >= beatInterval)
            {
                if (click)
                {
                    metronomeSound.Play();
                }
                beatTime = now;
            }

            if (tempoVis)
            { 
                float sinceBeat = now - beatTime;
                if (sinceBeat <= pulseDuration)
                {
                    float t = sinceBeat / pulseDuration;
                    
                    trail.widthMultiplier = Mathf.Lerp(trailMaxWidthMultiplier * trailWidthMod, trailMinWidthMultiplier * trailWidthMod, t);
                }
                else
                {
                    trail.widthMultiplier = trailMinWidthMultiplier * trailWidthMod;
                }
            }
        }

        if (newSpike)
        {  
            Vector3 pos = new Vector3(center.x + Mathf.Cos(-angle) * radius,
                                      center.y + Mathf.Sin(-angle) * radius,
                                      0);

            float radialAngle = Mathf.Atan2(pos.y - center.y, pos.x - center.x) * Mathf.Rad2Deg;

            float spikeRotation = radialAngle + 90f;

            if (hats)
            {
                //Transform openingParticles = hihatSpike.transform.Find("Opening");

                //openingParticles.localScale = new Vector3(0.2f, 0.375f, 0.25f);

                Transform openingParticles = hihatSpike.transform.Find("Opening");
                ParticleSystem particles = openingParticles.GetComponent<ParticleSystem>();

                var main = particles.main;
                main.playOnAwake = !closed;

                if (closed)
                {
                    Transform particlesSpike = hihatSpike.transform.Find("Hit");
                    particlesSpike.localScale = new Vector3(openingParticles.localScale.x, hatsYDefault + hatsClosedMod, openingParticles.localScale.z);
                }
                else
                {
                    openingParticles.localScale = new Vector3(0.6f - ((0.5f * (float)opening) / 127f), 0.335f, openingParticles.localScale.z);
                    Transform particlesSpike = hihatSpike.transform.Find("Hit");
                    particlesSpike.localScale = new Vector3(openingParticles.localScale.x, hatsYDefault + 0.185f, openingParticles.localScale.z);

                    ParticleSystemForceField ff = hihatSpike.GetComponent<ParticleSystemForceField>();

                    ff.gravity = hatsGravDefault + hatsGravMod;
                }



                Instantiate(hihatSpike, pos, Quaternion.Euler(0, 0, spikeRotation));

                hits.Add(hihatSpike);
            }
            else
            {

                var main = rideParticles[1].main;
                main.startSpeed = rideStartSpeed + rideStartSpeedMod;
                main.startLifetime = rideStartLifetime + rideStartLifetimeMod;

                var spiralMain = rideParticles[0].main;
                spiralMain.startLifetime = rideSpiral + rideSpiralMod;

                var em = rideParticles[1].emission;
                em.rateOverTime = rideEmission + rideEmissionMod;

                Instantiate(ride, pos, Quaternion.Euler(0, 0, spikeRotation));

                hits.Add(ride);
            }

            newSpike = false;
        }

        trail.widthCurve = curve;
        angle += speed * Time.deltaTime;
        float x = center.x + Mathf.Cos(-angle) * radius;
        float y = center.y + Mathf.Sin(-angle) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void activate(int strength, int note)
    {
        newSpike = true;
        float multiplier;

        switch (note)
        {
            case 42:
                multiplier = (float)strength / 127 * hatsSensitivity;
                Debug.Log("Closed Hi-Hat");
                hatsClosedMod = 0.28f * multiplier; 
                hats = true;
                closed = true;
                break;
            case 44:
            case 46:
                Debug.Log("Hi-Hat");
                //This shouldn't be a straight multiplier. Towards the top end it's not high enough and at the bottom end it's too high.
                //FIX LATER. MAYBE HAVE SOME CHECK IF THE PEDAL IS REALLY, REALLY OPEN THEN THE SCALE DOUBLES OR SOMETHING?? 
                multiplier = (float)strength / 127 * hatsSensitivity;
                hatsGravMod = -(0.5f * multiplier);
                closed = false;
                hats = true;
                break;
            case 51:
            case 53:
                multiplier = (float)strength / 127 * rideSensitivity;
                Debug.Log("Ride");
                rideEmissionMod = 100f * multiplier;
                rideStartLifetimeMod = 0.2f * multiplier;
                rideStartSpeedMod = 50f * multiplier;
                rideSpiralMod = 1f * multiplier;

                hats = false;
                break;
        }//WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP WASP


    }

    public void setPedal(int pedal)
    {
        opening = pedal;
    }

    public void setTempoVis(bool val)
    {
        tempoVis = val;
    }

    public void setClick(bool val)
    {
        click = val;
    }

    public void setTempo(string tempo)
    {
        try
        {
            bpm = float.Parse(tempo);

            loopTime = 4 * (60 / bpm); 
            trail.time = loopTime * 0.6f;
            speed = (2 * Mathf.PI) / loopTime;
        }catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }
}

