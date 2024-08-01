using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Login_Register");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
