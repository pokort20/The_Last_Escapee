/// Collision sound class
/**
    This class handles the collision sound of all 
    moveable objects in the scene.
*/
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    //Public variables defined in Unity inspector;
    public bool isHinge;

    //Other variables
    private float startTime;
    private float lastPlayed;

    //Init
    void Start()
    {
        startTime = Time.time;
        lastPlayed = Time.time;
    }

    //Functions
    private void OnCollisionEnter(Collision collision)
    {
        if(startTime + 2.0f > Time.time)
        {
            return;
        }
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if(isHinge)
            {
                if(lastPlayed + 1.0f < Time.time)
                {
                    AudioManager.instance.playAudio("thud_hinge");
                    lastPlayed = Time.time;
                }
            }
            else
            {
                AudioManager.instance.playThudAudio(gameObject.transform.position);
            }
        }
    }
}
