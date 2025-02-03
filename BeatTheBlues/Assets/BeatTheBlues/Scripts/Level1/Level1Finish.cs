using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Finish : MonoBehaviour
{
    public AudioSource finishSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Level1Manager.scoreValue == 2)
        {
            finishSound.Play();

            GameObject.Find("MainChar").SendMessageUpwards("Finish");

            collision.gameObject.SetActive(false);

            StartCoroutine(LoadLevel2());
               
                        
        }

        
    }

    IEnumerator LoadLevel2()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level2");

    }

    
}
