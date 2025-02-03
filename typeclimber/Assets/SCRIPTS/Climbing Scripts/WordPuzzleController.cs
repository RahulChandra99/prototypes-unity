using System;
using System.Collections;
using System.Collections.Generic;
using puzzleIO;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordPuzzleController : MonoBehaviour
{
    [Header("Game Variables")]
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
    
    
    [Header("Q&A variables")] 
    
    [Header("Round 1 Questions")]
    public List<QuestionAndAns> R1QnA;
    [Header("Round 2 Questions")]
    public List<QuestionAndAns> R2QnA;
    [Header("Round 3 Questions")]
    public List<QuestionAndAns> R3QnA;

    [Header("Answer Variables")]
    public int correctAnswerCounter;
    public int wrongAnswerCounter;
    public List<int> usedNumber;
    public int currentQuestion;

    [Header("Misc Variables")] 
    public float vibrationIntensity;
    public GameObject enterBtn;
    public GameObject alphaPad;
    public GameObject numberPad;
    
    
    

    private void OnEnable()
    {
        //number of correct answer at the start of each round
        correctAnswerCounter = 0;
        wrongAnswerCounter = 0;
        usedNumber = new List<int>();
        
        InitialCheckRoundNumber();
        
        slider.maxValue = maxTime;
        slider.value = maxTime;
        isTimerOver = false;
        timer = 0f;
        
    }
    
    void StartTimer()
    {
        timer += Time.deltaTime;
        float time = maxTime - timer;

        if (time <= 0)
        {
            isTimerOver = true;
                
            QuestionGenerationBasedOnRoundNumber();
                
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

    private void InitialCheckRoundNumber()
    {
        if (GM.Instance.roundNumber == 1)
        {
            for (int n = 0; n < R1QnA.Count; n++)    
            {
                usedNumber.Add(n);
            }
            int index = UnityEngine.Random.Range(0,usedNumber.Count-1);
            currentQuestion = usedNumber[index];
            questionTXT.text = R1QnA[currentQuestion].Question;
            usedNumber.RemoveAt(index); 
        }
        
        if (GM.Instance.roundNumber == 2)
        {
            for (int n = 0; n < R2QnA.Count; n++)    
            {
                usedNumber.Add(n);
            }
            int index = UnityEngine.Random.Range(0,usedNumber.Count-1);
            currentQuestion = usedNumber[index];
            questionTXT.text = R2QnA[currentQuestion].Question;
            usedNumber.RemoveAt(index); 
        }
        
        if (GM.Instance.roundNumber == 3)
        {
            for (int n = 0; n < R3QnA.Count; n++)    
            {
                usedNumber.Add(n);
            }
            int index = UnityEngine.Random.Range(0,usedNumber.Count-1);
            currentQuestion = usedNumber[index];
            questionTXT.text = R3QnA[currentQuestion].Question;
            usedNumber.RemoveAt(index); 
        }
    }

    public void QuestionGenerationBasedOnRoundNumber()
    {
        if (GM.Instance.roundNumber == 1)
        {
            int index = UnityEngine.Random.Range(0,usedNumber.Count-1);
            currentQuestion = usedNumber[index];
            questionTXT.text = R1QnA[currentQuestion].Question;
            usedNumber.RemoveAt(index);
        }
        if (GM.Instance.roundNumber == 2)
        {
            int index = UnityEngine.Random.Range(0,usedNumber.Count-1);
            currentQuestion = usedNumber[index];
            questionTXT.text = R2QnA[currentQuestion].Question;
            usedNumber.RemoveAt(index);
        }
        
        if (GM.Instance.roundNumber ==3)
        {
            int index = UnityEngine.Random.Range(0,usedNumber.Count-1);
            currentQuestion = usedNumber[index];
            questionTXT.text = R3QnA[currentQuestion].Question;
            usedNumber.RemoveAt(index);
        }
    }
    
    //Vibration on touch
    void VibrateOnPress()
    {
        long vibTime = Convert.ToInt64(vibrationIntensity);
        Vibration.Vibrate(vibTime);
    }
    
    //Numpad and alphabet toggle
  /*  void ButtonToggle()
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
    }*/
    
    //For detecting letter on key press
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

    //Change between 2 keypads
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
    
    //Skip Question button
    public void SkipQuestion()
    {
        //For Skip Button
        QuestionGenerationBasedOnRoundNumber();
        textInput.text = "";
    }

    //Enter and check answers
    public void EnterBtn()
    {
        if (GM.Instance.roundNumber == 1)
        {
            if (R1QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
            {
               CorrectAnswer();

               if (correctAnswerCounter == 3)
               {
                   NextRoundPrep();
               }
            }
            else if(!R1QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
            {
                WrongAnswer();
            }
        }
        
        if (GM.Instance.roundNumber == 2)
        {
            if (R2QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
            {
                CorrectAnswer();

                if (correctAnswerCounter == 3)
                {
                    NextRoundPrep();
                }
            }
            else if(!R2QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
            {
                WrongAnswer();
            }
        }
        
        if (GM.Instance.roundNumber == 3)
        {
            if (R3QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
            {
                CorrectAnswer();

                if (correctAnswerCounter == 3)
                {
                    Application.Quit();
                    NextRoundPrep();
                }
            }
            else if(!R3QnA[currentQuestion].Answer.Contains(textInput.text.ToUpper()))
            {
                WrongAnswer();
            }
        }
    }

    void CorrectAnswer()
    {
        Debug.Log("You are Correct");
        correctAnswerCounter++;
        textInput.text = "";
        QuestionGenerationBasedOnRoundNumber();
        PC.Instance.SpawnPlatform();
        PC.Instance.JumpForward();
        Debug.Log(correctAnswerCounter);
    }

    void WrongAnswer()
    {
        Debug.Log("You are Wrong");
        wrongAnswerCounter++;
        textInput.text = "";
    }

    void NextRoundPrep()
    {
        GM.Instance.resultUI.SetActive(true);
        Destroy(GM.Instance.topPart);
        GM.Instance.gameUI.SetActive(false);
        GM.Instance.roundNumber++;
        GM.Instance.PuzzleController.SetActive(false);
        
        //Result screen update
        GM.Instance.correctAns.text = correctAnswerCounter.ToString();
        GM.Instance.wrongAns.text = wrongAnswerCounter.ToString();
        GM.Instance.countingToTotalPlayers = 0;
        if (GM.Instance.totalPlayersInLobby == 64)
        {
            GM.Instance.totalPlayersInLobby = 16;
        }
        else if (GM.Instance.totalPlayersInLobby == 16)
        {
            GM.Instance.totalPlayersInLobby = 4;
        }
        

    }

    
}
