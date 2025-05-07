using UnityEngine;

public class SnareDrum : MonoBehaviour
{
    private bool enableParticles;
    public GameObject snare;
    public float defaultSimulationSpeed = 1.15f;
    //Vector3(2,4.12593746,2)
    public float defaultFieldX = 2f;
    public float defaultFieldY = 4.125f;

    public float defaultStartSpeed = 20f;
    public float defaultLifetime = 0.3f;

    public float startSpeedMod;
    public float lifetimeMod;

    public float defaultX = 0.1f;
    public float defaultY = 0.1f;

    public float defaultParticleScale = 0.8f;

    public float sensitivity = 1f;
    public float gravSetting = 1f;

    float strength;
    Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = new Vector3(5f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enableParticles)
        {
            if (strength > 80) {
                float x = 5f;
                if (pos.x > 4f && pos.x < 8f)
                {
                    x = Random.Range(-4f, -8f);
                } else if (pos.x < -4f && pos.x > -8f)
                {
                    x = Random.Range(4f, 8f);
                }
                float y = Random.Range(-2.5f, 2.5f);

                pos = new Vector3(x, y, 1f);
            }

            ParticleSystem[] snareParticles = snare.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem ps in snareParticles)
            {
                var main = ps.main;
                main.startSpeed = defaultStartSpeed + startSpeedMod;
                main.startLifetime = defaultLifetime + lifetimeMod;
            }


            Instantiate(snare, pos, Quaternion.Euler(0, 0, 0));

            enableParticles = false;
        }
    }

    public void activate(int hitStrength)
    {
        float multiplier = (float)hitStrength / 130f * sensitivity;

        startSpeedMod = 5 * multiplier;
        lifetimeMod = 0.2f * multiplier;

        //Debug.Log(Time.time);
        strength = (float)hitStrength * sensitivity;
        enableParticles = true;
    }
}
