using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Finish : MonoBehaviour
{
    public AudioSource finishSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Level2Manager.scoreValue == 3)
        {
            finishSound.Play();

            GameObject.Find("MainChar").SendMessageUpwards("Finish");

            collision.gameObject.SetActive(false);


            StartCoroutine(LoadLevel3());


        }
    }

    IEnumerator LoadLevel3()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level3");

    }


}
