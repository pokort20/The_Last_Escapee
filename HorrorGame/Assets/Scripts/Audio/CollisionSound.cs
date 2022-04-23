using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
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
            AudioManager.instance.playThudAudio(gameObject.transform.position);
        }
    }
}
