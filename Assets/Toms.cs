using UnityEngine;

public class Toms : MonoBehaviour
{
    //Create an array to hold the toms gameobjects
    private GameObject[] toms = new GameObject[3];

    bool playTom = false;
    int tomNum;
    float strength;

    //Default settings for the tom effects
    public float defaultParticleScale = 0.8f;
    public float defaultSimulationSpeed = 3f;

    public float defaultFieldX = 1f;
    public float defaultFieldY = 1f;
    public float defaultX = 0.5f;
    public float defaultY = 0.5f;
    public float gravSetting = 1f;
    public float defaultStartSpeed = 5f;

    //Sensitivity to input velocity from drum-kit
    public float sensitivity = 1f;

    void Start()
    {
        //Set toms
        toms[0] = GameObject.Find("Tom 1 Field");
        toms[1] = GameObject.Find("Tom 2 Field");
        toms[2] = GameObject.Find("Tom 3 Field");
    }

    // Update is called once per frame
    void Update()
    {
        //Play the tom effect
        if (playTom == true)
        {
            //Make sure toms aren't incorrectly triggered
            if (tomNum != -1)
            {
                //Get tom particles
                ParticleSystem[] particles = toms[tomNum].GetComponentsInChildren<ParticleSystem>();

                //If the toms have been hit hard, if so play outer particles
                if (strength > 60)
                {
                    particles[1].Play();
                }
                //Play main tom particles
                particles[0].Play();
            }
            playTom = false;
        }
    }

    public void activate(int note, int noteStrength)
    {
        //Check which tom has been activated 
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

        //Multiply the input strength by the sensitivity setting
        strength = (float)noteStrength * sensitivity;
        playTom = true;
    }
}
