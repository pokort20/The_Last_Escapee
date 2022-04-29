using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string itemInfo;
    public Sprite icon;
    public bool isUseable = true;

    public virtual void UseItem()
    {
        float timeScale = Time.timeScale;
        GameManager.instance.setTimeScale(1.0f);
        AudioManager.instance.playAudio("menu_click");
        GameManager.instance.setTimeScale(timeScale);
        if(Tutorial.instance != null)
        {
            Tutorial.instance.hideAllHelps();
        }
        
    }
}
