using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    //Get all the colour pickers
    public FlexibleColorPicker colourPicker1;
    public FlexibleColorPicker colourPicker2;
    public FlexibleColorPicker spiralColour1; 
    public FlexibleColorPicker spiralColour2; 
    public FlexibleColorPicker outerColour;
    public FlexibleColorPicker spinnerPicker;
    public FlexibleColorPicker backgroundPicker;
    public FlexibleColorPicker hatColour;
    public FlexibleColorPicker openingColour;

    //Crash
    public ParticleSystem[] crashParticleSystems;
    public ParticleSystemForceField crashField;
    public Vector3 crashPosition;

    //Ride
    public ParticleSystem[] rideParticles;
    public ParticleSystemForceField rideField;
    public Vector3 ridePosition;

    //Prefabs to edit
    public GameObject ride;
    public GameObject hihatSpike;

    //Hats
    public ParticleSystem[] hiHatParticles;
    public ParticleSystemForceField hiHatField;
    public Vector3 hatPosition;

    //Snare
    public ParticleSystemForceField snareField;
    public ParticleSystem[] snareParticles;
    public Vector3 snarePosition;

    //Bass
    public ParticleSystem bassParticles;
    public ParticleSystemForceField bassField;
    public Vector3 bassPosition;

    //Toms
    public ParticleSystemForceField[] tomFields = new ParticleSystemForceField[3];
    public Vector3[] tomPositions = new Vector3[3];

    //UI elements
    private Canvas setupPage;
    private Canvas editor;
    private Canvas globalEditor;
    private Canvas hatsRideEditor;
    public Canvas labels;
    public GameObject colourPanel;
    public GameObject rideColourPanel;
    public GameObject hatsColourPanel;

    void Start()
    {
        //Get UI elements
        setupPage = GameObject.Find("SetupPage").GetComponent<Canvas>();
        editor = GameObject.Find("Particle Editor").GetComponent<Canvas>();
        globalEditor = GameObject.Find("Global Editor").GetComponent<Canvas>();
        hatsRideEditor = GameObject.Find("Crash Ride Editor").GetComponent<Canvas>();
        labels = GameObject.Find("Labels").GetComponent<Canvas>();
        colourPanel = GameObject.Find("ColourPanel");
        //rideColourPanel = GameObject.Find("RideColourPanel");
        //hatsColourPanel = GameObject.Find("HatsColourPanel");


        //Disable everything but the setup page
        setupPage.enabled = true;
        editor.enabled = false;
        hatsRideEditor.enabled = false;
        globalEditor.enabled = false;
        colourPanel.SetActive(false);
        rideColourPanel.SetActive(false);
        hatsColourPanel.SetActive(false);

        //Get crash elements
        crashParticleSystems = GameObject.Find("Crash Field").GetComponentsInChildren<ParticleSystem>();
        crashField = GameObject.Find("Crash Field").GetComponent<ParticleSystemForceField>();

        //Get ride elements
        rideParticles = GameObject.Find("Ride Field").GetComponentsInChildren<ParticleSystem>();
        rideField = GameObject.Find("Ride Field").GetComponent<ParticleSystemForceField>();

        //Get hihat elements
        hiHatParticles = GameObject.Find("Hi-Hat Field").GetComponentsInChildren<ParticleSystem>();
        hiHatField = GameObject.Find("Hi-Hat Field").GetComponent<ParticleSystemForceField>();

        //Get snare elements
        snareField = GameObject.Find("Snare Drum Field").GetComponent<ParticleSystemForceField>();
        snareParticles = GameObject.Find("Snare Drum Field").GetComponentsInChildren<ParticleSystem>();

        //Get bass elements
        bassParticles = GameObject.Find("Bass Drum Field").GetComponentInChildren<ParticleSystem>();
        bassField = GameObject.Find("Bass Drum Field").GetComponent<ParticleSystemForceField>();

        //Get to elements
        tomFields[0] = GameObject.Find("Tom 1 Field").GetComponent<ParticleSystemForceField>();
        tomFields[1] = GameObject.Find("Tom 2 Field").GetComponent<ParticleSystemForceField>();
        tomFields[2] = GameObject.Find("Tom 3 Field").GetComponent<ParticleSystemForceField>();

        //Get initial positions
        crashPosition = crashField.transform.position;
        ridePosition = rideField.transform.position;
        hatPosition = hiHatField.transform.position;
        snarePosition = snareField.transform.position;
        bassPosition = bassField.transform.position;
        for (int i = 0; i < tomFields.Length; i++)
        {
            if (tomFields[i] != null)
            {
                tomPositions[i] = tomFields[i].transform.position;
            }
        }


    }

    void Awake()
    {
        //Keep object and children from being destroyed between scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        
    }

    /**
     * For moving to the live simulation from the setup page.
     */
    public void goToScene()
    {
        //Get spinner and set attached prefabs to new elements designed
        Spinner spinner = gameObject.GetComponentInChildren<Spinner>();
        spinner.hihatSpike = hiHatField.gameObject;
        spinner.ride = rideField.gameObject;

        //Get and set snare prefab to new design.
        SnareDrum snare = gameObject.GetComponent<SnareDrum>();
        snare.snare = snareField.gameObject;

        SceneManager.LoadScene("CustomVisuals");


        //Disable loops on all particles
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            var main = ps.main;
            main.loop = false;
        }

        //Disable particle click component on all elements
        ParticleClick[] clickables = GetComponentsInChildren<ParticleClick>();
        foreach (ParticleClick clickable in clickables)
        {
            clickable.enabled = false;
        }

        //Enable activation and snare drum components
        gameObject.GetComponent<Activation>().enabled = true;
        gameObject.GetComponent<SnareDrum>().enabled = true;
    }

    public void updateSensitivity(float value)
    {
        //Get field of object being set and make sure not null
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("force field not found");
            return;
        }

        //Run update based on object name
        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    //Update sensitivity value in component
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                    }
                    break; 
                }
            case "Crash Field":
                {
                    //Update sensitivity value in component
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    //Update sensitivity value in component, update for demo snare and global snare activator for later.
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    SnareDrum simActivator = gameObject.GetComponent<SnareDrum>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                        simActivator.sensitivity = value;
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    //Update sensitivity value in component
                    Toms activator = field.GetComponentInParent<Toms>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    //Update sensitivity value in component
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (activator != null)
                    {
                        activator.hatsSensitivity = value;
                    }
                    break;
                }
            case "Ride Field":
                {
                    //Update sensitivity value in component
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (activator != null)
                    {
                        activator.rideSensitivity = value;
                    }
                    break;
                }
        } 
    }

    public void updateGravity(float value)
    {
        //Get field of object being set and make sure not null
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("not found");
            return;
        }

        //Run update based on object name
        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    //Update gravity value in component
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                    }
                    break;
                }
            case "Crash Field":
                {
                    //Update gravity value in component
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    //Update gravity value in component, update for demo snare and global snare activator for later.
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    SnareDrum simActivator = gameObject.GetComponent<SnareDrum>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                        simActivator.gravSetting = value;
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    //Update gravity value in component
                    Toms activator = field.GetComponentInParent<Toms>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    //Update gravity value in component
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (activator != null)
                    {
                        activator.hatsSensitivity = value;
                    }
                    break;
                }
            case "Ride Field":
                {
                    //Update gravity value in component
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (activator != null)
                    {
                        activator.rideSensitivity = value;
                    }
                    break;
                }
        }
    }

    public void updateScale(float value)
    {
        //Get field of object being set and make sure not null
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("not found");
            return;
        }

        //Run update based on object name
        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    //Update modifier in component
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (activator != null)
                    {
                        activator.modifier = value;
                    }
                    break;
                }
            case "Crash Field":
                {
                    //Get component and particle systems
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        //Get new scale based on slider value
                        float newScaleX = activator.defaultFieldX + (value * activator.defaultFieldX);
                        float newScaleY = activator.defaultFieldY + (value * activator.defaultFieldY);
                        //Update field scale
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

                        //Loop through particles and update scale
                        foreach (ParticleSystem ps in particles)
                        {
                            newScaleX = activator.defaultX + (value * activator.defaultX);
                            newScaleY = activator.defaultY + (value * activator.defaultY);
                            ps.transform.localScale = new Vector3(newScaleX, newScaleY, ps.transform.localScale.z);
                        }
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    //Get component and particle systems
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        //get new scale based on slider value
                        float newScaleX = activator.defaultFieldX + (value * activator.defaultFieldX);
                        float newScaleY = activator.defaultFieldY + (value * activator.defaultFieldY);
                        //update field scale
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

                        //Loop through particles and update scale
                        foreach (ParticleSystem ps in particles)
                        {
                            newScaleX = activator.defaultX + (value * activator.defaultX);
                            newScaleY = activator.defaultY + (value * activator.defaultY);
                            ps.transform.localScale = new Vector3(newScaleX, newScaleY, ps.transform.localScale.z);
                        }
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    //Get component and particle systems
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        //Get new scale based on slider value 
                        float newScaleX = value * activator.hatsDefaultFieldScale;
                        float newScaleY = value * activator.hatsDefaultFieldScale;
                        //Update field scale
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

                        //Loop through particles and update scale
                        foreach (ParticleSystem ps in particles)
                        {
                            newScaleX = (value * activator.hatsDefaultXScale);
                            newScaleY = (value * activator.hatsDefaultYScale);
                            ps.transform.localScale = new Vector3(newScaleX, newScaleY, ps.transform.localScale.z);
                        }
                    }
                    break;
                }
            case "Ride Field":
                {
                    //Get component and particle systems
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        //Get new scale on slider value
                        float newScaleX = value * activator.rideFieldDefaultScale;
                        float newScaleY = value * activator.rideFieldDefaultScale;
                        //Update field scale
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

                        //Change inner and outer particles scales based on their own values
                        particles[0].transform.localScale = new Vector3(activator.rideInnerDefaultScale * value, activator.rideInnerDefaultScale * value, field.transform.localScale.z);
                        particles[1].transform.localScale = new Vector3(activator.rideOuterDefaultScale * value, activator.rideOuterDefaultScale * value, field.transform.localScale.z);
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    //Get component and particle systems
                    Toms activator = field.GetComponentInParent<Toms>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        //Get new scale on slider value
                        float newFieldScaleX = activator.defaultFieldX + (value * activator.defaultFieldX);
                        float newFieldScaleY = activator.defaultFieldY + (value * activator.defaultFieldY);
                        //update field scale
                        field.transform.localScale = new Vector3(newFieldScaleX, newFieldScaleY, field.transform.localScale.z);

                        //Loop through particles and update scale
                        foreach (ParticleSystem ps in particles)
                        {
                            float newScaleX = activator.defaultX + (value * activator.defaultX);
                            float newScaleY = activator.defaultY + (value * activator.defaultY);
                            ps.transform.localScale = new Vector3(newScaleX, newScaleY, ps.transform.localScale.z);
                        }
                    }
                    break;
                }
            default:
                {
                    Debug.Log("name not found");
                    break;
                }
        }
    }

    public void changeParticleType(Material mat)
    {
        //get renderer of particle systems
        ParticleSystemRenderer[] particles = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();

        //Loop through and change material to one in param
        foreach (ParticleSystemRenderer ps in particles)
        {
            ps.material = mat;
            ps.trailMaterial = mat;
        }
    }

    public void changeColour(int colourNum)
    {
        //Get particles
        ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        //Get secondary colour toggle
        Toggle toggle = colourPanel.GetComponentInChildren<Toggle>();

        //If toggled set for two colours
        if (toggle.isOn)
        {
            foreach (ParticleSystem ps in particles)
            {
                var mainModule = ps.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(colourPicker1.color, colourPicker2.color);
            }
        }
        else // toggle off, set for one colour
        {
            foreach (ParticleSystem ps in particles)
            {
                var mainModule = ps.main;
                mainModule.startColor = colourPicker1.color;
            }
        }
    }

    //Alternate colour changer for ride having two seperate sets of particles
    public void changeRideHatsColour()
    {
        //Get particles
        ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            //Get field
            ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
            
            //If ride field set colours for ride
            if (field.gameObject.name == "Ride Field")
            {
                //Set main spiral colours
                var mainModule = particles[0].main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(spiralColour1.color, spiralColour2.color);

                //Set outer particles colours
                mainModule = particles[1].main;
                mainModule.startColor = outerColour.color;
            } else
            {
                //Set main spike colour
                var mainModule = particles[0].main;
                mainModule.startColor = hatColour.color;
                //Set opening colour
                mainModule = particles[1].main;
                mainModule.startColor = openingColour.color;
            }
        } 
    }


    public void updateParticleScale(float value)
    {
        //Get field of object being set and make sure not null
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("not found.");
            return;
        }

        //Run update based on object name
        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    //Get particles and component
                    ParticleSystem ps = field.GetComponentInChildren<ParticleSystem>();
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (ps != null && activator != null)
                    {
                        //Update particle scale
                        var mainModule = ps.main;
                        mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                    }
                    break;
                }
            case "Crash Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (particles != null && activator != null)
                    {
                        //Loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update particle scale
                            var mainModule = ps.main;
                            mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                        }
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    if (particles != null && activator != null)
                    {
                        //Loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update particle scale
                            var mainModule = ps.main;
                            mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                        }
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        //Loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update particle scale
                            var mainModule = ps.main;
                            mainModule.startSize = value * activator.hatsPartScale;
                        }
                    }
                    break;
                }
            case "Ride Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        //Update particle scale for inner particles
                        var mainModule = particles[0].main;
                        mainModule.startSize = (value * activator.rideInnerPartScale);
                        //Update particle scale for outer particles
                        mainModule = particles[1].main;
                        mainModule.startSize = new ParticleSystem.MinMaxCurve(activator.rideOuterPartMin * value, activator.rideOuterPartMax * value);
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    //Get particles and component
                    ParticleSystem[] particlesArray = field.GetComponentsInChildren<ParticleSystem>();
                    Toms activator = field.GetComponentInParent<Toms>();
                    //Loop through particles
                    foreach (ParticleSystem p in particlesArray)
                    {
                        //Update particle scale
                        var mainModule = p.main;
                        mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("name not found");
                    break;
                }
        }
    }

    public void updateSimulationSpeed(float value)
    {
        //Get field of object being set and make sure not null
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("not found.");
            return;
        }

        //Run update based on object name
        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    //Get particles and component
                    ParticleSystem ps = field.GetComponentInChildren<ParticleSystem>();
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (ps != null && activator != null)
                    {
                        //Update simulation speed
                        var mainModule = ps.main;
                        mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                    }
                    break;
                }
            case "Crash Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (particles != null && activator != null)
                    {
                        //loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update simulation speed
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                        }
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    if (particles != null && activator != null)
                    {
                        //loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update simulation speed
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                        }
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        //loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update simulation speed
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = (value * activator.hatsSimSpeed);
                        }
                    }
                    break;
                }
            case "Ride Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        //Set inner particles sim speed
                        var mainModule = particles[0].main;
                        mainModule.simulationSpeed = (value * activator.rideSimSpeed);

                        //Set outer particles sim speed
                        mainModule = particles[1].main;
                        mainModule.simulationSpeed = (value * activator.particlesSimSpeed);
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    //Get particles and component
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Toms activator = field.GetComponentInParent<Toms>();
                    if (particles != null && activator != null)
                    {
                        //loop through particles
                        foreach (ParticleSystem ps in particles)
                        {
                            //Update simulation speed
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                        }
                    }
                    break;
                }
            default:
                {
                    Debug.Log("name not found");
                    break;
                }
        }
    }

    //Start coroutine for manually looping particle in menu
    public void particleLooping(ParticleSystemForceField fieldClicked)
    {
        ParticleSystem ps;
        ParticleSystem[] particlesArray;
        ParticleSystem.MainModule main;
        float delay;

        //Determine object by name
        switch (fieldClicked.gameObject.name)
        {
            case "Bass Drum Field":
                //Stop particle system of field
                ps = fieldClicked.GetComponentInChildren<ParticleSystem>();
                main = ps.main;
                main.loop = false;
                //Calculate delay
                delay = main.duration + main.startLifetime.constantMax;
                //activate from component
                bassField.GetComponent<BassDrum>().activate(40);
                //Start manual looping coroutine
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Crash Field":
                //Stop particle system of field
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                //Calculate delay
                delay = main.duration + main.startLifetime.constantMax;
                //activate from component
                crashField.GetComponent<CrashCymbal>().activate(40);
                //Start manual looping coroutine
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Snare Drum Field":
                //Stop particle system of field
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                //Calculate delay
                delay = main.duration + main.startLifetime.constantMax;
                //activate from component
                snareField.GetComponent<SnareDrum>().activate(40);
                //Start manual looping coroutine
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Tom 1 Field":
                //Stop particle system of field
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                //Calculate delay
                delay = main.duration + main.startLifetime.constantMax;
                //activate from component
                fieldClicked.GetComponentInParent<Toms>().activate(48, 40);
                //Start manual looping coroutine
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Tom 2 Field":
                //Stop particle system of field
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                //Calculate delay
                delay = main.duration + main.startLifetime.constantMax;
                //activate from component
                fieldClicked.GetComponentInParent<Toms>().activate(47, 40);
                //Start manual looping coroutine
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Tom 3 Field":
                //Stop particle system of field
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                //Calculate delay
                delay = main.duration + main.startLifetime.constantMax;
                //activate from component
                fieldClicked.GetComponentInParent<Toms>().activate(45, 40);
                //Start manual looping coroutine
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            default:
                Debug.Log("name not found");
                break;
        }
    }

    private IEnumerator restartParticles(ParticleSystemForceField field, float delay)
    {
        //Wait for delay calculated
        yield return new WaitForSeconds(delay);
        //Whilst the solo editor is enabled
        while (editor.enabled)
        {
            //Disable object collider
            field.GetComponent<Collider2D>().enabled = false;
            //Activate based on name
            switch (field.gameObject.name)
            {
                case "Bass Drum Field":
                    field.GetComponent<BassDrum>().activate(40);
                    break;
                case "Crash Field":
                    field.GetComponent<CrashCymbal>().activate(40);
                    break;
                case "Snare Drum Field":
                    field.GetComponent<SnareDrum>().activate(40);
                    break;
                case "Hi-Hat Field":
                case "Ride Field":
                    //Manually play particles as activation functions attached to spinner
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particles)
                    {
                        ps.Play();
                    }
                    break;
                case "Tom 1 Field":
                    field.GetComponentInParent<Toms>().activate(48, 40);
                    break;
                case "Tom 2 Field":
                    field.GetComponentInParent<Toms>().activate(47, 40);
                    break;
                case "Tom 3 Field":
                    field.GetComponentInParent<Toms>().activate(45, 40);
                    break;
                default:
                    Debug.Log("name not found");
                    break;
            }
            //Wait for delay calculated
            yield return new WaitForSeconds(delay);
        }
    }

    //Return particles to normal on solo editor exit
    public void resetParticles()
    {
        ParticleSystem.MainModule main;

        //re-activate crash
        crashField.gameObject.SetActive(true);
        crashField.GetComponent<Collider2D>().enabled = true;
        foreach (ParticleSystem ps in crashParticleSystems)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        //Re-active snare
        snareField.gameObject.SetActive(true);
        snareField.GetComponent<Collider2D>().enabled = true;
        foreach (ParticleSystem ps in snareParticles)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        //Reactivate bass
        bassField.gameObject.SetActive(true);
        bassField.GetComponent<Collider2D>().enabled = true;
        main = bassParticles.main;
        main.loop = true;
        bassParticles.Play();

        //Reactivate ride
        rideField.gameObject.SetActive(true);
        foreach (ParticleSystem ps in rideParticles)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        //Reactivate hihat
        hiHatField.gameObject.SetActive(true);
        foreach (ParticleSystem ps in hiHatParticles)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        //Reactivate toms
        foreach (ParticleSystemForceField tom in tomFields)
        {
            tom.gameObject.SetActive(true);
            tom.GetComponent<Collider2D>().enabled = true;
            foreach (ParticleSystem ps in tom.gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                main = ps.main;
                main.loop = true;
                ps.Play();
            }
        }

        //Move components back to position stored in editor
        crashField.transform.position = crashPosition;
        snareField.transform.position = snarePosition;
        bassField.transform.position = bassPosition;
        rideField.transform.position = ridePosition;
        hiHatField.transform.position = hatPosition;
        for (int i = 0; i < tomFields.Length; i++)
        {
            tomFields[i].transform.position = tomPositions[i];
        }

        //Recenter spinner
        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        spinner.center.x = 0;

        
        //Display correct menus
        setupPage.enabled = true;
        globalEditor.enabled = false;
        editor.enabled = false;
        hatsRideEditor.enabled = false;
    }

    public void globalSetup()
    {
        //enable global editor
        setupPage.enabled = false;
        editor.enabled = false;
        globalEditor.enabled = true;
        colourPanel.SetActive(false);

        //Disable all particles
        crashField.gameObject.SetActive(false);
        snareField.gameObject.SetActive(false);
        bassField.gameObject.SetActive(false);
        rideField.gameObject.SetActive(false);
        hiHatField.gameObject.SetActive(false);
        foreach (ParticleSystemForceField tom in tomFields)
        {
            tom.gameObject.SetActive(false);
        }

        //Move spinner into view
        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        spinner.center.x = 4.5f;
    }

    //Displays correct colour panel for ride and hat elements
    public void editColourRideHats()
    {
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field.gameObject.name == "Ride Field")
        {
            rideColourPanel.SetActive(true);
        }
        else
        {
            hatsColourPanel.SetActive(true);
        }
    }

    public void setSpinnerThickness(float value)
    {
        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        //Update spinner thickness based on slider value
        spinner.trailWidthMod = value;
    }

    public void setSpinnerParticles(Material trailMaterial)
    {
        //Set spinner particles to param material
        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();

        TrailRenderer trail = GameObject.Find("Spinner").GetComponent<TrailRenderer>();

        trail.material = trailMaterial;
    }

    public void setSpinnerColour()
    {
        TrailRenderer trail = GameObject.Find("Spinner").GetComponent<TrailRenderer>();

        //Update the spinner colour by setting the gradient using keys.
        var g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(spinnerPicker.color, 0f),
                new GradientColorKey(spinnerPicker.color, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );

        trail.colorGradient = g;
    }

    public void setBackgroundColour()
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = backgroundPicker.color;
    }


    public void rideMenu()
    {
        //Disable and enable canvas' for ride menu
        setupPage.enabled = false;
        editor.enabled = false;
        globalEditor.enabled = false;
        colourPanel.SetActive(false);
        hatsRideEditor.enabled = true;

        //Disable and enable objects for menu
        crashField.gameObject.SetActive(false);
        snareField.gameObject.SetActive(false);
        bassField.gameObject.SetActive(false);
        hiHatField.gameObject.SetActive(false);
        rideField.gameObject.SetActive(true);

        //Move ride element to be in view
        rideField.gameObject.transform.position = new Vector3(4.5f, 0, 0);
    }

    public void hatsMenu()
    {
        //Disable and enable canvas' for hats menu
        setupPage.enabled = false;
        editor.enabled = false;
        globalEditor.enabled = false;
        colourPanel.SetActive(false);
        hatsRideEditor.enabled = true;

        //Disable and enable objects for menu
        crashField.gameObject.SetActive(false);
        snareField.gameObject.SetActive(false);
        bassField.gameObject.SetActive(false);
        hiHatField.gameObject.SetActive(true);
        rideField.gameObject.SetActive(false);

        //Move hats elements to be in view
        hiHatField.gameObject.transform.position = new Vector3(4.5f, 0, 0);
    }

    //Pause particles for solo menu
    public void pauseParticles(ParticleSystemForceField clickedField)
    {
        //Check which field has been clicked on and set active accordingly
        if (clickedField != crashField)
        {
            crashField.gameObject.SetActive(false);
        }

        if (clickedField != snareField)
        {
            snareField.gameObject.SetActive(false);
        }

        if (bassField != clickedField)
        {
            bassField.gameObject.SetActive(false);
        }

        if (clickedField != rideField)
        {
            rideField.gameObject.SetActive(false);
        }

        if (clickedField != hiHatField)
        {
            hiHatField.gameObject.SetActive(false);
        }

        foreach (ParticleSystemForceField tom in tomFields)
        {
            if (tom != clickedField)
            {
                tom.gameObject.SetActive(false);
            }
        }

        Toggle toggle = colourPanel.GetComponentInChildren<Toggle>();

        ParticleSystem ps = clickedField.GetComponentInChildren<ParticleSystem>();
        var mainModule = ps.main;
        //Set colour picker colours to the colours the element is currently set to.
        if (clickedField != rideField && clickedField != hiHatField)
        {
            ParticleSystem.MinMaxGradient startColor = mainModule.startColor;
            switch (startColor.mode)
            {
                case ParticleSystemGradientMode.Color:
                    colourPicker1.color = startColor.color;
                    toggle.isOn = false;
                    break;

                case ParticleSystemGradientMode.TwoColors:
                    colourPicker1.color = startColor.colorMin;
                    colourPicker2.color = startColor.colorMax;
                    toggle.isOn = true;
                    break;
            }
        }
    }
}