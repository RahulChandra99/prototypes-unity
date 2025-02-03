using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChoose : MonoBehaviour
{
    public void lvl1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void lvl2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void lvl3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void lvl4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void lvl5()
    {
        SceneManager.LoadScene("Level5");
    }
}
