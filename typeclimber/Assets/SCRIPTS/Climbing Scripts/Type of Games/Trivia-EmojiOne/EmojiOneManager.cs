using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmojiOneManager : MonoBehaviour
{
    public List<GameObject> topinputBoxes;
    public List<GameObject> bottomletterBoxes;
    public int questionNumber;
    public Image myEmoji1;
    public Image myEmoji2;
    public GameObject enterBtn;
    public bool isAllCorrect;

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
    public int score;
    
    [HideInInspector] public GameObject Player;
    
    public CinemachineVirtualCamera cmvc;

    private int tempMissingletter;

    public GameObject topLetterBox;
    public GameObject topBoxParent;

    public Sprite blueS, greenS, redS;
    private void OnEnable()
    {
        isTimerOver = false;
    
        questionNumber = 0;
       
        ResetTimer();
        
        findPlayer();
        Initialization();
        timerEnabled = true;
    
        //Ini Score = 0
        score = 0;
        
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
        questionNumber = 0;

        GenerateQuestion();
    }
    void FixedUpdate()
    {
        if (!isTimerOver)
        {
            StartTimer();
            //EnterBtnActive();
            if ( TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                .emojiLetterQs[questionNumber].numberOfMissingLetters == 0)
            {
                Invoke("CheckAnswer",0f);
            }

            if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                .emojiLetterQs[questionNumber].numberOfMissingLetters != 0)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    {
                        temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
                    }  
                } 
            }
           
            
        }
    }
    void SetScoreAnimationOff()
    {
        FText.gameObject.SetActive(false);
        SText.gameObject.SetActive(false);
        TText.gameObject.SetActive(false);

    }

    void Awake()
    {
        //TriviaQB.instance.initQuestionTrivia1();
        questionNumber = 0;
       
    }

    public GameObject[] temp;

    public void GenerateQuestion()
    {
        
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }
        if (questionNumber >= TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
            .emojiLetterQs.Count)
        {
            questionNumber = 0;
        }
        for (int i = 0; i < temp.Length; i++)
        {
            Destroy(temp[i]);
        }
        Array.Clear(temp,0,temp.Length);
        enterBtn.SetActive(false);
        ResetTimer();
        tempMissingletter = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].numberOfMissingLetters;
        myEmoji1.sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].emos[0];
        myEmoji2.sprite=  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].emos[1];

        
        temp = new GameObject[TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].topLetterInputBoxes.Length];
        for (int i = 0; i < TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].topLetterInputBoxes.Length; i++)
        {
           temp[i]=  Instantiate(topLetterBox, topBoxParent.transform);
           temp[i].GetComponent<Button>().onClick.AddListener(ResetLetterBox);
           
           temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
               TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                   .emojiLetterQs[questionNumber].topLetterInputBoxes[i];
            
           if (temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "")
           {
               temp[i].GetComponent<Button>().interactable = false;
               temp[i].GetComponent<Image>().color = Color.grey;
           }
        }
        
        bottomletterBoxes.Shuffle(bottomletterBoxes.Count);
        for (int i = 0; i <  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].bottomLetterBoxes.Length; i++)
        {
            bottomletterBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                    .emojiLetterQs[questionNumber].bottomLetterBoxes[i];
        }
        foreach (var x in bottomletterBoxes)
        {
            x.GetComponent<Button>().interactable = true;
        }

    }

    
    public void CheckLetter()
    {
        
      
        for (int i = 0; i < temp.Length; i++)
        {
            
            
            if (temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == "")
            {
               
                
                temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = EventSystem.current.currentSelectedGameObject
                    .transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToUpper();
                
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
                TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                    .emojiLetterQs[questionNumber].numberOfMissingLetters--;
                 return;
            }
        }
       
       
        //EnterBtnActive();

    }

    
    public void ResetLetterBox()
    {
        TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].numberOfMissingLetters++;
         check = false;//to reset wrong answer multiple calls
        
        foreach (var y in temp)
        {
            if (y.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == EventSystem.current
                .currentSelectedGameObject
                .transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
            {
                EventSystem.current
                    .currentSelectedGameObject.GetComponent<Button>().interactable = true;

                for (int i = 0; i < bottomletterBoxes.Count; i++)
                {
                    if (bottomletterBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == EventSystem
                        .current
                        .currentSelectedGameObject
                        .transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
                    {
                        bottomletterBoxes[i].GetComponent<Button>().interactable = true;
                        //break;
                    }
                }
            }
        }
        
        EventSystem.current.currentSelectedGameObject
            .transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        
    }
    
    
    void EnterBtnActive()
    {
        if ( TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].numberOfMissingLetters == 0)
        {
            //Invoke("Climb_NextQ",2f);
            enterBtn.SetActive(true);
        }
        else
        {
            enterBtn.SetActive(false);
        } 
    }

    private string myInput;
    private string correctInput;
      
    public void CheckAnswer()
    {
       Checking();
       
        if (isAllCorrect)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            }
            Invoke("CorrectAnswer",0f);
        }
        else 
        {
            //isTimerOver = true
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            WrongAnswer();
        }
    }

    void Checking()
    {
        for (int i = 0; i < temp.Length; i++)
        {
            if ( TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                .emojiLetterQs[questionNumber].CAnswer.Contains(temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text))
            {
                isAllCorrect = true;
            }
            else
            {
                isAllCorrect = false;
                return;
            }
        }
    }

    void CorrectAnswer()
    {
        foreach (var x in bottomletterBoxes)
        {
            x.GetComponent<Button>().interactable = false;
        }
        
        
        myEmoji1.sprite =  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].emos[0];
        myEmoji2.sprite=  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].emos[1];

        isAllCorrect = false;
        
      /*  for (int i = 0; i <  TriviaQB.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji1[questionNumber].topLetterInputBoxes.Length; i++)
        {
            temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                 TriviaQB.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji1[questionNumber].topLetterInputBoxes[i];
            
            if (temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "")
            {
                temp[i].GetComponent<Button>().interactable = false;
            }
        }*/
        
        for (int i = 0; i <  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
            .emojiLetterQs[questionNumber].bottomLetterBoxes.Length; i++)
        {
            bottomletterBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                    .emojiLetterQs[questionNumber].bottomLetterBoxes[i];
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
            Invoke("SetScoreAnimationOff",1.3f);
            questionNumber++;

          /*  
            */

        
    }

    private bool check;
    
    void WrongAnswer()
    {
        if (!check)
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
            check = true;
            //make buttons interactable after some fixed seconds
            foreach (var x in bottomletterBoxes)
            {
                x.GetComponent<Button>().interactable = false;
            }
            
            StartCoroutine("WaitandEnableWrongQues");
        }
        
    }

    IEnumerator WaitandEnableWrongQues()
    {
        yield return new WaitForSeconds(1f);
        
        foreach (var x in bottomletterBoxes)
        {
            x.GetComponent<Button>().interactable = true;
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
     

                questionNumber++;
                
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

        public void ResetQuestion()
        {
            TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                .emojiLetterQs[questionNumber].numberOfMissingLetters = tempMissingletter;
            
            isAllCorrect = false;
        
            for (int i = 0; i <  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                .emojiLetterQs[questionNumber].topLetterInputBoxes.Length; i++)
            {
                temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                        .emojiLetterQs[questionNumber].topLetterInputBoxes[i];
            
                if (temp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "")
                {
                    temp[i].GetComponent<Button>().interactable = false;
                }
            }
        
            for (int i = 0; i <  TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                .emojiLetterQs[questionNumber].bottomLetterBoxes.Length; i++)
            {
                bottomletterBoxes[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                        .emojiLetterQs[questionNumber].bottomLetterBoxes[i];
            }

            foreach (var x in bottomletterBoxes)
            {
                x.GetComponent<Button>().interactable = true;
            }
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
        }
    }
