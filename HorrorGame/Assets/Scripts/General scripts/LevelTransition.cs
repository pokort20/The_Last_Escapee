using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string sceneName;

    private GameManager gameManager;
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        if(sceneName == null || sceneName == "")
        {
            Debug.LogWarning("Level transition scene name not specified in inspector!");
        }
        gameManager = GameManager.instance;
        inventory = Inventory.instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.LogWarning("Player switching level!");
            //destroy scene transition from previous level
            SceneTransitionData std = FindObjectOfType<SceneTransitionData>();
            if (std != null)
            {
                Destroy(std.gameObject);
            }
            //create new scene transition for the following level
            GameObject ga = new GameObject();
            ga.name = "SceneTransition";
            ga.AddComponent<SceneTransitionData>();
            std = ga.GetComponent<SceneTransitionData>();
            std.health = gameManager.health;
            std.stamina = gameManager.stamina;
            std.batteryLevel = gameManager.batteryLevel;
            std.flashlightEnabled = gameManager.flashlightEnabled;
            std.inventoryItems = inventory.items;
            DontDestroyOnLoad(ga);

            try
            {
                SceneManager.LoadScene(sceneName);
            }
            catch(Exception e)
            {
                Debug.LogError("Loading scene failed: " + e.Message);
            }
        }
    }
}
