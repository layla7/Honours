using UnityEngine;
using System.Collections; 

public class BassDrum : MonoBehaviour
{

    private ParticleSystem particles;
    private ParticleSystemForceField field;
    private bool enableParticles;

    //Bass defaults
    public float defaultStartRange = 0.5f;      
    public float defaultX = 1f;                 
    public float defaultY = 1f;
    public float modifier = 0f;
    public float defaultLifetime = 1f;
    public float defaultStartSpeed = 5f;
    public float defaultSimulationSpeed = 3f;
    public float defaultParticleScale = 0.8f;

    //setup defaults
    public float sensitivity = 1;
    public float gravSetting = 1;

    //field defaults
    public float defaultGravity = 0.5f;
    public float defaultFieldX = 2f;
    public float defaultFieldY = 2f;


    private float maxLifetime => defaultLifetime + 0.8f;
    private float maxStartRange => defaultStartRange + 0.1f;
    private float maxParticleScale => defaultX * 1.75f;
    private float maxFieldScale => defaultFieldX * 1.75f;
    private float maxStartSpeed => defaultStartSpeed + 0.2f;

    private float currentLifetime;
    private float currentStartRange;
    private float currentParticleScale;
    private float currentFieldScale;
    private float currentStartSpeed;

    private float lastHitTime = 0f;

    float hitResetThreshold;

    float strength;

    private Coroutine resetCoroutine;

    void Start()
    {

        particles = GetComponentInChildren<ParticleSystem>();
        field = GetComponent<ParticleSystemForceField>();

        //Set current values to defults
        currentLifetime = defaultLifetime;
        currentStartRange = defaultStartRange;
        currentParticleScale = defaultX;
        currentFieldScale = defaultFieldX;
        currentStartSpeed = defaultStartSpeed;

        //Update values to current
        ParticleSystem.MainModule main = particles.main;
        main.startLifetime = currentLifetime;
        main.startSpeed = currentStartSpeed; 

        var shape = particles.shape;
        shape.radius = currentStartRange;

        if (GameObject.Find("Spinner") != null)
        {
            //Check spinner tempo for hit threshold
            Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
            hitResetThreshold = (60f / spinner.bpm);
        }
        

        //Update field settings to defaults
        particles.transform.localScale = new Vector3(currentParticleScale, currentParticleScale, currentParticleScale);
        if (field != null)
        {
            field.transform.localScale = new Vector3(currentFieldScale, currentFieldScale, currentFieldScale);
            field.gravity = defaultGravity; 
        }
    }

    void FixedUpdate()
    {
        if (enableParticles)
        {
            setValues();
            particles.Play();
            enableParticles = false;
        }
    }

    public void activate(int hitStrength)
    {
        Debug.Log("BassDrum activated");
        strength = hitStrength;
        enableParticles = true;
    }

    private void setValues()
    {
        //Calculate multiplier
        float multiplier = (float)strength / 130f * sensitivity;

        if (resetCoroutine != null)
        {
            //Stop
            StopCoroutine(resetCoroutine);
        }
        //Reset bass drum to defaults after beat time has passed
        resetCoroutine = StartCoroutine(resetAfterDelay(hitResetThreshold));

        //Reset bass drum if time since last beat too long
        if (Time.time - lastHitTime > hitResetThreshold)
        {
            ResetToDefaults();
        }
        
        lastHitTime = Time.time;

        //set current values based on multipliers
        currentLifetime = Mathf.Min(currentLifetime + (0.8f * multiplier), maxLifetime);
        currentStartRange = Mathf.Min(currentStartRange + (0.1f * multiplier), maxStartRange);
        currentParticleScale = Mathf.Min(currentParticleScale + (defaultX * 0.75f * multiplier), maxParticleScale);
        currentFieldScale = Mathf.Min(currentFieldScale + (defaultFieldX * 0.75f * multiplier), maxFieldScale);
        currentStartSpeed = Mathf.Min(currentStartSpeed + (0.2f * multiplier), maxStartSpeed);
        //Update particles
        updateParticleProperties();
        //Update field
        if (field != null)
        {
            field.gravity = defaultGravity * gravSetting + (-0.3f * multiplier);
        }
    }

    private IEnumerator resetAfterDelay(float delay)
    {
        //Wait and reset particles
        yield return new WaitForSeconds(delay);
        ResetToDefaults();
    }

    private void updateParticleProperties()
    {
        //Update particles to current values
        var main = particles.main;
        main.startSpeed = currentStartSpeed;

        var shape = particles.shape;
        shape.radius = currentStartRange;

        particles.transform.localScale = new Vector3(currentParticleScale, currentParticleScale, currentParticleScale);

        if (field != null)
        {
            field.transform.localScale = new Vector3(currentFieldScale, currentFieldScale, currentFieldScale);
        }
    }
    //Reset all current values to defaults
    private void ResetToDefaults()
    {
        currentLifetime = defaultLifetime;
        currentStartRange = defaultStartRange;
        currentParticleScale = defaultX + (defaultX * modifier);
        currentFieldScale = defaultFieldX + (defaultFieldX * modifier);
        currentStartSpeed = defaultStartSpeed;
        updateParticleProperties();

        if (field != null)
        {
            field.gravity = defaultGravity * gravSetting;
        }
    }
}
