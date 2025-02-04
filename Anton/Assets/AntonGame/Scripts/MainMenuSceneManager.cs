using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    public Animator exitMenu;

    
    public bool check = false;
    private void Start()
    {
        FindObjectOfType<AntonPlayerController>().enabled = false;
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
        //for healthpanelui
        check = true;
        

    }

    public void Options()
    {
        //play menu off animation
    }

    public void Quit_()
    {
        Application.Quit();
    }

    private void Update()
    {
        

        

    }
}
