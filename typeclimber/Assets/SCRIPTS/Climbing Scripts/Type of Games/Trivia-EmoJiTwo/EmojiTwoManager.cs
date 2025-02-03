using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Climbing_Scripts.Type_of_Games
{
    public class EmojiTwoManager : MonoBehaviour
    {
        public GameObject threeRow;
        public GameObject twoRow;
        public Image[] row1Images3;
        public Image[] row2Images3;
        public Image[] row3Images3;
    
        public Image[] row1Images2;
        public Image[] row2Images2;

        public int questionNumber;
        public TextMeshProUGUI questionTxt;
    
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
        //public GameObject selectedS;
        public Sprite blueBG, greenBG;

        private void Awake()
        {
            GenerateQuestion();
        }

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
            if (questionNumber >= TriviaQB.instance.LevelOfDiff[GM_Climbing._instance.levelDifficulty]
                .wordEmojiQs.Count)
            {
                questionNumber = 0;
            }
            questionTxt.color = Color.white;
            TriviaQB.instance.shuffleImages(questionNumber);
            if (twoRow.activeInHierarchy)
            {
                foreach (var x in row1Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
                foreach (var x in row2Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
            }
            
            if (threeRow.activeInHierarchy)
            {
                foreach (var x in row1Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
                foreach (var x in row2Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
                foreach (var x in row3Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
            }
            
            tempCounter = 0;
            ResetTimer();
            if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].numberOfRows == 2)
            {
                var a = FindObjectsOfType<ColnManager>();
                foreach (var x in a)
                {
                    x.DisableAllBtns();
                }
            
            
                twoRow.SetActive(true);
                threeRow.SetActive(false);
            }
            else if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].numberOfRows == 3)
            {
                var a = FindObjectsOfType<ColnManager>();
                foreach (var x in a)
                {
                    x.DisableAllBtns();
                }
            
                twoRow.SetActive(false);
                threeRow.SetActive(true);
            }

            if (twoRow.activeInHierarchy)
            {
                questionTxt.text = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].questionText;

                for (int i = 0; i < 3; i++)
                {
                    row1Images2[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row1Emos[i];
                    row2Images2[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row2Emos[i];
                }
            }
        
            if (threeRow.activeInHierarchy)
            {
                questionTxt.text = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].questionText;

                for (int i = 0; i < 3; i++)
                {
                    row1Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row1Emos[i];
                    row2Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row2Emos[i];
                    row3Images3[i].sprite =TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row3Emos[i];
                }
            }
        
            isAnswerCorrect1 = false;
            isAnswerCorrect2 = false;
            isAnswerCorrect3 = false;
        }
    
        public bool isAnswerCorrect1;
        public bool isAnswerCorrect2;
        public bool isAnswerCorrect3;
        public int tempCounter;
    
        public void EmoJiPressed()
        {
            
            if (threeRow.activeInHierarchy)
            {
                tempCounter++;
                
            
                if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].correctAnsR1)
                {
                    isAnswerCorrect1 = true;
                }
                else if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].correctAnsR2)
                {
                    isAnswerCorrect2 = true;
                }
            
                else if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].correctAnsR3)
                {
                    isAnswerCorrect3 = true;
                }
            
            
                if (tempCounter == 3)
                {
                    CheckAnswer();
                }
            }
        
            if (twoRow.activeInHierarchy)
            {
                
                tempCounter++;
            
                if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].correctAnsR1)
                {
                    isAnswerCorrect1 = true;
                }
                else if(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>().sprite.name == TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].correctAnsR2)
                {
                    isAnswerCorrect2 = true;
                }
            
              
                if (tempCounter == 2)
                {

                    CheckAnswer();
                    
                }
            }
        
        }

       

        private bool check;
        void CheckAnswer()
        {
            
            if (!check)
            {
                Invoke("ResetCheckAnswer",1.5f);
                check = true;
                if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                    .wordEmojiQs[questionNumber].numberOfRows == 3)
                {
                    if (tempCounter == 3)
                    {
                        if (isAnswerCorrect1 && isAnswerCorrect2 && isAnswerCorrect3)
                        {
                            tempCounter = 0;
                            questionTxt.color = Color.green;
                            OnCorrect();
                        
                        }
                        else
                        {
                            questionTxt.color = Color.red;
                            OnWrong();
                            tempCounter = 0;
                        }
                    }
                
                }
        

                else if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty]
                    .wordEmojiQs[questionNumber].numberOfRows == 2)
                {
                
                    
                    if (isAnswerCorrect1 && isAnswerCorrect2 )
                    {
                        tempCounter = 0;
                        OnCorrect();
                        
                    }
                    else
                    {
                    
                        OnWrong();
                        
                    }  
                
                
                }

            
            }
            
        }

        void ResetCheckAnswer()
        {
            check = false;
        }

        void OnCorrect()
        {
            
            tempCounter = 0;
            if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].numberOfRows == 2)
            {
                twoRow.SetActive(true);
                threeRow.SetActive(false);
            }
            else if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].numberOfRows == 3)
            {
                twoRow.SetActive(false);
                threeRow.SetActive(true);
            }

            if (twoRow.activeInHierarchy)
            {
                questionTxt.text = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].questionText;

                for (int i = 0; i < 3; i++)
                {
                    row1Images2[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row1Emos[i];
                    row2Images2[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row2Emos[i];
                }
            }
        
            if (threeRow.activeInHierarchy)
            {
                questionTxt.text = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].questionText;

                for (int i = 0; i < 3; i++)
                {
                    row1Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row1Emos[i];
                    row2Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row2Emos[i];
                    row3Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row3Emos[i];
                }
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

           

            isTimerOver = true;
            Invoke("SetScoreAnimationOff",1.3f);
            questionNumber++;

            if (twoRow.activeInHierarchy)
            {
                foreach (var x in row1Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
                foreach (var x in row2Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            
            if (threeRow.activeInHierarchy)
            {
                foreach (var x in row1Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
                foreach (var x in row2Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
                foreach (var x in row3Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
        
        }

        void OnWrong()
        {

            isTimerOver = true;
            tempCounter = 0;
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
            if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].numberOfRows == 2)
            {
                twoRow.SetActive(true);
                threeRow.SetActive(false);
            }
            else if (TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].numberOfRows == 3)
            {
                twoRow.SetActive(false);
                threeRow.SetActive(true);
            }

            if (twoRow.activeInHierarchy)
            {
                questionTxt.text = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].questionText;

                for (int i = 0; i < 3; i++)
                {
                    row1Images2[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row1Emos[i];
                    row2Images2[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row2Emos[i];
                
                    var a = FindObjectsOfType<ColnManager>();
                    foreach (var x in a)
                    {
                        x.DisableAllBtns();
                    }
                
                }
            }
        
            if (threeRow.activeInHierarchy)
            {
                questionTxt.text = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].questionText;

                for (int i = 0; i < 3; i++)
                {
                    row1Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row1Emos[i];
                    row2Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row2Emos[i];
                    row3Images3[i].sprite = TriviaQB.instance.LevelOfDiff[GM_Climbing.Instance.levelDifficulty].wordEmojiQs[questionNumber].row3Emos[i];

                    var a = FindObjectsOfType<ColnManager>();
                    foreach (var x in a)
                    {
                        x.DisableAllBtns();
                    }
                }
            }

            
            isAnswerCorrect1 = false;
            isAnswerCorrect2 = false;
            isAnswerCorrect3 = false;
            
            GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber-1]++;
            GM_Climbing.Instance.wrongQ[GM_Climbing.Instance.roundNumber-1]++;
           
            if (twoRow.activeInHierarchy)
            {
                foreach (var x in row1Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
                foreach (var x in row2Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
            }
            
            if (threeRow.activeInHierarchy)
            {
                foreach (var x in row1Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
                foreach (var x in row2Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
                foreach (var x in row3Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = false;
                }
            }

            StartCoroutine("WaitandResetWrongQuestion");
        }

        IEnumerator WaitandResetWrongQuestion()
        {
            yield return new WaitForSeconds(1f);
            questionTxt.color = Color.white;
            isTimerOver = false;
            if (twoRow.activeInHierarchy)
            {
                foreach (var x in row1Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
                foreach (var x in row2Images2)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
            }
            
            if (threeRow.activeInHierarchy)
            {
                foreach (var x in row1Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
                foreach (var x in row2Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
                foreach (var x in row3Images3)
                {
                    x.transform.parent.GetComponent<Button>().interactable = true;
                }
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
            
            //Debug.Log("With Combo Total is now : " + points);
        
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
            
            //Debug.Log("With Combo Total is now : " + points);
        
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

        }
    }
}
