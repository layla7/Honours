using UnityEngine;

public class Toms : MonoBehaviour
{
    //private ParticleSystem[] particleSystems;

    private GameObject[] toms = new GameObject[3];

    bool playTom = false;
    int tomNum;
    float strength;

    public float defaultParticleScale = 0.8f;
    public float defaultSimulationSpeed = 3f;

    public float defaultFieldX = 1f;
    public float defaultFieldY = 1f;
    public float defaultX = 0.5f;
    public float defaultY = 0.5f;

    public float sensitivity = 1f;
    public float gravSetting = 1f;

    public float defaultStartSpeed = 5f;
    
    void Start()
    {
        //particleSystems = GetComponentsInChildren<ParticleSystem>();

        toms[0] = GameObject.Find("Tom 1 Field");
        toms[1] = GameObject.Find("Tom 2 Field");
        toms[2] = GameObject.Find("Tom 3 Field");
    }

    // Update is called once per frame
    void Update()
    {
        if (playTom == true)
        {
            if (tomNum != -1)
            {
                ParticleSystem[] particles = toms[tomNum].GetComponentsInChildren<ParticleSystem>();

                if (strength > 30)
                {
                    particles[1].Play();
                }
                particles[0].Play();
            }
            playTom = false;
        }
    }

    public void activate(int note, int noteStrength)
    {
        switch(note)
        {
            case 45:
                tomNum = 2;
                break;
            case 47:
                tomNum = 1;
                break;
            case 48:
                tomNum = 0;
                break;
            default:
                tomNum = -1;
                break;
        }

        strength = (float)noteStrength * sensitivity;
        playTom = true;
    }
}
