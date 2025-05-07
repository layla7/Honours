using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Multimedia;
using TMPro;
using System.Collections.Generic;
using System.Linq; 

public class Menu : MonoBehaviour
{
    private static Canvas home;
    private static Canvas setup;
    private static Canvas startingPage;
    private BassDrum bass;
    //static int page;
    private Button start;
    public static string selectedMidiDeviceName = "";
    
    void Start()
    {
        home = GameObject.Find("Home").GetComponent<Canvas>();
        setup = GameObject.Find("Setup").GetComponent<Canvas>();
        startingPage = GameObject.Find("StartPage").GetComponent<Canvas>();

        start = GameObject.Find("Start").GetComponent<Button>();
        
        bass = GameObject.Find("BassDrum").GetComponent<BassDrum>();
        home.enabled = true;
        setup.enabled = false;
        startingPage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //pageNav(0);
    }

    public static void pageNav(int page) { 
        switch(page)
        {
            case 0:
                home.enabled = true;
                setup.enabled = false;
                break;
            case 1:
                home.enabled = false;
                setup.enabled = true;
                break;      
            default:
                home.enabled = true;
                setup.enabled = false;
                break;

        }
    }

    public void startSim()
    {
        SceneManager.LoadScene("SampleScene");

    }

    public void setupSim()
    {
        SceneManager.LoadScene("Setup");
    }


    public static void startPage()
    {
        home.enabled = false;
        startingPage.enabled = true;

        TMP_Dropdown midiDropdown = GameObject.Find("MIDI Dropdown").GetComponent<TMP_Dropdown>();

        midiDropdown.ClearOptions();

       
        var inputDevices = InputDevice.GetAll().ToList();

        List<string> options = new List<string>();

        if (inputDevices.Count == 0)
        {
            options.Add("No devices found");
            midiDropdown.AddOptions(options);
        }
        else
        {
            foreach (var device in inputDevices)
            {
                options.Add(device.Name);
            }
            midiDropdown.AddOptions(options);

            midiDropdown.onValueChanged.RemoveAllListeners();
            
            midiDropdown.onValueChanged.AddListener(delegate { OnMidiDeviceSelected(midiDropdown); });
        }
    }

    
    private static void OnMidiDeviceSelected(TMP_Dropdown dropdown)
    {
        Activation.deviceName = dropdown.options[dropdown.value].text;
    }


    public static void setupPage ()
    {
        home.enabled = false;
        setup.enabled = true;

        Slider stability = GameObject.Find("StabilitySlider").GetComponent<Slider>();
        stability.enabled = true;
        //if (stability.value )

    }

    public void playHit()
    {
        bass.activate(100);
        return;
    } 
}
