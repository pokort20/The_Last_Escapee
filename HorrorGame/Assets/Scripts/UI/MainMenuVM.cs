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
    }
    public void OnPlayButtonUse()
    {
        Debug.Log("Clicked PLAY!");
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void OnLevelsButtonUse()
    {
        Debug.Log("Clicked LEVELS!");
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
    }
    public void OnControlsButtonUse()
    {
        Debug.Log("Clicked CONTROLS!");
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
    public void OnExitButtonUse()
    {
        Debug.Log("Clicked EXIT!");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void OnBackButtonUse()
    {
        Debug.Log("Clicked BACK!");
        levelsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void OnLevel1ButtonUse()
    {
        Debug.Log("Clicked LEVEL1!");
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void OnLevel2ButtonUse()
    {
        Debug.Log("Clicked LEVEL2!");
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }
    public void OnLevel3ButtonUse()
    {
        Debug.Log("Clicked LEVEL3!");
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        //SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }
}
