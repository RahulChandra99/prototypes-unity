using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    AntonPlayerController cc;
    void Start()
    {
        cc = GetComponent<AntonPlayerController>();
    }

    
    void Update()
    {
        if(cc.isGrounded == true && GetComponent<AudioSource>().isPlaying == false && (cc.horzMov == 1 || cc.horzMov == -1) && cc.enabled==true)
        {
            //GetComponent<AudioSource>().volume = Random.Range(0.5f, 0.6f);
            //GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.6f);
            GetComponent<AudioSource>().Play();
        }
    }
}
