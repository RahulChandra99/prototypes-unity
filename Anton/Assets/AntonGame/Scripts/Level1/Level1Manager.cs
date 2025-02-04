using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public Animator exitMenu;
    private void Start()
    {
        FindObjectOfType<AntonPlayerController>().enabled = false;
        exitMenu.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        exitMenu.SetBool("goOut", true);
        //play menu off animation
        //play full dark panel animation
        //play full dark panel off animation
        StartCoroutine(StartIT());
    }

    IEnumerator StartIT()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<AntonPlayerController>().enabled = true;
        exitMenu.gameObject.SetActive(false);

    }

    public void Options()
    {
        //play menu off animation
    }

    public void Quit_()
    {
        Application.Quit();
    }
}
