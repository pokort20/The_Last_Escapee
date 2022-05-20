/// Tutorial class
/**
    This class handles displaying the tutorial hints.
    Other scripts can access this class in order to 
    display desired help.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //Singleton
    public static Tutorial instance;

    //Public variables defined in Unity inspector
    public bool tutorialEnabled;
    public GameObject movementHelp;
    public GameObject inventoryHelp;
    public GameObject flashlightHelp;
    public GameObject batteriesHelp;
    public GameObject medkitHelp;
    public GameObject staminaShotHelp;
    public GameObject moveObjectHelp;
    public GameObject lightHelp;

    public float helpDurationTime;

    //Other variables
    private Dictionary<string, Tuple<GameObject, bool>> helps;
    private float helpDuration;

    //Init
    void Awake()
    {
        if(tutorialEnabled)
        {
            Debug.Log("Started Tutorial");
            if (instance != null)
            {
                Debug.LogError("Multiple instances of tutorial!!!");
                return;
            }
            instance = this;
        }
        else
        {
            instance = null;
        }
        helps = new Dictionary<string, Tuple<GameObject, bool>>();
    }
    void Start()
    {
        helps.Add("movement", new Tuple<GameObject, bool>(movementHelp, false));
        helps.Add("inventory", new Tuple<GameObject, bool>(inventoryHelp, false));
        helps.Add("flashlight", new Tuple<GameObject, bool>(flashlightHelp, false));
        helps.Add("batteries", new Tuple<GameObject, bool>(batteriesHelp, false));
        helps.Add("medkit", new Tuple<GameObject, bool>(medkitHelp, false));
        helps.Add("staminaShot", new Tuple<GameObject, bool>(staminaShotHelp, false));
        helps.Add("moveObject", new Tuple<GameObject, bool>(moveObjectHelp, false));
        helps.Add("light", new Tuple<GameObject, bool>(lightHelp, false));

        hideAllHelps();
    }

    //Update
    void Update()
    {
        if (!tutorialEnabled) return;
        helpDuration -= Time.deltaTime;
        if(helpDuration <= 0.0f)
        {
            hideAllHelps();
        }
    }

    //Functions
    public void hideAllHelps()
    {
        if (!tutorialEnabled) return;
        foreach (KeyValuePair<string, Tuple<GameObject, bool>> help in helps)
        {
            help.Value.Item1.SetActive(false);
        }
    }
    public void hideHelp(string helpName)
    {
        if (!tutorialEnabled) return;
        Tuple<GameObject, bool> help;
        if (helps != null)
        {
            helps.TryGetValue(helpName, out help);
        }
        else
        {
            return;
        }
        if(help != null)
        {
            help.Item1.SetActive(false);
        }
    }
    public void showHelp(string helpName)
    {
        if (!tutorialEnabled) return;
        Tuple<GameObject, bool> help;
        if (helps != null)
        {
            helps.TryGetValue(helpName, out help);
        }
        else
        {
            return;
        }
        if (help!=null)
        {
            if (help.Item2)
            {
                return;
            }
            hideAllHelps();
            help.Item1.SetActive(true);
            helps[helpName] = new Tuple<GameObject, bool>(help.Item1, true);
            helpDuration = helpDurationTime;
        }
        else
        {
            Debug.LogWarning("Help with the name: " + helpName + " is not in the help dictionary");
        }
    }
}
