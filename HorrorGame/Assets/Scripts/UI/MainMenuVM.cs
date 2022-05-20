/// Main menu VM class
/**
    This class handles the main menu UI and the
    logic behind.
*/

using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuVM : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public Texture2D cursorTexture;
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject controlsMenu;
    public GameObject settingsMenu;

    public Scrollbar brightnessBar;
    public Scrollbar volumeBar;

    //Init
    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(24.0f, 24.0f), CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1.0f;
        try
        {
            string path = Application.dataPath + "/Saves/Level2.sejv";
            if (!File.Exists(path))
            {
                levelsMenu.transform.GetChild(1).gameObject.GetComponentInChildren<Button>().interactable = false;
                levelsMenu.transform.GetChild(1).gameObject.GetComponentInChildren<HoverButtonHighlight>().enabled = false;
            }
            path = Application.dataPath + "/Saves/Level3.sejv";
            if (!File.Exists(path))
            {
                levelsMenu.transform.GetChild(2).gameObject.GetComponentInChildren<Button>().interactable = false;
                levelsMenu.transform.GetChild(2).gameObject.GetComponentInChildren<HoverButtonHighlight>().enabled = false;
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to disable levels menu item, " + e.Message);
        }
        levelsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //Update
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            levelsMenu.SetActive(false);
            controlsMenu.SetActive(false);
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    //Button functions
    public void OnPlayButtonUse()
    {
        Debug.Log("Clicked PLAY!");
        GameManager.instance.saveSettingsData();
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void OnLevelsButtonUse()
    {
        Debug.Log("Clicked LEVELS!");
        AudioManager.instance.playAudio("menu_click");
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
    }
    public void OnControlsButtonUse()
    {
        Debug.Log("Clicked CONTROLS!");
        AudioManager.instance.playAudio("menu_click");
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
    public void OnSettingsButtonUse()
    {
        Debug.Log("Clicked SETTINGS");
        AudioManager.instance.playAudio("menu_click");
        mainMenu.SetActive(false);
        brightnessBar.value = PostProcessing.instance.brightness;
        volumeBar.value = AudioManager.instance.generalVolume;
        settingsMenu.SetActive(true);
    }
    public void OnExitButtonUse()
    {
        Debug.Log("Clicked EXIT!");
        AudioManager.instance.playAudio("menu_click");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void OnBackButtonUse()
    {
        Debug.Log("Clicked BACK!");
        AudioManager.instance.playAudio("menu_click");
        levelsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void OnLevel1ButtonUse()
    {
        Debug.Log("Clicked LEVEL1!");
        GameManager.instance.saveSettingsData();
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void OnLevel2ButtonUse()
    {
        Debug.Log("Clicked LEVEL2!");
        GameManager.instance.saveSettingsData();
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }
    public void OnLevel3ButtonUse()
    {
        Debug.Log("Clicked LEVEL3!");
        GameManager.instance.saveSettingsData();
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }
    public void OnBrightnessValueChanged()
    {
        if(PostProcessing.instance != null)
        {
            PostProcessing.instance.brightness = brightnessBar.value;
        }
    }
    public void OnVolumeValueChanged()
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.generalVolume = volumeBar.value;
        }
    }
}
