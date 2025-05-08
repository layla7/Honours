using UnityEngine;

public class CrashCymbal : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private ParticleSystemForceField field;
    private bool enableParticles;

    //defaults
    public float defaultGravity = 2.5f;
    public float defaultDuration = 0.2f;
    public float defaultROT = 250f;
    public float startLifetime = 0.2f;
    public float defaultSimulationSpeed = 2f;
    public float defaultX = 0.5f;
    public float defaultY = 0.75f;
    public float defaultParticleScale = 0.8f;

    //setup settings
    public float sensitivity = 1;
    public float gravSetting = 1;

    //Field defaults
    public float defaultFieldX = 2f;
    public float defaultFieldY = 2f;

    //Mods
    float gravityMod;
    float durationMod;
    float ROTMod;
    float startLifetimeMod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get particles and field
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        field = GetComponent<ParticleSystemForceField>();

        //Loop paticles
        foreach (ParticleSystem particleElement in particleSystems)
        {
            //Set values to defaults
            var em = particleElement.emission;
            em.rateOverTime = defaultROT;

            var main = particleElement.main;
            main.duration = defaultDuration;
            main.startLifetime = startLifetime;
        }

        field.gravity = defaultGravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableParticles)
        {
            //Loop particles
            foreach (ParticleSystem particleElement in particleSystems)
            {
                //Set emmision rate over time by ROTMod
                var em = particleElement.emission;
                em.rateOverTime = defaultROT + ROTMod;

                //Set particle duration and lifetime
                var main = particleElement.main;
                main.duration = defaultDuration + durationMod;
                main.startLifetime = startLifetime + startLifetimeMod;

                //Play particles
                particleElement.Play();
            }

            //sest gravity by mod and grav setting
            field.gravity = (defaultGravity * gravSetting) + gravityMod;

            enableParticles = false;
        }
    }

    //Determine modifiers set effect to play
    public void activate(int strength)
    {
        Debug.Log("running");
        float multiplier = (float)strength / 130f * sensitivity;

        gravityMod = -(1.6f * multiplier);
        durationMod = 0.2f * multiplier;
        ROTMod = 750f * multiplier;
        startLifetimeMod = 0.3f * multiplier;

        enableParticles = true;
    }
}
