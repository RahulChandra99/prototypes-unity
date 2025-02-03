using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Finish : MonoBehaviour
{
    public AudioSource finishSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Level3Manager.scoreValue == 5)
        {
            finishSound.Play();

            GameObject.Find("MainChar").SendMessageUpwards("Finish");

            collision.gameObject.SetActive(false);


            StartCoroutine(LoadLevel4());


        }
    }

    IEnumerator LoadLevel4()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level4");

    }


}
