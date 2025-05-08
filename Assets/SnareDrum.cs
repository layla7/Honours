using UnityEngine;

public class SnareDrum : MonoBehaviour
{
    private bool enableParticles;
    //Set in editor to snare prefab
    public GameObject snare;

    //Default snare particle values
    public float defaultSimulationSpeed = 1.15f;
    public float defaultX = 0.1f;
    public float defaultY = 0.1f;
    public float defaultParticleScale = 0.8f;
    public float defaultStartSpeed = 20f;
    public float defaultLifetime = 0.3f;

    //Particle system field defaults
    public float defaultFieldX = 2f;
    public float defaultFieldY = 4.125f;
    public float gravSetting = 1f;

    //Modifiers
    public float startSpeedMod;
    public float lifetimeMod;

    //Sensitivity to input velocity from drum-kit
    public float sensitivity = 1f;

    float strength;
    Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Start default start position
        pos = new Vector3(5f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enableParticles)
        {
            //If strength over eighty change position
            if (strength > 80) {
                float x = 5f;
                //If snare on right
                if (pos.x > 4f && pos.x < 8f)
                {
                    //Place snare randomly on other side
                    x = Random.Range(-4f, -8f);
                }//If snare on left 
                else if (pos.x < -4f && pos.x > -8f)
                {
                    //Place snare randomly on other side
                    x = Random.Range(4f, 8f);
                }
                //Determine random height for snare
                float y = Random.Range(-2.5f, 2.5f);

                //Create new position vector
                pos = new Vector3(x, y, 1f);
            }


            //Loop through paticles
            ParticleSystem[] snareParticles = snare.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in snareParticles)
            {
                //Alter start speed and lifetime based on modifiers
                var main = ps.main;
                main.startSpeed = defaultStartSpeed + startSpeedMod;
                main.startLifetime = defaultLifetime + lifetimeMod;
            }

            //Place prefab at new position
            Instantiate(snare, pos, Quaternion.Euler(0, 0, 0));
            enableParticles = false;
        }
    }

    public void activate(int hitStrength)
    {
        //factor in sensitivity from setup and hit strength fraction.
        float multiplier = (float)hitStrength / 130f * sensitivity;

        //Alter start speed and lifetime with how much will be added on.
        startSpeedMod = 5 * multiplier;
        lifetimeMod = 0.2f * multiplier;

        //Multiply strength by sensitivity setting changed in setup.
        strength = (float)hitStrength * sensitivity;
        enableParticles = true;
    }
}
