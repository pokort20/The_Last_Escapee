using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public bool isHinge;

    private float startTime;
    private float lastPlayed;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        lastPlayed = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
