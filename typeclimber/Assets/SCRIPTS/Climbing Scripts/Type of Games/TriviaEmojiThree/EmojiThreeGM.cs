using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmojiThreeGM : MonoBehaviour
{
    public GameObject threeColumn;
    public GameObject twoColumn;
    public Image[] row1Images3;
    public Image[] row2Images3;
    public Image[] row3Images3;
    
    public Image[] row1Images2;
    public Image[] row2Images2;

    public Image[] inputImagesColumns3;
    public Image[] inputImagesColumns2;

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
    public int score;
    
    [HideInInspector] public GameObject Player;
    
    public CinemachineVirtualCamera cmvc;
    
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
    
        //QBTrivia.instance.changeRound += NextRoundPrep;
        //QBTrivia.instance.ResetQCounter();
   
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

        GenerateQuestion();
        
    }
    
    void FixedUpdate()
    {
        if (!isTimerOver)
        {
            StartTimer();
           
        }
    }
    void SetScoreAnimationOff()
    {
        FText.gameObject.SetActive(false);
        SText.gameObject.SetActive(false);
        TText.gameObject.SetActive(false);

    }


    public void GenerateQuestion()
    {
        ResetTimer();
        if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,GM_Climbing.Instance.roundNumber - 1,questionNumber) == 2)
        {
            twoColumn.SetActive(true);
            threeColumn.SetActive(false);
        }
        else if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,GM_Climbing.Instance.roundNumber - 1,questionNumber) == 3)
        {
            twoColumn.SetActive(false);
            threeColumn.SetActive(true);
        }
        
        
        if (twoColumn.activeInHierarchy)
        {

            for (int i = 0; i < 3; i++)
            {
                row1Images2[i].sprite = QBTrivia.instance.getEmoji3SpriteRow1(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row2Images2[i].sprite = QBTrivia.instance.getEmoji3SpriteRow2(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }

            for (int i = 0; i < 3; i++)
            {
                inputImagesColumns2[i].sprite = QBTrivia.instance.getEmoji3SpriteInputImages(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }
        }
        
        if (threeColumn.activeInHierarchy)
        {
            for (int i = 0; i < 3; i++)
            {
                row1Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow1(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row2Images3[i].sprite =QBTrivia.instance.getEmoji3SpriteRow2(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row3Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow3(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }
            
            for (int i = 0; i < 4; i++)
            {
                inputImagesColumns3[i].sprite = QBTrivia.instance.getEmoji3SpriteInputImages(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }
        }
    }

    public int tempCounter;
    public bool[] isAnswerCorrect;
    
    public void EmojiPressed()
    {
        if (threeColumn.activeInHierarchy)
        {
            tempCounter++;
            
            
            if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == QBTrivia.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji3[questionNumber].correctAnswer[0].name)
            {
                isAnswerCorrect[0] = true;
                inputImagesColumns3[0].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                    .GetComponent<Image>().sprite;
            }
            else if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == QBTrivia.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji3[questionNumber].correctAnswer[1].name)
            {
                isAnswerCorrect[1] = true;
                inputImagesColumns3[1].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                    .GetComponent<Image>().sprite;
            }
            
            else if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == QBTrivia.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji3[questionNumber].correctAnswer[2].name)
            {
                isAnswerCorrect[2] = true;
                inputImagesColumns3[2].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                    .GetComponent<Image>().sprite;
            }
            else
            {
                if (inputImagesColumns3[0].sprite.name == "button_4")
                {
                    inputImagesColumns3[0].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                        .GetComponent<Image>().sprite;
                    
                }
                else if(inputImagesColumns3[1].sprite.name == "button_4")
                {
                    inputImagesColumns3[1].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                        .GetComponent<Image>().sprite;
                    
                }
                else if(inputImagesColumns3[2].sprite.name == "button_4")
                {
                    inputImagesColumns3[2].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                        .GetComponent<Image>().sprite;
                    
                }
                
            }
            
            if (tempCounter == 3)
            {
                Invoke("CheckAnswer",0.5f);
            }
        }
        
         if (twoColumn.activeInHierarchy)
        {
            tempCounter++;
            
            
            if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == QBTrivia.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji3[questionNumber].correctAnswer[0].name)
            {
                isAnswerCorrect[0] = true; 
                inputImagesColumns2[0].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                    .GetComponent<Image>().sprite;
            }
            else if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == QBTrivia.instance.matches[GM_Climbing.Instance.MatchNumber].Rounds[GM_Climbing.Instance.roundNumber-1].triviaEmoji3[questionNumber].correctAnswer[1].name)
            {
                isAnswerCorrect[1] = true;
                inputImagesColumns2[1].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                    .GetComponent<Image>().sprite;
            }
            
            else
            {
                if (inputImagesColumns2[0].sprite.name == "button_4")
                {
                    inputImagesColumns2[0].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                        .GetComponent<Image>().sprite;
                    
                }
                else if(inputImagesColumns2[1].sprite.name == "button_4")
                {
                    inputImagesColumns2[1].sprite = EventSystem.current.currentSelectedGameObject.transform.GetChild(0)
                        .GetComponent<Image>().sprite;
                    
                }
               
                
            }
            
            if (tempCounter == 2)
            {
                Invoke("CheckAnswer",0.5f);
            }
        }
    }
    
    void Update()
    {
        if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,GM_Climbing.Instance.roundNumber - 1,questionNumber) == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (row1Images2[i].transform.parent.GetComponent<Toggle>().isOn)
                {
                    row1Images2[i].transform.parent.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    row1Images2[i].transform.parent.GetComponent<Image>().color = Color.white;
                }
                if (row2Images2[i].transform.parent.GetComponent<Toggle>().isOn)
                {
                    row2Images2[i].transform.parent.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    row2Images2[i].transform.parent.GetComponent<Image>().color = Color.white;
                }
            } 
        }
        if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,GM_Climbing.Instance.roundNumber - 1,questionNumber) == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (row1Images3[i].transform.parent.GetComponent<Toggle>().isOn)
                {
                    row1Images3[i].transform.parent.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    row1Images3[i].transform.parent.GetComponent<Image>().color = Color.white;
                }
                if (row2Images3[i].transform.parent.GetComponent<Toggle>().isOn)
                {
                    row2Images3[i].transform.parent.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    row2Images3[i].transform.parent.GetComponent<Image>().color = Color.white;
                }
                if (row3Images3[i].transform.parent.GetComponent<Toggle>().isOn)
                {
                    row3Images3[i].transform.parent.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    row3Images3[i].transform.parent.GetComponent<Image>().color = Color.white;
                }
            } 
        }
        
    }
    void CheckAnswer()
    {
        if (twoColumn.activeInHierarchy)
        {
            if (isAnswerCorrect[0] && isAnswerCorrect[1])
            {
                OnCorrect();
            }
            else
            {
                OnWrong();
            } 
        }

        if (threeColumn.activeInHierarchy)
        {
            if (isAnswerCorrect[0] && isAnswerCorrect[1] && isAnswerCorrect[2])
            {
                OnCorrect();
            }
            else
            {
                OnWrong();
            } 
        }
       
            
    }
    public Sprite button;

    void OnCorrect()
    {
        Debug.Log("Climb Up");
        
        if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,GM_Climbing.Instance.roundNumber - 1,questionNumber) == 2)
        {
            twoColumn.SetActive(true);
            threeColumn.SetActive(false);
        }
        else if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,GM_Climbing.Instance.roundNumber - 1,questionNumber) == 3)
        {
            twoColumn.SetActive(false);
            threeColumn.SetActive(true);
        }
        
        
        if (twoColumn.activeInHierarchy)
        {

            for (int i = 0; i < 3; i++)
            {
                row1Images2[i].sprite = QBTrivia.instance.getEmoji3SpriteRow1(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row2Images2[i].sprite = QBTrivia.instance.getEmoji3SpriteRow2(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }

            for (int i = 0; i < 3; i++)
            {
                inputImagesColumns2[i].sprite = QBTrivia.instance.getEmoji3SpriteInputImages(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }
            
            for (int i = 0; i < 2; i++)
            {
                isAnswerCorrect[i] = false;
            }
        }
        
        if (threeColumn.activeInHierarchy)
        {
            for (int i = 0; i < 3; i++)
            {
                row1Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow1(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row2Images3[i].sprite =QBTrivia.instance.getEmoji3SpriteRow2(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row3Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow3(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }
            
            for (int i = 0; i < 3; i++)
            {
                inputImagesColumns3[i].sprite = QBTrivia.instance.getEmoji3SpriteInputImages(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                inputImagesColumns3[i].sprite = QBTrivia.instance.getEmoji3SpriteInputImages(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            isAnswerCorrect[i] = false;
        }
        GM_Climbing.Instance.rightQ[GM_Climbing.Instance.roundNumber-1]++;
            GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber-1]++;
            ComboCount++;
            if (ComboCount >= 5)
            {
                ComboCount = 5;
            }
       
        
            GM_Climbing.Instance.totalCorrect++;
            
        
            int points=0;
            Debug.Log("Attempt Time:" + AttemptTime);
            if(AttemptTime >= 17)
            {
                points = 100;
            
                FText.gameObject.SetActive(true);
            
            }else if (AttemptTime >=7 && AttemptTime < 17)
            {
                points = 70;

                SText.gameObject.SetActive(true);
            }
            else if (AttemptTime >0 && AttemptTime<7 )
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
            Invoke("SetScoreAnimationOff",1.3f);
            questionNumber++;

        tempCounter = 0;
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

        if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber, GM_Climbing.Instance.roundNumber - 1,
            questionNumber) == 2)
        {
            twoColumn.SetActive(true);
            threeColumn.SetActive(false);
        }
        else if (QBTrivia.instance.getEmoji3Index(GM_Climbing.Instance.MatchNumber,
            GM_Climbing.Instance.roundNumber - 1, questionNumber) == 3)
        {
            twoColumn.SetActive(false);
            threeColumn.SetActive(true);
        }


        if (twoColumn.activeInHierarchy)
        {

            for (int i = 0; i < 3; i++)
            {
                row1Images2[i].sprite = QBTrivia.instance.getEmoji3SpriteRow1(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row2Images2[i].sprite = QBTrivia.instance.getEmoji3SpriteRow2(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);

                row1Images2[i].transform.parent.GetComponent<Toggle>().isOn = false;
                row2Images2[i].transform.parent.GetComponent<Toggle>().isOn = false;



            }

            inputImagesColumns2[0].sprite = button;
            inputImagesColumns2[1].sprite = button;

            tempCounter = 0;
            for (int i = 0; i < 2; i++)
            {
                isAnswerCorrect[i] = false;
            }
        }

        if (threeColumn.activeInHierarchy)
        {

            for (int i = 0; i < 3; i++)
            {
                row1Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow1(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row2Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow2(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);
                row3Images3[i].sprite = QBTrivia.instance.getEmoji3SpriteRow3(GM_Climbing.Instance.MatchNumber,
                    GM_Climbing.Instance.roundNumber - 1, questionNumber, i);

                row1Images3[i].transform.parent.GetComponent<Toggle>().isOn = false;
                row2Images3[i].transform.parent.GetComponent<Toggle>().isOn = false;
                row3Images3[i].transform.parent.GetComponent<Toggle>().isOn = false;
            }

            inputImagesColumns3[0].sprite = button;
            inputImagesColumns3[1].sprite = button;
            inputImagesColumns3[2].sprite = button;

            tempCounter = 0;
            for (int i = 0; i < 3; i++)
            {
                isAnswerCorrect[i] = false;
            }
        }
    }

    void OffCombo()
        {
            comboBar.SetActive(false);
        }

        public void UnselectObject()
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>().isOn)
            {
                tempCounter--;
                EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>().isOn = false;
            }
        
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
     

                ResetTimer();

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
            //QBTrivia.instance.changeRound -= NextRoundPrep;

            ResetTimer();
        }
    }
