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

    //Ride defaults
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
    public float rideSimSpeed = 5f;
    public float particlesSimSpeed = 1f;

    //Hats defaults
    public float hatsSimSpeed = 2f;
    public float hatsYDefault = 0.15f;
    public float hatsXDefault = 0.6f;
    public float hatsGravDefault = 2f;
    public float hatsDefaultXScale = 0.6f;
    public float hatsDefaultYScale = 0.335f;
    public float hatsPartScale = 0.8f;
    public float hatsDefaultFieldScale = 1;

    
    ParticleSystem[] rideParticles;

    //Spinner settings
    private float radius = 4f;
    private int opening;
    

    //Tempo settings
    public float bpm = 120f;
    float loopTime;
    float beatTime;
    
   
    bool hats;
    bool closed;
    float speed;

    //Ride modifiers
    float rideEmissionMod;
    float rideStartLifetimeMod;
    float rideStartSpeedMod;
    float rideSpiralMod;

    //Hats modifiers
    float hatsGravMod;
    float hatsClosedMod;

    //Sensitivity determined in setup page
    public float rideSensitivity = 1f;
    public float hatsSensitivity = 1f;

    private float angle = 180f;
    List<GameObject> hits;
    bool newSpike = false;
    float startTime;
    private TrailRenderer trail;
    private AnimationCurve curve;

    public Vector3 center;

    //Thickness of the standard trail
    public float trailMinWidthMultiplier = 2f;
    //Thickness of the trail during a beat
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
        //Set center of spinner to (0,0)
        center = transform.position;

        //Calculate how long a loop should take
        loopTime = 4 * (60f / bpm);
        //Keep track of beats
        beatTime = Time.time;

        trail.widthMultiplier = trailMinWidthMultiplier;

        //set speed based on trail time
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
        //If tempo settings are on
        if (tempo)
        {
            //Take current time
            float now = Time.time;
            float beatInterval = 60f / bpm;

            //If time passed is more than interval between beats
            if (now - beatTime >= beatInterval)
            {
                //If click track is on plasy the sound
                if (click)
                {
                    metronomeSound.Play();
                }
                beatTime = now;
            }

            //If visualising tempo
            if (tempoVis)
            { 
                //If time since last beat is less than our pulse duration update width multiplier
                float sinceBeat = now - beatTime;
                if (sinceBeat <= pulseDuration)
                {
                    float t = sinceBeat / pulseDuration;
                    
                    trail.widthMultiplier = Mathf.Lerp(trailMaxWidthMultiplier * trailWidthMod, trailMinWidthMultiplier * trailWidthMod, t);
                }
                else
                {
                    //Update width mulultiplier
                    trail.widthMultiplier = trailMinWidthMultiplier * trailWidthMod;
                }
            }
        }

        //If a new hi-hat spike is in
        if (newSpike)
        {  
            //Calculate the position of the now spike on the spinner
            Vector3 pos = new Vector3(center.x + Mathf.Cos(-angle) * radius,
                                      center.y + Mathf.Sin(-angle) * radius,
                                      0);


            //Calculate angle of effect to the spinner
            float radialAngle = Mathf.Atan2(pos.y - center.y, pos.x - center.x) * Mathf.Rad2Deg;

            //Set effect to be at 90 degree angle
            float spikeRotation = radialAngle + 90f;

            //If hi-hat and not ride
            if (hats)
            {
                //Find particles of opening
                Transform openingParticles = hihatSpike.transform.Find("Opening");
                ParticleSystem particles = openingParticles.GetComponent<ParticleSystem>();

                //opening particles play based on whether closed
                var main = particles.main;
                main.playOnAwake = !closed;

                if (closed)
                {
                    //scale paticles by closed mod
                    Transform particlesSpike = hihatSpike.transform.Find("Hit");
                    particlesSpike.localScale = new Vector3(openingParticles.localScale.x, hatsYDefault + hatsClosedMod, openingParticles.localScale.z);
                }
                else
                {
                    //Scale particles by opening scale 
                    openingParticles.localScale = new Vector3(0.6f - ((0.5f * (float)opening) / 127f), 0.335f, openingParticles.localScale.z);
                    Transform particlesSpike = hihatSpike.transform.Find("Hit");
                    particlesSpike.localScale = new Vector3(openingParticles.localScale.x, hatsYDefault + 0.185f, openingParticles.localScale.z);

                    ParticleSystemForceField ff = hihatSpike.GetComponent<ParticleSystemForceField>();
                    //Lower gravity by mod
                    ff.gravity = hatsGravDefault + hatsGravMod;
                }


                //Place spike
                Instantiate(hihatSpike, pos, Quaternion.Euler(0, 0, spikeRotation));
                //Add to array keeping track
                hits.Add(hihatSpike);
            }
            else // Ride
            {
                //Update outer particles start speed and lifetime by mod
                var main = rideParticles[1].main;
                main.startSpeed = rideStartSpeed + rideStartSpeedMod;
                main.startLifetime = rideStartLifetime + rideStartLifetimeMod;

                //Update spiral lifetime and emission rate by mod
                var spiralMain = rideParticles[0].main;
                spiralMain.startLifetime = rideSpiral + rideSpiralMod;

                var em = rideParticles[1].emission;
                em.rateOverTime = rideEmission + rideEmissionMod;

                //place ride on spinner
                Instantiate(ride, pos, Quaternion.Euler(0, 0, spikeRotation));

                //Add to array keeping track
                hits.Add(ride);
            }

            newSpike = false;
        }

        //Keep spinner spinning
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

        //Check what's being played and update strengths and modifiers accordingly
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
        }


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
            //Convery tempo to float. calculate tempo stuff
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

