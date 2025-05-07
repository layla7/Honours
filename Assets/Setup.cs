using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    Activation activation;

    public FlexibleColorPicker colourPicker1;
    public FlexibleColorPicker colourPicker2;
    public FlexibleColorPicker spiralColour1; 
    public FlexibleColorPicker spiralColour2; 
    public FlexibleColorPicker outerColour;
    public FlexibleColorPicker spinnerPicker;
    public FlexibleColorPicker backgroundPicker;
    public FlexibleColorPicker hatColour;
    public FlexibleColorPicker openingColour;

    public ParticleSystem[] crashParticleSystems;
    public ParticleSystemForceField crashField;
    public Vector3 crashPosition;

    public ParticleSystem[] rideParticles;
    public ParticleSystemForceField rideField;
    public Vector3 ridePosition;

    public GameObject ride;
    public GameObject hihatSpike;
    //public Spinner spin;

    public ParticleSystem[] hiHatParticles;
    public ParticleSystemForceField hiHatField;
    public Vector3 hatPosition;

    public ParticleSystemForceField snareField;
    public ParticleSystem[] snareParticles;
    public Vector3 snarePosition;

    public ParticleSystem bassParticles;
    public ParticleSystemForceField bassField;
    public Vector3 bassPosition;

    public ParticleSystemForceField[] tomFields = new ParticleSystemForceField[3];
    public Vector3[] tomPositions = new Vector3[3];

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
        setupPage = GameObject.Find("SetupPage").GetComponent<Canvas>();
        editor = GameObject.Find("Particle Editor").GetComponent<Canvas>();
        globalEditor = GameObject.Find("Global Editor").GetComponent<Canvas>();
        hatsRideEditor = GameObject.Find("Crash Ride Editor").GetComponent<Canvas>();
        labels = GameObject.Find("Labels").GetComponent<Canvas>();
        colourPanel = GameObject.Find("ColourPanel");
        //rideColourPanel = GameObject.Find("RideColourPanel");
        //hatsColourPanel = GameObject.Find("HatsColourPanel");



        setupPage.enabled = true;
        editor.enabled = false;
        hatsRideEditor.enabled = false;
        globalEditor.enabled = false;
        colourPanel.SetActive(false);
        rideColourPanel.SetActive(false);
        hatsColourPanel.SetActive(false);

        crashParticleSystems = GameObject.Find("Crash Field").GetComponentsInChildren<ParticleSystem>();
        crashField = GameObject.Find("Crash Field").GetComponent<ParticleSystemForceField>();

        rideParticles = GameObject.Find("Ride Field").GetComponentsInChildren<ParticleSystem>();
        rideField = GameObject.Find("Ride Field").GetComponent<ParticleSystemForceField>();

        hiHatParticles = GameObject.Find("Hi-Hat Field").GetComponentsInChildren<ParticleSystem>();
        hiHatField = GameObject.Find("Hi-Hat Field").GetComponent<ParticleSystemForceField>();

        snareField = GameObject.Find("Snare Drum Field").GetComponent<ParticleSystemForceField>();
        snareParticles = GameObject.Find("Snare Drum Field").GetComponentsInChildren<ParticleSystem>();

        bassParticles = GameObject.Find("Bass Drum Field").GetComponentInChildren<ParticleSystem>();
        bassField = GameObject.Find("Bass Drum Field").GetComponent<ParticleSystemForceField>();

        tomFields[0] = GameObject.Find("Tom 1 Field").GetComponent<ParticleSystemForceField>();
        tomFields[1] = GameObject.Find("Tom 2 Field").GetComponent<ParticleSystemForceField>();
        tomFields[2] = GameObject.Find("Tom 3 Field").GetComponent<ParticleSystemForceField>();

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
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        
    }

    public void goToScene()
    {
        Spinner spinner = gameObject.GetComponentInChildren<Spinner>();

        spinner.hihatSpike = hiHatField.gameObject;

        spinner.ride = rideField.gameObject;

        SnareDrum snare = gameObject.GetComponent<SnareDrum>();

        snare.snare = snareField.gameObject;

        /**
        ParticleSystemForceField ff = ride.GetComponent<ParticleSystemForceField>();
        ff = rideField;

        ff = hihatSpike.GetComponent<ParticleSystemForceField>();
        ff = hiHatField;

        ParticleSystem[] hatParticles = hihatSpike.GetComponentsInChildren<ParticleSystem>();
        hatParticles[0] = hiHatParticles[0];
        hatParticles[1] = hiHatParticles[1];
        **/

        SceneManager.LoadScene("CustomVisuals");



        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            var main = ps.main;
            main.loop = false;
        }

        ParticleClick[] clickables = GetComponentsInChildren<ParticleClick>();

        foreach (ParticleClick clickable in clickables)
        {
            clickable.enabled = false;
        }

        gameObject.GetComponent<Activation>().enabled = true;
        gameObject.GetComponent<SnareDrum>().enabled = true;
    }

    public void updateSensitivity(float value)
    {
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("updateScale: No ParticleSystemForceField found.");
            return;
        }

        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                    }
                    break; 
                }
            case "Crash Field":
                {
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                    }
                    break;
                }
            case "Snare Drum Field":
                {
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
                    Toms activator = field.GetComponentInParent<Toms>();
                    if (activator != null)
                    {
                        activator.sensitivity = value;
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (activator != null)
                    {
                        activator.hatsSensitivity = value;
                    }
                    break;
                }
            case "Ride Field":
                {
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
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("updateScale: No ParticleSystemForceField found.");
            return;
        }

        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                    }
                    break;
                }
            case "Crash Field":
                {
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                    }
                    break;
                }
            case "Snare Drum Field":
                {
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
                    Toms activator = field.GetComponentInParent<Toms>();
                    if (activator != null)
                    {
                        activator.gravSetting = value;
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (activator != null)
                    {
                        activator.hatsSensitivity = value;
                    }
                    break;
                }
            case "Ride Field":
                {
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
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("updateScale: No ParticleSystemForceField found.");
            return;
        }

        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (activator != null)
                    {
                        activator.modifier = value;
                    }
                    break;
                }
            case "Crash Field":
                {
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        float newScaleX = activator.defaultFieldX + (value * activator.defaultFieldX);
                        float newScaleY = activator.defaultFieldY + (value * activator.defaultFieldY);
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

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
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        float newScaleX = activator.defaultFieldX + (value * activator.defaultFieldX);
                        float newScaleY = activator.defaultFieldY + (value * activator.defaultFieldY);
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

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
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        float newScaleX = value * activator.hatsDefaultFieldScale;
                        float newScaleY = value * activator.hatsDefaultFieldScale;
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

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
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        float newScaleX = value * activator.rideFieldDefaultScale;
                        float newScaleY = value * activator.rideFieldDefaultScale;
                        field.transform.localScale = new Vector3(newScaleX, newScaleY, field.transform.localScale.z);

                        particles[0].transform.localScale = new Vector3(activator.rideInnerDefaultScale * value, activator.rideInnerDefaultScale * value, field.transform.localScale.z);
                        particles[1].transform.localScale = new Vector3(activator.rideOuterDefaultScale * value, activator.rideOuterDefaultScale * value, field.transform.localScale.z);


                        ParticleSystem[] prefabParticles = ride.GetComponentsInChildren<ParticleSystem>();
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    Toms activator = field.GetComponentInParent<Toms>();
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    if (activator != null)
                    {
                        float newFieldScaleX = activator.defaultFieldX + (value * activator.defaultFieldX);
                        float newFieldScaleY = activator.defaultFieldY + (value * activator.defaultFieldY);
                        //float newSpeed = activator.defaultStartSpeed + (value * activator.defaultStartSpeed);
                        field.transform.localScale = new Vector3(newFieldScaleX, newFieldScaleY, field.transform.localScale.z);

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
                    Debug.Log("updateScale: No matching field found for updating scale.");
                    break;
                }
        }
    }

    public void changeParticleType(Material mat)
    {
        ParticleSystemRenderer[] particles = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();

        foreach (ParticleSystemRenderer ps in particles)
        {
            ps.material = mat;
            ps.trailMaterial = mat;
        }
    }

    public void changeColour(int colourNum)
    {
        ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        Toggle toggle = colourPanel.GetComponentInChildren<Toggle>();

        if (toggle.isOn)
        {
            foreach (ParticleSystem ps in particles)
            {
                var mainModule = ps.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(colourPicker1.color, colourPicker2.color);
            }
        }
        else
        {
            foreach (ParticleSystem ps in particles)
            {
                var mainModule = ps.main;
                mainModule.startColor = colourPicker1.color;
            }
        }
    }

    public void changeRideColour()
    {
        ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
            if (field.gameObject.name == "Ride Field")
            {
                var mainModule = particles[0].main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(spiralColour1.color, spiralColour2.color);

                mainModule = particles[1].main;
                mainModule.startColor = outerColour.color;
            } else
            {
                var mainModule = particles[0].main;
                mainModule.startColor = hatColour.color;

                mainModule = particles[1].main;
                mainModule.startColor = openingColour.color;
            }
        } 
    }


    public void updateParticleScale(float value)
    {
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();
        if (field == null)
        {
            Debug.Log("updateParticleScale: No ParticleSystemForceField found.");
            return;
        }

        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    ParticleSystem ps = field.GetComponentInChildren<ParticleSystem>();
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (ps != null && activator != null)
                    {
                        var mainModule = ps.main;
                        mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                    }
                    break;
                }
            case "Crash Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                        }
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                        }
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.startSize = value * activator.hatsPartScale;
                        }
                    }
                    break;
                }
            case "Ride Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        var mainModule = particles[0].main;
                        mainModule.startSize = (value * activator.rideInnerPartScale);

                        mainModule = particles[1].main;
                        mainModule.startSize = new ParticleSystem.MinMaxCurve(activator.rideOuterPartMin * value, activator.rideOuterPartMax * value);
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    ParticleSystem[] particlesArray = field.GetComponentsInChildren<ParticleSystem>();
                    Toms activator = field.GetComponentInParent<Toms>();
                    foreach (ParticleSystem p in particlesArray)
                    {
                        var mainModule = p.main;
                        mainModule.startSize = activator.defaultParticleScale + (value * activator.defaultParticleScale);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("updateParticleScale: No matching field found for updating particle scale.");
                    break;
                }
        }
    }

    public void updateSimulationSpeed(float value)
    {
        ParticleSystemForceField field = gameObject.GetComponentInChildren<ParticleSystemForceField>();

        switch (field.gameObject.name)
        {
            case "Bass Drum Field":
                {
                    ParticleSystem ps = field.GetComponentInChildren<ParticleSystem>();
                    BassDrum activator = field.GetComponent<BassDrum>();
                    if (ps != null && activator != null)
                    {
                        var mainModule = ps.main;
                        mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                    }
                    break;
                }
            case "Crash Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    CrashCymbal activator = field.GetComponent<CrashCymbal>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                        }
                    }
                    break;
                }
            case "Snare Drum Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    SnareDrum activator = field.GetComponent<SnareDrum>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                        }
                    }
                    break;
                }
            case "Hi-Hat Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = (value * activator.hatsSimSpeed);
                        }
                    }
                    break;
                }
            case "Ride Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Spinner activator = gameObject.GetComponentInChildren<Spinner>();
                    if (particles != null && activator != null)
                    {
                        var mainModule = particles[0].main;
                        mainModule.simulationSpeed = (value * activator.rideSimSpeed);

                        mainModule = particles[1].main;
                        mainModule.simulationSpeed = (value * activator.particlesSimSpeed);
                    }
                    break;
                }
            case "Tom 1 Field":
            case "Tom 2 Field":
            case "Tom 3 Field":
                {
                    ParticleSystem[] particles = field.GetComponentsInChildren<ParticleSystem>();
                    Toms activator = field.GetComponentInParent<Toms>();
                    if (particles != null && activator != null)
                    {
                        foreach (ParticleSystem ps in particles)
                        {
                            var mainModule = ps.main;
                            mainModule.simulationSpeed = activator.defaultSimulationSpeed + (value * activator.defaultSimulationSpeed);
                        }
                    }
                    break;
                }
            default:
                {
                    Debug.Log("updateSimulationSpeed: No matching field found for updating simulation speed.");
                    break;
                }
        }
    }

    public void particleLooping(ParticleSystemForceField fieldClicked)
    {
        ParticleSystem ps;
        ParticleSystem[] particlesArray;
        ParticleSystem.MainModule main;
        float delay;
        switch (fieldClicked.gameObject.name)
        {
            case "Bass Drum Field":
                ps = fieldClicked.GetComponentInChildren<ParticleSystem>();
                main = ps.main;
                main.loop = false;
                delay = main.duration + main.startLifetime.constantMax;
                bassField.GetComponent<BassDrum>().activate(40);
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Crash Field":
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                delay = main.duration + main.startLifetime.constantMax;
                crashField.GetComponent<CrashCymbal>().activate(40);
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Snare Drum Field":
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                delay = main.duration + main.startLifetime.constantMax;
                snareField.GetComponent<SnareDrum>().activate(40);
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Tom 1 Field":
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                delay = main.duration + main.startLifetime.constantMax;
                fieldClicked.GetComponentInParent<Toms>().activate(48, 40);
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Tom 2 Field":
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                delay = main.duration + main.startLifetime.constantMax;
                fieldClicked.GetComponentInParent<Toms>().activate(47, 40);
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            case "Tom 3 Field":
                particlesArray = fieldClicked.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particles in particlesArray)
                {
                    main = particles.main;
                    main.loop = false;
                }
                delay = main.duration + main.startLifetime.constantMax;
                fieldClicked.GetComponentInParent<Toms>().activate(45, 40);
                StartCoroutine(restartParticles(fieldClicked, delay));
                break;
            default:
                Debug.Log("IDFK");
                break;
        }
    }

    private IEnumerator restartParticles(ParticleSystemForceField field, float delay)
    {
        yield return new WaitForSeconds(delay);
        while (editor.enabled)
        {
            field.GetComponent<Collider2D>().enabled = false;
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
                    Debug.Log("IDFK");
                    break;
            }
            yield return new WaitForSeconds(delay);
        }
    }

    public void resetParticles()
    {
        ParticleSystem.MainModule main;
        crashField.gameObject.SetActive(true);
        crashField.GetComponent<Collider2D>().enabled = true;
        foreach (ParticleSystem ps in crashParticleSystems)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        snareField.gameObject.SetActive(true);
        snareField.GetComponent<Collider2D>().enabled = true;
        foreach (ParticleSystem ps in snareParticles)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        bassField.gameObject.SetActive(true);
        bassField.GetComponent<Collider2D>().enabled = true;
        main = bassParticles.main;
        main.loop = true;
        bassParticles.Play();

        rideField.gameObject.SetActive(true);
        //rideField.GetComponent<Collider2D>().enabled = true;
        foreach (ParticleSystem ps in rideParticles)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

        hiHatField.gameObject.SetActive(true);
        //hiHatField.GetComponent<Collider2D>().enabled = true;
        foreach (ParticleSystem ps in hiHatParticles)
        {
            main = ps.main;
            main.loop = true;
            ps.Play();
        }

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

        crashField.transform.position = crashPosition;
        snareField.transform.position = snarePosition;
        bassField.transform.position = bassPosition;
        rideField.transform.position = ridePosition;
        hiHatField.transform.position = hatPosition;
        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        spinner.center.x = 0;

        for (int i = 0; i < tomFields.Length; i++)
        {
            tomFields[i].transform.position = tomPositions[i];
        }

        setupPage.enabled = true;
        globalEditor.enabled = false;
        editor.enabled = false;
        hatsRideEditor.enabled = false;
    }

    public void globalSetup()
    {
        setupPage.enabled = false;
        editor.enabled = false;
        globalEditor.enabled = true;
        colourPanel.SetActive(false);


        crashField.gameObject.SetActive(false);
        snareField.gameObject.SetActive(false);
        bassField.gameObject.SetActive(false);
        rideField.gameObject.SetActive(false);
        hiHatField.gameObject.SetActive(false);

        foreach (ParticleSystemForceField tom in tomFields)
        {
            tom.gameObject.SetActive(false);
        }

        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        spinner.center.x = 4.5f;
    }

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
        spinner.trailWidthMod = value;
    }

    public void setSpinnerParticles(Material trailMaterial)
    {
        Spinner spinner = GameObject.Find("Spinner").GetComponent<Spinner>();

        TrailRenderer trail = GameObject.Find("Spinner").GetComponent<TrailRenderer>();

        trail.material = trailMaterial;
    }

    public void setSpinnerColour()
    {
        TrailRenderer trail = GameObject.Find("Spinner").GetComponent<TrailRenderer>();

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
        setupPage.enabled = false;
        editor.enabled = false;
        globalEditor.enabled = false;
        colourPanel.SetActive(false);
        hatsRideEditor.enabled = true;


        crashField.gameObject.SetActive(false);
        snareField.gameObject.SetActive(false);
        bassField.gameObject.SetActive(false);
        hiHatField.gameObject.SetActive(false);
        rideField.gameObject.SetActive(true);

        rideField.gameObject.transform.position = new Vector3(4.5f, 0, 0);

        ParticleSystem[] particles = rideField.gameObject.GetComponentsInChildren<ParticleSystem>();

        
    }

    public void hatsMenu()
    {
        setupPage.enabled = false;
        editor.enabled = false;
        globalEditor.enabled = false;
        colourPanel.SetActive(false);
        hatsRideEditor.enabled = true;


        crashField.gameObject.SetActive(false);
        snareField.gameObject.SetActive(false);
        bassField.gameObject.SetActive(false);
        hiHatField.gameObject.SetActive(true);
        rideField.gameObject.SetActive(false);

        hiHatField.gameObject.transform.position = new Vector3(4.5f, 0, 0);

        ParticleSystem[] particles = hiHatField.gameObject.GetComponentsInChildren<ParticleSystem>();


    }

    public void pauseParticles(ParticleSystemForceField clickedField)
    {
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
        

        Debug.Log("All particle systems have been paused except the clicked one.");
    }
}