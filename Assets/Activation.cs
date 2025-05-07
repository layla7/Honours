using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Multimedia;
using System.Linq;

//Code Adapted from DryWetMidi library docs https://melanchall.github.io/drywetmidi/articles/devices/Input-device.html

public class Activation : MonoBehaviour
{
    private static IInputDevice drumkit;

    private CrashCymbal crash;
    private RideCymbal ride;
    private BassDrum bass;
    private Toms toms;
    private Spinner spinner;
    private SnareDrum snare;

    public static String deviceName;
    public MidiDevice midiDev;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deviceName = "UMC204HD 192k";
        var devices = InputDevice.GetAll();
        if (devices != null && devices.Any())
        {
            deviceName = devices.First().Name;
            Debug.Log(deviceName);
        }
        getDrums();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    void OnApplicationQuit()
    {
        (drumkit as IDisposable)?.Dispose();
    }

    private void OnEventReceived(object sender, MidiEventReceivedEventArgs e){
        var midiDevice = (MidiDevice)sender;

        
        if (e.Event.EventType.ToString() == "NoteOn") {
            Dictionary<String, int> hitInfo = new Dictionary<String, int>
            {
                { "note", int.Parse(e.Event.ToString().Split(",")[0].Split("(")[1]) },
                { "strength", int.Parse(e.Event.ToString().Split(",")[1].Split(")")[0]) }
            };
            Debug.Log(hitInfo["note"]);
            Debug.Log(hitInfo["strength"]);
            
            sendHit(hitInfo);
        } else if (e.Event.EventType.ToString() == "ControlChange")
        {
            spinner.setPedal(int.Parse(e.Event.ToString().Split(",")[1].Split(")")[0]));
            // Debug.Log(e.Event.ToString());
        }

        //if (e.Event.ToString() == "Timing Clock" || e.Event.ToString() == "Active Sensing" || e.Event.ToString() == "Note Off") return;

        //Debug.Log($"Event received from '{midiDevice.Name}' at {DateTime.Now.ToString("hh.mm.ss.ffffff")}: {e.Event.ToString()}");

        //Debug.Log(e);
    }

    void sendHit(Dictionary<String, int> info)
    {
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

    void getDrums() {
        crash = GameObject.Find("Crash Field").GetComponent<CrashCymbal>();
        bass = GameObject.Find("Bass Drum Field").GetComponent<BassDrum>();
        spinner = GameObject.Find("Spinner").GetComponent<Spinner>();
        toms = GameObject.Find("Toms").GetComponent<Toms>();
        snare = GetComponent<SnareDrum>();
    }
}
