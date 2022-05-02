using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveLoadSystem
{
    public static bool SaveGame(SceneTransitionData sceneData, string saveName)
    {
        bool success;
        try
        {
            SaveData saveData = new SaveData(sceneData);
            BinaryFormatter bf = new BinaryFormatter();
            Directory.CreateDirectory(Application.dataPath + "/Saves/");
            string path = Application.dataPath + "/Saves/" + saveName + ".sejv";
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, saveData);
            fs.Close();
            success = true;
        }
        catch(Exception e)
        {
            Debug.LogError("Saving the game FAILED! " + e.Message);
            success = false;
        }
        return success;
    }
    public static SceneTransitionData LoadGame(string saveName)
    {
        SceneTransitionData sceneData;
        SaveData saveData;

        //try loading save file
        try
        {

            string path = Application.dataPath + "/Saves/" + saveName + ".sejv";
            if(File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open);
                saveData = bf.Deserialize(fs) as SaveData;
                fs.Close();
            }
            else
            {
                Debug.LogWarning("Loading the game FAILED, save file not found!");
                return null;
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Loading the game FAILED! " + e.Message);
            return null;
        }

        //decode loaded data to scene data
        if(saveData != null)
        {
            GameObject sceneDataObject = new GameObject("SceneData");
            sceneDataObject.AddComponent<SceneTransitionData>();
            sceneData = sceneDataObject.GetComponent<SceneTransitionData>();
            sceneData.health = saveData.health;
            sceneData.stamina = saveData.stamina;
            sceneData.batteryLevel = saveData.batteryLevel;
            sceneData.flashlightEnabled = saveData.flashlightEnabled;
            sceneData.inventoryItems = new List<Item>();
            foreach(string s in saveData.items)
            {
                try
                {
                    //Load prefab from resources
                    string path = "Prefabs/Items/" + s;
                    GameObject reference = Resources.Load(path) as GameObject;

                    if (reference == null)
                    {
                        Debug.LogError("Resources could not load prefab " + s + " given the path: " + path);
                        continue;
                    }

                    //create item object in scene and add it to inventoryItems in sceneData
                    GameObject gameObject = UnityEngine.Object.Instantiate(reference);
                    UnityEngine.Object.DontDestroyOnLoad(gameObject);
                    switch (s)
                    {
                        case "flashlight":
                            sceneData.inventoryItems.Add(gameObject.GetComponent<FlashlightItem>());
                            break;
                        case "batteries":
                            sceneData.inventoryItems.Add(gameObject.GetComponent<BatteryItem>());
                            break;
                        case "medkit":
                            sceneData.inventoryItems.Add(gameObject.GetComponent<MedkitItem>());
                            break;
                        case "staminaShot":
                            sceneData.inventoryItems.Add(gameObject.GetComponent<StaminaShotItem>());
                            break;
                        case "key":
                            sceneData.inventoryItems.Add(gameObject.GetComponent<KeyItem>());
                            break;
                        case "labCard":
                            sceneData.inventoryItems.Add(gameObject.GetComponent<SecurityCardItem>());
                            break;
                        default:
                            Debug.LogError("wrong item name while loading from save");
                            break;
                    }
                    gameObject.SetActive(false);
                }
                catch(Exception e)
                {
                    Debug.LogError("Failed to load item " + s + ", error: " + e.Message);
                }
            }
        }
        else
        {
            return null;
        }

        return sceneData;
    }
}
