using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4Finish : MonoBehaviour
{
    public AudioSource finishSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Level4Manager.scoreValue == 5)
        {
            finishSound.Play();

            GameObject.Find("MainChar").SendMessageUpwards("Finish");

            collision.gameObject.SetActive(false);


            StartCoroutine(LoadLevel5());


        }
    }

    IEnumerator LoadLevel5()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level5");

    }


}
