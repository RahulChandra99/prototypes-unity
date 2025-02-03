using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace puzzleIO
{
    public class WordType_Manager : MonoBehaviour
    {
        [SerializeField] private bool canType;
        
        public TextMeshProUGUI questionTXT;
        public TextMeshProUGUI textInput;

        [Header("Timer Variables")]
        public float maxTime;
        public Slider slider;
        private float timer;
        public Gradient timerGradient;
        public Image timerFillArea;
        public bool isTimerOver;
        [SerializeField] public float timeAfterWhichTimerStarts;

        [Header("For Delay Animation")]
        public GameObject playerGO;
        public float DelayTimer;
        private float temp;
        private bool isAnswered;

        public GameObject enterBtn;
        public float vibrationIntensity;
        
        [Header("Q&A variables")] 
        [Header("Round 1 Questions")]
        public List<QuestionAndAns> R1QnA;
        [Header("Round 2 Questions")]
        public List<QuestionAndAns> R2QnA;
        [Header("Round 3 Questions")]
        public List<QuestionAndAns> R3QnA;
        public int currentQuestion;
        public List<int> usedNumbers = new List<int>();
        public int correctAnsCounter;

       

        public GameObject numberPad;
        public GameObject alphaPad;

        public GameObject ResultScreen;

        private void Awake()
        {
            

            correctAnsCounter = 0;
        }

        private void OnEnable()
        {
            //When the actual timer and game starts
            GameIni();
           
            
        }

        private void Update()
        {
           
           ButtonToggle();
            
            if (GM.Instance.isGameStarted && !isTimerOver && canType)
            {
               /* Enable Keyboard Input Field
               textInput.Select();
               textInput.ActivateInputField();*/
            }

            //timer Gradient Color
            timerFillArea.color = timerGradient.Evaluate(slider.normalizedValue);

            DelayAnimation();
            
        }
        
        //CallBack Functions

        void GameIni()
        {
            Debug.Log("GameIni");
            alphaPad.SetActive(true);
            numberPad.SetActive(false);

            
            
            slider.maxValue = maxTime;
            slider.value = maxTime;
            isTimerOver = false;
            timer = 0f;
            canType = false;
           
            temp = DelayTimer;
            Debug.Log(GM.Instance.roundNumber + " Round number");
            if (GM.Instance.roundNumber == 1)
            {
                for (int n = 0; n < R1QnA.Count; n++)    
                {
                   
                    usedNumbers.Add(n);
                }
                
        
                int index = UnityEngine.Random.Range(0,usedNumbers.Count-1);
                currentQuestion = usedNumbers[index];
                questionTXT.text = R1QnA[currentQuestion].Question;
                usedNumbers.RemoveAt(index); 
            }
            
            if (GM.Instance.roundNumber == 2)
            {
                for (int n = 0; n < R2QnA.Count; n++)    
                {
                    usedNumbers.Add(n);
                }
        
                int index = UnityEngine.Random.Range(0,usedNumbers.Count-1);
                currentQuestion = usedNumbers[index];
                questionTXT.text = R2QnA[currentQuestion].Question;
                usedNumbers.RemoveAt(index); 
            }
            StartCoroutine("InitialAnimation");
        }
        
        void RandomQGenerator()
        {
            if (GM.Instance.roundNumber == 1)
            {
                int index = UnityEngine.Random.Range(0,usedNumbers.Count-1);
                currentQuestion = usedNumbers[index];
                //currentQuestion = UnityEngine.Random.Range(0, R1QnA.Count);
                questionTXT.text = R1QnA[currentQuestion].Question;
                usedNumbers.RemoveAt(index);
            }
            
            if (GM.Instance.roundNumber == 2)
            {
                int index = UnityEngine.Random.Range(0,usedNumbers.Count-1);
                currentQuestion = usedNumbers[index];
                //currentQuestion = UnityEngine.Random.Range(0, R1QnA.Count);
                questionTXT.text = R2QnA[currentQuestion].Question;
                usedNumbers.RemoveAt(index);
            }
            
            
            

        }
        
        void StartTimer()
        {
            timer += Time.deltaTime;
            float time = maxTime - timer;

            if (time <= 0)
            {
                isTimerOver = true;
                
                RandomQGenerator();
                
                slider.maxValue = maxTime;
                slider.value = maxTime;
                isTimerOver = false;
                timer = 0f;
                
                //madeUpWord.text = "";
             
            }

            if (isTimerOver == false)
            {
                slider.value = time;
            }
        }

        void VibrateOnPress()
        {
            long vibTime = Convert.ToInt64(vibrationIntensity);
            Vibration.Vibrate(vibTime);
        }
        
        void ButtonToggle()
        {
            if (textInput.text != "")
            {
                enterBtn.gameObject.GetComponent<Image>().color = Color.green;
                enterBtn.gameObject.GetComponent<Button>().enabled = true;
            }
            else
            {
                enterBtn.gameObject.GetComponent<Image>().color = Color.gray;
                enterBtn.gameObject.GetComponent<Button>().enabled = false;
            } 
        }

        void DelayAnimation()
        {
            if (temp <= DelayTimer - 5)
            {
                isAnswered = false;
            }
            temp -= Time.deltaTime;
            if (temp <= 0 && !isAnswered)
            {
                playerGO = GameObject.FindWithTag("Player");
                if (playerGO)
                {
                    playerGO.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Delay_01");
                    temp = DelayTimer;
                }
            }
        }
        public void DetectLetter()
        {
            VibrateOnPress();
       
            string letter;
            letter = EventSystem.current.currentSelectedGameObject.name.ToUpper();

            if (letter == "SPACE")
            {
                textInput.text += " ";
            }
            else if (letter == "BACK")
            {
                textInput.text = textInput.text.Remove(textInput.text.Length - 1);
            }
            else
            {
                textInput.text += letter;
            }

        }

        public void ToggleNumPad()
        {
            if (alphaPad.activeInHierarchy)
            {
                numberPad.SetActive(true);
                alphaPad.SetActive(false);
            }

            else if (numberPad.activeInHierarchy)
            {
                alphaPad.SetActive(true);
                numberPad.SetActive(false);
            }
        }
    
        public void EnterAnswer()
        {
    
       if (R1QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
       {
           MultiAnsCorrectAnswer();
           Debug.Log("Correct");
           correctAnsCounter++;

           if (GM.Instance.roundNumber==1 && correctAnsCounter == 4)
           {
               
               correctAnsCounter = -1;
               
               //GM.Instance.LoadNextRound();
               
           }
           
       }

       else
       {
           MultiAnsWrongAnswer();
           Debug.Log("Wrong");
       }
            
            isAnswered = true;
            temp = DelayTimer;
        }

        

        public void SkipQuestion()
        {
            //For Skip Button
            RandomQGenerator();
            textInput.text = "";
        }

        void MultiAnsCorrectAnswer()
        {
            textInput.text = "";
            RandomQGenerator();
            
            FindObjectOfType<PlayerController>().RightAnswer();
        }

        void MultiAnsWrongAnswer()
        {
            textInput.text = "";
            
            
        }
        
        //Coroutines
        
        IEnumerator InitialAnimation()
        {
            yield return new  WaitForSeconds(timeAfterWhichTimerStarts);
            canType = true;
        
            if (GM.Instance.isGameStarted && !isTimerOver && canType)
            {
                InvokeRepeating("StartTimer",0f,0.001f);
            }
        }
        
    }
  
}
