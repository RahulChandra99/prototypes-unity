using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordOrderManager : MonoBehaviour
{
    
    public TextMeshProUGUI inputOrderText;
    public GameObject[] options;
    public int currentQuestion;
    public GameObject enterBtn;
    public int questionNumber;
    
    
    [Header("Timer Variables")]
    public float maxTime;
    public Slider slider;
    public float timer;
    public Gradient timerGradient;
    public Image timerFillArea;
    [HideInInspector]public bool isTimerOver;
 
    [Header("Scoring System")]
    [SerializeField]float AttemptTime;
    public int ComboCount;
    public bool timerEnabled;
    public GameObject FText;
    public GameObject SText;
    public GameObject TText;
    [HideInInspector]public TextMeshProUGUI playerTotalScore;
    public GameObject comboBar;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreText;
  
    [HideInInspector] public GameObject Player;
    
    public CinemachineVirtualCamera cmvc;
    public Sprite yellowS, whiteS, greenS, redS;
    private void OnEnable()
    {
        isTimerOver = false;
    
        questionNumber = 0;
       
        ResetTimer();
        
        findPlayer();
        Initialization();
        timerEnabled = true;
    
    }
    
    
    void Initialization()
    {
        ComboCount = 0;
        comboBar.SetActive(false);
        FText.gameObject.SetActive(false);
        SText.gameObject.SetActive(false);
        TText.gameObject.SetActive(false);
        
    
        //TriviaQB.instance.changeRound += NextRoundPrep;
        //TriviaQB.instance.ResetQCounter();
   
    }
    
    void FixedUpdate()
    {
        if (!isTimerOver)
        {
            StartTimer();
        }
    }
    
    void Awake()
    {
        //TriviaQB.instance.initQuestionWordOrder();
        inputOrderText.text = "";
        currentQuestion = 0;
        SetOptions();
        
    }

    void Start()
    {
        //Camera Follow
        cmvc.Follow = Player.transform.GetChild(0);
        cmvc.LookAt = Player.transform.GetChild(0);
        
        
    }
    
    void findPlayer()
    {
        var a = FindObjectsOfType<PlayerControls_2>();
        foreach (var x in a)
        {
            if (!x.isAI)
            {
                Player = x.gameObject;
                x.playerdone += SetOptions; //subscribe
                // Debug.Log("subscribe");
            }
        }
    }
    
    void SetOptions()
    {
        if (questionNumber >= TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
            .wordOrderQs.Count)
        {
            questionNumber = 0;
        }
        ResetAnswer();
        enterBtn.SetActive(false);
        ResetTimer();
        TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
            .wordOrderQs[FindObjectOfType<WordOrderManager>().questionNumber].Options.Shuffle(4);
        if (questionNumber > 7)
        {
            questionNumber = 0;
        }
        var a = TriviaQB.instance.getWOoptions(GM_Climbing.Instance.levelDifficulty, questionNumber);
        
        for (int i = 0; i < a.Count; i++)
        {
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = a[i];

        }
        
        Invoke("SetScoreAnimationOff",2f);
    }
    
    
    void SetScoreAnimationOff()
    {
        FText.gameObject.SetActive(false);
        SText.gameObject.SetActive(false);
        TText.gameObject.SetActive(false);

    }

    public int tempCounter = 4;
    public void SetInputOnBtnPress()
    {
        if (inputOrderText.text == " " || inputOrderText.text == "  "|| inputOrderText.text == "   "|| inputOrderText.text == "    "|| inputOrderText.text == "     "|| inputOrderText.text == "      "|| inputOrderText.text == "       "|| inputOrderText.text == "        ")
        {
            inputOrderText.text = "";
        }
        
        if (Regex.Replace(inputOrderText.text, @"\s", "").Contains(Regex.Replace(EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
            .GetComponent<TextMeshProUGUI>().text, @"\s", "")))
        {
            tempCounter++;
            inputOrderText.text= inputOrderText.text.Replace(" "+EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                .GetComponent<TextMeshProUGUI>().text, "");
            EventSystem.current.currentSelectedGameObject.transform.GetComponent<Image>().sprite = whiteS;
            
        }
        
        else
        {
            tempCounter--;
            inputOrderText.text += " " + EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                .GetComponent<TextMeshProUGUI>().text;
            EventSystem.current.currentSelectedGameObject.transform.GetComponent<Image>().sprite = yellowS;
        }

        if (tempCounter == 0)
        {
            Invoke("CheckAnswer",0.5f);
            
        }
    }


    public void ResetAnswer()
    {
        tempCounter = 4;
        inputOrderText.text = "";
        foreach (var x in options)
        {
            x.GetComponent<Image>().sprite = whiteS;
        }
    }
    

    public void CheckAnswer()
    {
        if (TriviaQB.instance.CheckWOAnswer(GM_Climbing.Instance.levelDifficulty, inputOrderText.text,questionNumber) == false) // make sure to change the MAtch index 
        {
            
            WrongAnswer();
           
        }
        else 
        {
            CorrectAnswer();
        }
    }

    void CorrectAnswer()
    {
        tempCounter = 4;
            for (int i = 0; i < options.Length; i++)
            {
                options[i].GetComponent<Image>().sprite = greenS;

            }
            
            GM_Climbing.Instance.rightQ[GM_Climbing.Instance.roundNumber-1]++;
            GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber-1]++;
            
            ComboCount++;
        if (ComboCount >= 5)
        {
            ComboCount = 5;
        }
        
        
        GM_Climbing.Instance.totalCorrect++;
            
        
        //oldPoints();
        newPoints();

        
    }

    void WrongAnswer()
    {
        Debug.Log("wrong");
        tempCounter = 4;
        enterBtn.SetActive(false);
        ComboCount = 0;
        OffCombo();
        var g = FindObjectsOfType<PlayerControls_2>();
        foreach (var x in g)
        {
            if (!x.isAI)
            {
                x.WrongAnswerFeedback();
            }
        }
        GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber-1]++;
        GM_Climbing.Instance.wrongQ[GM_Climbing.Instance.roundNumber-1]++;
        inputOrderText.text = "";
        for (int i = 0; i < options.Length; i++)
        {
            
            options[i].GetComponent<Button>().interactable = true;
            options[i].GetComponent<Image>().sprite = redS;
        }

        StartCoroutine("WaitandResetOptions");

    }

    IEnumerator WaitandResetOptions()
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < options.Length; i++)
        {
            
            options[i].GetComponent<Button>().interactable = true;
            options[i].GetComponent<Image>().sprite = whiteS;
        }
    }
    
    void OffCombo()
  {
    comboBar.SetActive(false);
  }
  
  
  public void NextRoundPrep()
  {
    GM_Climbing.Instance.ResultScreenUpdate();
    StartCoroutine(WaitAndRoundFinish());
        
  }

  IEnumerator WaitAndRoundFinish()
  {
        
    yield return new WaitForSeconds(0f);
   //if (Player)
    //{
     //  Player.GetComponent<PlayerControls_2>().Won();
    //}
   // else
   // {
      //findPlayer();
    //  Player.GetComponent<PlayerControls_2>().Won();
    //}

    if (GM_Climbing.Instance.roundNumber >= 3)
    {
      GM_Climbing.Instance.MatchFinish();
      
            
    }
    else
    {
      GM_Climbing.Instance.RoundFinish();
      GM_Climbing.Instance.roundNumber++;
    }
        
        
    //Result screen update
        
        
        
    GM_Climbing.Instance.countingToTotalPlayers = 0;
    if (GM_Climbing.Instance.totalPlayersInLobby == 27)
    {
      GM_Climbing.Instance.totalPlayersInLobby = 9;
    }
    else if (GM_Climbing.Instance.totalPlayersInLobby == 9)
    {
      GM_Climbing.Instance.totalPlayersInLobby = 3;
    }

    GM_Climbing.Instance.PuzzleController.SetActive(false);


  }
  
  void StartTimer()
  {
        
    timer += Time.fixedDeltaTime;
    float time = maxTime - timer;
    AttemptTime = time;
    if (time <= 0)
    {
      isTimerOver = true;
      ResetAnswer();
      inputOrderText.text = "";

      questionNumber++;
      SetOptions();

    }

    if (isTimerOver == false)
    {
      slider.value = time;
    }
        
    timerFillArea.color = timerGradient.Evaluate(slider.normalizedValue);

  }
  
  void ResetTimer()
  {
    //Timer ini
    slider.maxValue = maxTime;
    slider.value = maxTime;
    isTimerOver = false;
    timer = 0f;
  }
  
  void OnDisable()
  {
    if (Player)
    {
      Player.GetComponent<PlayerControls_2>().playerdone -= SetOptions; //subscribe
    }
    //TriviaQB.instance.changeRound -= NextRoundPrep;

    ResetTimer();
  }
    
  void oldPoints()
  {
      int points=0;
//        Debug.Log("Attempt Time:" + AttemptTime);
      if(AttemptTime >= 10.4f)
      {
          points = 100;
            
          FText.gameObject.SetActive(true);
            
      }else if (AttemptTime >=4.4f && AttemptTime < 10.4f)
      {
          points = 70;

          SText.gameObject.SetActive(true);
      }
      else if (AttemptTime >0 && AttemptTime<4.4f )
      {
          points = 40;

          TText.gameObject.SetActive(true);
      }else
      {
          points = 0;
      }
      //Debug.Log("points got based on attempt: "+points );

      switch (ComboCount)
      {
          case 2:
              points *= ComboCount;
              break;
          case 3:
              points *= ComboCount;
              break;
          case 4:
              points *= ComboCount;
              break;
          case 5:
              points *= ComboCount;
              break;
            
      }
      Debug.Log("With Combo Total is now : " + points);
        
      if (GM_Climbing.Instance.roundNumber == 1)
      {
            
            
          scoreText = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
          GM_Climbing.Instance.r1C += points;
          scoreText.text = GM_Climbing.Instance.r1C.ToString();
      }
      if (GM_Climbing.Instance.roundNumber == 2)
      {
            
            
          scoreText = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
          GM_Climbing.Instance.r2C += points;
          scoreText.text = GM_Climbing.Instance.r2C.ToString();
      }
      if (GM_Climbing.Instance.roundNumber == 3)
      {
            
            
          scoreText = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
          GM_Climbing.Instance.r3C += points;
          scoreText.text = GM_Climbing.Instance.r3C.ToString();
      }
        
      var a = FindObjectsOfType<PlayerControls_2>();
      foreach (var x in a)
      {
          if (!x.isAI)
          {
              x.CorrectAnswer(points);
          }
      }
        
      if (comboBar)
      {
          comboText.text = ComboCount.ToString() + "X";
            
          if (ComboCount > 1)
          {
              comboBar.SetActive(true);
              Invoke("OffCombo",2.5f);
          }
          else
          {
              comboBar.SetActive(false);
          }
          
      }

      isTimerOver = true;
            
      questionNumber++;
      currentQuestion++;
  }

  void newPoints()
  {
      int points=0;
//        Debug.Log("Attempt Time:" + AttemptTime);
      if(AttemptTime >= 10.4f)
      {
          points = 50;
            
          FText.gameObject.SetActive(true);
            
      }else if (AttemptTime >=4.4f && AttemptTime < 10.4f)
      {
          points = 30;

          SText.gameObject.SetActive(true);
      }
      else if (AttemptTime >0 && AttemptTime<4.4f )
      {
          points = 10;

          TText.gameObject.SetActive(true);
      }else
      {
          points = 0;
      }
      //Debug.Log("points got based on attempt: "+points );

      switch (ComboCount)
      {
          case 2:
              points *= ComboCount;
              break;
          case 3:
              points *= ComboCount;
              break;
          case 4:
              points *= ComboCount;
              break;
          case 5:
              points *= ComboCount;
              break;
            
      }
      Debug.Log("With Combo Total is now : " + points);
        
      if (GM_Climbing.Instance.roundNumber == 1)
      {
            
            
          scoreText = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
          GM_Climbing.Instance.r1C += points;
          scoreText.text = GM_Climbing.Instance.r1C.ToString();
      }
      if (GM_Climbing.Instance.roundNumber == 2)
      {
            
            
          scoreText = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
          GM_Climbing.Instance.r2C += points;
          scoreText.text = GM_Climbing.Instance.r2C.ToString();
      }
      if (GM_Climbing.Instance.roundNumber == 3)
      {
            
            
          scoreText = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
          GM_Climbing.Instance.r3C += points;
          scoreText.text = GM_Climbing.Instance.r3C.ToString();
      }
        
      var a = FindObjectsOfType<PlayerControls_2>();
      foreach (var x in a)
      {
          if (!x.isAI)
          {
              x.CorrectAnswer(points);
          }
      }
        
      if (comboBar)
      {
          comboText.text = ComboCount.ToString() + "X";
            
          if (ComboCount > 1)
          {
              comboBar.SetActive(true);
              Invoke("OffCombo",2.5f);
          }
          else
          {
              comboBar.SetActive(false);
          }
          
      }

      isTimerOver = true;
            
      questionNumber++;
      currentQuestion++;
  }
}
