/// Class that holds information about audio files
/**
    This class is used to encapsule variables used for audio clips.
    The audio manager stores a List<Audio>, where all audio files
    are stored.
 */
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
