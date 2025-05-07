using UnityEngine;
using System.Collections; 

public class BassDrum : MonoBehaviour
{

    private ParticleSystem particles;
    private ParticleSystemForceField field;
    private bool enableParticles;

    public float defaultStartRange = 0.5f;      
    public float defaultFieldX = 2f;            
    public float defaultFieldY = 2f;            
    public float defaultX = 1f;                 
    public float defaultY = 1f;
    public float modifier = 0f;

    public float sensitivity = 1;
    public float gravSetting = 1;

    public float defaultGravity = 0.5f;
    public float defaultLifetime = 1f;          
    public float defaultStartSpeed = 5f;        
    public float defaultSimulationSpeed = 3f;
    public float defaultParticleScale = 0.8f;

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

        currentLifetime = defaultLifetime;
        currentStartRange = defaultStartRange;
        currentParticleScale = defaultX;
        currentFieldScale = defaultFieldX;
        currentStartSpeed = defaultStartSpeed;


        ParticleSystem.MainModule main = particles.main;
        main.startLifetime = currentLifetime;
        main.startSpeed = currentStartSpeed; 

        var shape = particles.shape;
        shape.radius = currentStartRange;

        if (GameObject.Find("Spinner") != null)
        {
            Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
            hitResetThreshold = (60f / spinner.bpm);
        }
        

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
        float multiplier = Mathf.Clamp01((float)strength / 130f * sensitivity);

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetAfterDelay(hitResetThreshold));

        if (Time.time - lastHitTime > hitResetThreshold)
        {
            ResetToDefaults();
        }
        
        lastHitTime = Time.time;

        currentLifetime = Mathf.Min(currentLifetime + (0.8f * multiplier), maxLifetime);
        currentStartRange = Mathf.Min(currentStartRange + (0.1f * multiplier), maxStartRange);
        currentParticleScale = Mathf.Min(currentParticleScale + (defaultX * 0.75f * multiplier), maxParticleScale);
        currentFieldScale = Mathf.Min(currentFieldScale + (defaultFieldX * 0.75f * multiplier), maxFieldScale);
        currentStartSpeed = Mathf.Min(currentStartSpeed + (0.2f * multiplier), maxStartSpeed);

        UpdateParticleProperties();

        if (field != null)
        {
            field.gravity = defaultGravity * gravSetting + (-0.3f * multiplier);
        }
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetToDefaults();
    }

    private void UpdateParticleProperties()
    {
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

    private void ResetToDefaults()
    {
        currentLifetime = defaultLifetime;
        currentStartRange = defaultStartRange;
        currentParticleScale = defaultX + (defaultX * modifier);
        currentFieldScale = defaultFieldX + (defaultFieldX * modifier);
        currentStartSpeed = defaultStartSpeed;
        UpdateParticleProperties();

        if (field != null)
        {
            field.gravity = defaultGravity * gravSetting;
        }
    }
}
