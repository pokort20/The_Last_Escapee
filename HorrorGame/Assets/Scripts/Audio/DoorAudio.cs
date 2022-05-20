/// Door audio class
/**
    This class handles the door squaking audio. When a
    door is moved, it produces a squaking sound based 
    on its angular speed.
*/
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    //Public variables defined in Unity inspector
    public AudioSource audioSource;
    public GameObject door;

    //Other variables
    private Rigidbody rb;
    private float volumeDecay;
    private bool isSqueaking;
    private float volumeLevel;

    //Init
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

    //Update
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
}
