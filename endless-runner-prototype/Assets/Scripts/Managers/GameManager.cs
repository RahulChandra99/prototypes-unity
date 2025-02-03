using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject pauseUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

    }

    

    private void Start()
    {
        coinScore = 0;
        Distance = 0;
        pauseUI.SetActive(false);
    }

    public int coinScore = 0;
    public int playerHealth = 3;
    public int Distance = 0;


    public void PauseMenu()
    {
        if (pauseUI.activeSelf)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }

        else
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Sound()
    {
        SoundManager.Instance.SoundToggle();
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.isStarted = true;
        coinScore = 0;
        playerHealth = 3;
        Distance = 0;
        SceneManager.LoadSceneAsync("EndlessRunnerScene");
        
    }
}

