using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public float volume;
    public float brightness;

    public SettingsData(float _volume, float _brightness)
    {
        brightness = _brightness;
        volume = _volume;
    }
}
