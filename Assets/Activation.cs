using UnityEngine;
using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Multimedia;
using System.Linq;

//Code Adapted from DryWetMidi library docs https://melanchall.github.io/drywetmidi/articles/devices/Input-device.html

public class Activation : MonoBehaviour
{
    private static IInputDevice drumkit;

    private CrashCymbal crash;
    private BassDrum bass;
    private Toms toms;
    private Spinner spinner;
    private SnareDrum snare;

    public static String deviceName;
    public MidiDevice midiDev;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Device name midi interpreter used in development by default
        deviceName = "UMC204HD 192k";

        //Get all midi devices connected to system
        var devices = InputDevice.GetAll();
        //If devices, set to first listed
        if (devices != null && devices.Any())
        {
            deviceName = devices.First().Name;
            Debug.Log(deviceName);
        }
        
        getDrums();

        //Try to set up event listening on midi device
        try
        {
            drumkit = InputDevice.GetByName(deviceName);
            drumkit.EventReceived += OnEventReceived;
            drumkit.StartEventsListening();
        } 
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Exit program using escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Stop editor if in unity dev, otherwise exit program
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    //If stopping program, properly dispose of the drum kit, otherwise issues
    void OnApplicationQuit()
    {
        (drumkit as IDisposable)?.Dispose();
    }

    //Drumkit event handler
    private void OnEventReceived(object sender, MidiEventReceivedEventArgs e){
        //Check if event is a note being played
        if (e.Event.EventType.ToString() == "NoteOn")
        {
            //Format string to get note number and strength as integers
            Dictionary<String, int> hitInfo = new Dictionary<String, int>
            {
                { "note", int.Parse(e.Event.ToString().Split(",")[0].Split("(")[1]) },
                { "strength", int.Parse(e.Event.ToString().Split(",")[1].Split(")")[0]) }
            };
            Debug.Log(hitInfo["note"]);
            Debug.Log(hitInfo["strength"]);
            //Send hit to case statement
            sendHit(hitInfo);
        }//If event is a hi-hat pedal change, set the pedal value in the spinner component
        else if (e.Event.EventType.ToString() == "ControlChange")
        {
            //Format and send new control value
            spinner.setPedal(int.Parse(e.Event.ToString().Split(",")[1].Split(")")[0]));
        }
    }

    void sendHit(Dictionary<String, int> info)
    {
        //send to effect handler based on note
        switch (info["note"])
        {
            case 49:
            case 55:
                Debug.Log("Crash hit!");
                crash.activate(info["strength"]);
                break;
            case 36:
                Debug.Log("Bass Drum Hit!");
                bass.activate(info["strength"]);
                break;
            case 38:
                Debug.Log("Snare Hit!");
                snare.activate(info["strength"]);
                break;
            case 45:
            case 47:
            case 48:
                Debug.Log("Toms!");
                toms.activate(info["note"], info["strength"]);
                break;
            case 42:
            case 44:
            case 46:
                Debug.Log("Hi-Hat");
                spinner.activate(info["strength"], info["note"]);
                break;
            case 51:
            case 53:
                Debug.Log("Ride");
                spinner.activate(info["strength"], info["note"]);
                break;
            default:
                Debug.Log("other hit");
                break;
        }
    }

    //Get drum components in game.
    void getDrums() {
        crash = GameObject.Find("Crash Field").GetComponent<CrashCymbal>();
        bass = GameObject.Find("Bass Drum Field").GetComponent<BassDrum>();
        spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        toms = GameObject.Find("Toms").GetComponent<Toms>();
        snare = GetComponent<SnareDrum>();
    }
}
