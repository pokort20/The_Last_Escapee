using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVM : MonoBehaviour
{
    public Texture2D cursorTexture;
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject controlsMenu;
    // Start is called before the first frame update
    void Start()
    {
        levelsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
        Cursor.SetCursor(cursorTexture, new Vector2(24.0f, 24.0f), CursorMode.ForceSoftware);
        Time.timeScale = 1.0f;
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
