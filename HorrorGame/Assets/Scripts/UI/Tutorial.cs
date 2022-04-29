using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject movementHelp;
    public GameObject inventoryHelp;
    public GameObject flashlightHelp;
    public GameObject batteriesHelp;
    public GameObject medkitHelp;
    public GameObject staminaShotHelp;
    public float helpDurationTime;

    //private List<GameObject> helps;
    private Dictionary<string, GameObject> helps;
    private float helpDuration;
    // Start is called before the first frame update
    void Start()
    {
        helps = new Dictionary<string, GameObject>();
        helps.Add("movement", movementHelp);
        helps.Add("inventory", inventoryHelp);
        helps.Add("flashlight", flashlightHelp);
        helps.Add("batteries", batteriesHelp);
        helps.Add("medkit", medkitHelp);
        helps.Add("staminaShot", staminaShotHelp);

        hideAllHelps();
    }

    // Update is called once per frame
    void Update()
    {
        helpDuration -= Time.time;
        if(helpDuration <= 0.0f)
        {
            hideAllHelps();
        }
    }
    public void hideAllHelps()
    {
        foreach(KeyValuePair<string, GameObject> help in helps)
        {
            help.Value.SetActive(false);
        }
    }
    public void showHelp(string helpName)
    {
        helps.TryGetValue(helpName, out GameObject help);
        if (help!=null)
        {
            hideAllHelps();
            help.SetActive(true);
            helpDuration = helpDurationTime;
        }
        else
        {
            Debug.LogWarning("Help with the name: " + helpName + " is not in the help dictionary");
        }
    }
    /*
    public void showMovementHelp()
    {
        hideAllHelps();
        movementHelp.SetActive(true);
        helpDuration = helpDurationTime;
    }
    public void showInventoryHelp()
    {
        hideAllHelps();
        inventoryHelp.SetActive(true);
        helpDuration = helpDurationTime;
    }
    public void showFlashlightHelp()
    {
        hideAllHelps();
        flashlightHelp.SetActive(true);
        helpDuration = helpDurationTime;
    }
    public void showBatteriesHelp()
    {
        hideAllHelps();
        batteriesHelp.SetActive(true);
        helpDuration = helpDurationTime;
    }
    public void showMedkitHelp()
    {
        hideAllHelps();
        medkitHelp.SetActive(true);
        helpDuration = helpDurationTime;
    }
    public void showStaminaShotHelp()
    {
        hideAllHelps();
        staminaShotHelp.SetActive(true);
        helpDuration = helpDurationTime;
    }
    */
}
