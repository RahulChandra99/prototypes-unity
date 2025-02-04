using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Material[] skyboxes;
    public GameObject[] ballType;
    public GameObject activatedBall;
    public int activatedBallNumber;
    public int activatedSkyNumber;

    public AudioSource errorAS;
    public AudioSource ballbounce_AS;
    public AudioSource platformBreaking_AS;

    public AudioClip[] ballBounceSFX;

    public int totalStreak;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
    }

    private UIManager _uiManager;
    public bool isGamePaused;
    public CameraController _CameraController;

    public bool isGameStarted;
    public bool isGameOver;

    public int highestStreak;
    
    private void Start()
    {
        GameIni();
        
        Application.targetFrameRate = 75;

        

    }

    private void GameIni()
    {
        _uiManager = UIManager.Instance;
        
        _uiManager.pause_panel.SetActive(false);

        foreach (var ballType in ballType)
        {
            ballType.SetActive(false);
        }
        
        
        RenderSettings.skybox = skyboxes[PlayerPrefs.GetInt("SkyNumber")];
        
        activatedBall = ballType[PlayerPrefs.GetInt("BallNumber")];
        activatedBall.SetActive(true);
        
        _uiManager.customDoTweenAnimation(_uiManager.streak_GO.transform,-3000,0f,Ease.InOutCubic);
        _uiManager.customDoTweenAnimation(_uiManager.restart_GO.transform,3000,0f,Ease.InOutCubic);
        
        highestStreak = PlayerPrefs.GetInt("HighScore");
        _uiManager.highestStreak_GO.GetComponent<TextMeshProUGUI>().text = "Highest Streak : " + highestStreak.ToString();


        if (_CameraController != null)
            _CameraController.ball = activatedBall.transform;
    }

    public void ResetLevelBtn()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void PauseBtn()
    {
        _uiManager.OpenPanel(_uiManager.pause_panel);
        //Time.timeScale = 0;
        isGamePaused = true;
    }

    public void ResumeBtn()
    {
        Time.timeScale = 1;
        _uiManager.ClosePanel(_uiManager.pause_panel);
        isGamePaused = false;

    }

    public void GameStarted()
    {
        _uiManager.customDoTweenAnimation(_uiManager.TopHalf_GO.transform,-3000,0.5f,Ease.InOutCubic);
        _uiManager.customDoTweenAnimation(_uiManager.BottomHalf_GO.transform,3000,0.5f,Ease.InOutCubic);
        
        _uiManager.customDoTweenAnimation(_uiManager.streak_GO.transform,3000,0.5f,Ease.InOutCubic);

    }

    public void GameOver()
    {
        Error();
        
        //UIManager.Instance.customDoTweenAnimation(_uiManager.TopHalf_GO.transform,3000,0.5f,Ease.InOutCubic);
        //UIManager.Instance.customDoTweenAnimation(_uiManager.BottomHalf_GO.transform,-3000,0.5f,Ease.InOutCubic);
        UIManager.Instance.customDoTweenAnimation(_uiManager.restart_GO.transform,-3000,0.5f,Ease.InOutCubic);
    }

    public void Error()
    {
        StartCoroutine(ErrorCoroutine());
    }

    IEnumerator ErrorCoroutine()
    {
        errorAS.Play();
        
        //red crash
        _uiManager.crashPanel_GO.GetComponent<Image>().DOFade(1f, 1f);
        
        //screen shake
        _CameraController.TriggerCameraShake(0.2f, 0.2f);
        
        TriggerVibration();
        yield return new WaitForSeconds(0.5f);
        
        //red screen
        _uiManager.crashPanel_GO.GetComponent<Image>().DOFade(0f, 2f);
        
        
    }

    private void Update()
    {
        _uiManager.streak_GO.GetComponent<TextMeshProUGUI>().text = "Streak : " + totalStreak.ToString();
        
        
    }

    public void SwitchEnvironment(int envNumber)
    {
        activatedSkyNumber = envNumber;
        RenderSettings.skybox = skyboxes[envNumber];
        DynamicGI.UpdateEnvironment(); // Update lighting to reflect skybox change
        
        PlayerPrefs.SetInt("SkyNumber",activatedSkyNumber);
    }

    public void SwitchBall(int ballNumber)
    {
        foreach (var typeOfBall in ballType)
        {
            typeOfBall.SetActive(false);
        }

        activatedBall = ballType[ballNumber];
        activatedBall.SetActive(true);
        activatedBallNumber = ballNumber;
        

        if (_CameraController != null)
            _CameraController.ball = activatedBall.transform;
        
        PlayerPrefs.SetInt("BallNumber",activatedBallNumber);
    }
    
    public void TriggerVibration(float duration = 0.5f)
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate(); // Trigger vibration
#endif
    }
}