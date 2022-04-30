using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuVM : MonoBehaviour
{
    public Texture2D cursorTexture;
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject controlsMenu;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(24.0f, 24.0f), CursorMode.ForceSoftware);
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            levelsMenu.SetActive(false);
            controlsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void OnPlayButtonUse()
    {
        Debug.Log("Clicked PLAY!");
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
        mainMenu.SetActive(true);
    }
    public void OnLevel1ButtonUse()
    {
        Debug.Log("Clicked LEVEL1!");
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void OnLevel2ButtonUse()
    {
        Debug.Log("Clicked LEVEL2!");
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }
    public void OnLevel3ButtonUse()
    {
        Debug.Log("Clicked LEVEL3!");
        AudioManager.instance.playAudio("menu_click");
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        //SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }
}
