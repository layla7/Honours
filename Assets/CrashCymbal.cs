using UnityEngine;

public class CrashCymbal : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private ParticleSystemForceField field;
    private bool enableParticles;

    public float defaultGravity = 2.5f;
    public float defaultDuration = 0.2f;
    public float defaultROT = 250f;
    public float startLifetime = 0.2f;
    public float defaultSimulationSpeed = 2f;
    public float defaultX = 0.5f;
    public float defaultY = 0.75f;

    public float sensitivity = 1;
    public float gravSetting = 1;

    public float defaultFieldX = 2f;
    public float defaultFieldY = 2f;

    public float defaultParticleScale = 0.8f;

    float gravityMod;
    float durationMod;
    float ROTMod;
    float startLifetimeMod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        field = GetComponent<ParticleSystemForceField>();

        foreach (ParticleSystem particleElement in particleSystems)
        {
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
            foreach (ParticleSystem particleElement in particleSystems)
            {
                var em = particleElement.emission;
                em.rateOverTime = defaultROT + ROTMod;

                var main = particleElement.main;
                main.duration = defaultDuration + durationMod;
                main.startLifetime = startLifetime + startLifetimeMod;

                particleElement.Play();
            }

            field.gravity = (defaultGravity * gravSetting) + gravityMod;

            enableParticles = false;
        }
    }

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
