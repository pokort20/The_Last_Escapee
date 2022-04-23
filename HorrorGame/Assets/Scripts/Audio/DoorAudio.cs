using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject door;

    private Rigidbody rb;
    private float volumeDecay;
    private bool isSqueaking;
    private float volumeLevel;
    // Start is called before the first frame update
    void Start()
    {
        if(audioSource == null)
        {
            Debug.LogWarning("Door" + gameObject.name + " has no audio source!");
        }
        if(door == null)
        {
            Debug.LogWarning("Door object not assigned!");
        }
        else
        {
            rb = door.GetComponent<Rigidbody>();
        }
        volumeDecay = 0.0f;
        isSqueaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.angularVelocity.magnitude < 0.5)
        {
            audioSource.Pause();
            return;
        }

        if(!audioSource.isPlaying)
        {
            AudioManager.instance.playAudio("door_squeak" + Random.Range(1, 4).ToString(), audioSource);
            volumeLevel = audioSource.volume;
            audioSource.volume = volumeDecay;
        }
        if (!isSqueaking && rb.angularVelocity.magnitude > 1.0f)
        {
            isSqueaking = true;
            volumeDecay = 0.9f;
        }
        if (isSqueaking)
        {
            audioSource.volume = volumeLevel * volumeDecay;
            volumeDecay -= 1.6f * Time.deltaTime;
            if (volumeDecay <= 0.0f)
            {
                isSqueaking = false;
                volumeDecay = 0.0f;
            }
        }
    }
    private float remap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float val;
        val = Mathf.InverseLerp(iMin, iMax, value);
        return Mathf.Lerp(oMin, oMax, val);
    }
}
