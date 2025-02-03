using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using puzzleIO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriviaManager : MonoBehaviour
{
  
  public TextMeshProUGUI QuestionText;
  public GameObject[] Options;
  private bool isCorrect;
  public int currentQuestion;
  [SerializeField] private int counter;
  public TextMeshProUGUI scoreText;
  public int score;
  
  
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
  
  [HideInInspector] public GameObject Player;
  
  public int questionNo;

  public CinemachineVirtualCamera cmvc;
  public Sprite greenS, redS, whiteS;

  public int[] generic, food, world, mats, tf, phrase;
  
  private void OnEnable()
  {
    isTimerOver = false;
    findPlayer();
    questionNo = 0;
    GenerateQuestion();
    
    ResetTimer();
    
    //Ini Score = 0
    score = 0;

    Initialization();
    
    
  }
  
  void findPlayer()
  {
    var a = FindObjectsOfType<PlayerControls_2>();
    foreach (var x in a)
    {
      if (!x.isAI)
      {
        Player = x.gameObject;
        x.playerdone += GenerateQuestion; //subscribe
        // Debug.Log("subscribe");
                
      }
    }
  }
  
  void Start()
  {
    //Camera Follow
    cmvc.Follow = Player.transform.GetChild(0);
    cmvc.LookAt = Player.transform.GetChild(0);
       
  }

  void Awake()
  {
    //TriviaQB.instance.initQuestionMCQ();
  
  }

  List<int> doneConcepts = new List<int>();
  
  


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
  
  public void GenerateQuestion()
  {
    //InvokeRepeating("StartTimer", 0f, 1f);
    ResetTimer();
    if (TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
      .MCQCategories[TriviaQB.instance.categoryNumber].categoryName == "True/False")
    {
      GM_Climbing._instance.triviaOption3.SetActive(false);
      GM_Climbing._instance.triviaOption4.SetActive(false);
    }
    else
    {
      GM_Climbing._instance.triviaOption3.SetActive(true);
      GM_Climbing._instance.triviaOption4.SetActive(true);
    }
    SetAnswer();
    
    for (int i = 0; i < Options.Length; i++)
    {
      Options[i].GetComponent<Button>().interactable = true;
      Options[i].GetComponent<Image>().sprite = whiteS;
      Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.blue;
    }

    //Avoid repetation

    if (questionNo >= TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
      .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ.Count)
    {
      questionNo = 0;
    }
    
    QuestionText.text = TriviaQB.instance.getMCQQuestion(GM_Climbing.Instance.levelDifficulty,TriviaQB.instance.categoryNumber,questionNo);
    
    
    Invoke("SetScoreAnimationOff",0f);
    //questionNo++;
  }
  
  void SetScoreAnimationOff()
  {
    FText.gameObject.SetActive(false);
    SText.gameObject.SetActive(false);
    TText.gameObject.SetActive(false);

  }

  void SetAnswer()
  {
    if (questionNo >= TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
      .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ.Count)
    {
      questionNo = 0;
    }
    TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.MatchNumber]
      .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ[questionNo].Answers.Shuffle(4);
   var a = TriviaQB.instance.getMCQOptions(GM_Climbing.Instance.levelDifficulty,TriviaQB.instance.categoryNumber,questionNo);
    
    for (int i = 0; i < a.Count; i++)
    {
      isCorrect = false;
      Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = a[i];

    }
  }

  public void CheckAnswer()
  {
    
      if (TriviaQB.instance.CheckMCQAnswer(GM_Climbing.Instance.levelDifficulty,TriviaQB.instance.categoryNumber,EventSystem.current.currentSelectedGameObject
        .transform
        .GetChild(0).GetComponent<TextMeshProUGUI>().text,questionNo))
      {
        OnCorrect();
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = greenS;
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1,75,0);
        foreach (var x in Options)
        {
          if (TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.MatchNumber]
            .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ[questionNo].correctAns == x
            .transform
            .GetChild(0).GetComponent<TextMeshProUGUI>().text)
          {
            x.SetActive(true);
          }
          
        }
      }
      else
      {
        //EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(120,21,38);
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = redS;
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        //Display right answer 
        for (int i = 0; i < Options.Length; i++)
        {
          if (TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.MatchNumber]
            .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ[questionNo].correctAns == Options[i]
            .transform
            .GetChild(0).GetComponent<TextMeshProUGUI>().text)
          {
            Options[i].GetComponent<Image>().sprite = greenS;;
          }
          
        }
        //hide options
       /* foreach (var x in Options)
        {
          if (TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.MatchNumber]
            .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ[questionNo].correctAns == x
            .transform
            .GetChild(0).GetComponent<TextMeshProUGUI>().text)
          {
            x.SetActive(true);
          }

          else if (TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.MatchNumber]
            .MCQCategories[TriviaQB.instance.categoryNumber].categoryMCQ[questionNo].correctAns != x
            .transform
            .GetChild(0).GetComponent<TextMeshProUGUI>().text)
          {
            x.SetActive(false);
          }
        }*/
        OnWrong();
        
      }

  }

  
  void OnCorrect()
  {

    GM_Climbing.Instance.rightQ[GM_Climbing.Instance.roundNumber-1]++;
    GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber-1]++;
    ComboCount++;
        if (ComboCount >= 5)
        {
            ComboCount = 5;
        }
        
        
        //QuestionText.text = "......" ;
        for (int i = 0; i < Options.Length; i++)
        {
          //Options[i].GetComponentInChildren<TextMeshProUGUI>().text = ".....";
         
        }
        
        GM_Climbing.Instance.totalCorrect++;
            
        
       // oldPoints();
        newPoints();


        
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
        questionNo++;
  }

  void OnWrong()
  {
    
    Debug.Log("Wrong");
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
    questionNo++;
    Invoke("ResetQuestion",0.8f);
    isTimerOver = true;
    
  }
  
  void OffCombo()
  {
    comboBar.SetActive(false);
  }
  public void ResetQuestion()
  {
    
    for (int i = 0; i < Options.Length; i++)
    {
      
      Options[i].GetComponent<Button>().interactable = true;
      Options[i].GetComponent<Image>().sprite = whiteS;
      Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.blue;
    }
    
    //QuestionGeneration
    ResetTimer();
    counter = 0;
    GenerateQuestion();
    SetAnswer();
    
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
      
      questionNo++;
      GenerateQuestion();
      

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
      Player.GetComponent<PlayerControls_2>().playerdone -= GenerateQuestion; //subscribe
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
//        Debug.Log("With Combo Total is now : " + points);
        
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
//        Debug.Log("With Combo Total is now : " + points);
        
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
  }
  
}
