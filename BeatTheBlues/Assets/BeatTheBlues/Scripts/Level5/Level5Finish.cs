using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level5Finish : MonoBehaviour
{
    public AudioSource finishSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Level5Manager.scoreValue == 5)
        {
            finishSound.Play();

            GameObject.Find("MainChar").SendMessageUpwards("Finish");

            collision.gameObject.SetActive(false);

            
               
            
        }
    }

    
}
