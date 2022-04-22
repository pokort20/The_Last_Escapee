using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Audio
{

    public AudioClip audioClip;
    public string name;
    [Range(0.0f, 1.0f)]
    public float volume;
    [Range(0.0f, 5.0f)]
    public float pitch;
    public bool loop;

}
