using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   public void GameScene(string level)
    {
        SceneManager.LoadSceneAsync(level);
       
        SoundManager.Instance.musicAudio.time = 0f;

        SoundManager.Instance.isStarted = true;
    }
}
