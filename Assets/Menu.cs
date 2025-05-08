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
        //Get page canvases
        home = GameObject.Find("Home").GetComponent<Canvas>();
        setup = GameObject.Find("Setup").GetComponent<Canvas>();
        startingPage = GameObject.Find("StartPage").GetComponent<Canvas>();

        start = GameObject.Find("Start").GetComponent<Button>();
        
        bass = GameObject.Find("BassDrum").GetComponent<BassDrum>();

        //enable default home page
        home.enabled = true;
        setup.enabled = false;
        startingPage.enabled = false;
    }


    public void startSim()
    {
        SceneManager.LoadScene("SampleScene");

    }

    public void setupSim()
    {
        SceneManager.LoadScene("Setup");
    }
}
