using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    
    public void PlayBtn()
    {
        SceneManager.LoadScene("InstructionScreen");
    }

    public void OkBtn()
    {
        SceneManager.LoadSceneAsync("LevelChoose");
    }
}
