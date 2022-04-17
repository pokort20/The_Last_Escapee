using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVM : MonoBehaviour
{
    public Texture2D cursorTexture;
    // Start is called before the first frame update
    void Start()
    {
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
    }
    public void OnSettingsButtonUse()
    {
        Debug.Log("Clicked SETTINGS!");
    }
    public void OnExitButtonUse()
    {
        Debug.Log("Clicked EXIT!");
    }
}
