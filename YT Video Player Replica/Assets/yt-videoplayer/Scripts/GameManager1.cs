using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
   public void startGame()
    {
        SceneManager.LoadSceneAsync("video-player");
    }
}
