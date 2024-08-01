using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<QnA> QandA1;
    [Space(5)]
    public List<QnA> QandA2;
    
    
    public TextMeshProUGUI QuestionText;
    public GameObject[] Options;
    public TextMeshProUGUI scoreText;
    public int score;
    public int currentQuestion;
    private bool isCorrect;
    public int levelNumber;

    public TextMeshProUGUI levelnumberText;
    public TextMeshProUGUI questionCounter;
    public int numberOfQs;
    public TextMeshProUGUI winText;
    public GameObject nextLevelBtn;
    private int mycurrentQuestion;
    private int totalQuestion;
    public TextMeshProUGUI name;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void OnEnable()
    {
        totalQuestion = QandA1.Count;
        
        if (FacebookIntegration.Instance.myName!=null)
        {
            name.text = FacebookIntegration.Instance.myName;
        }
        else
        {
            name.GetComponent<GameObject>().SetActive(false);
            name.transform.GetComponentInParent<GameObject>().SetActive(false);
        }
        numberOfQs = QandA1.Count;
        winText.text = "";
        nextLevelBtn.SetActive(false);
        levelNumber = 1;
        score = 0;
        GenerateQuestion();
        SetAnswer();
        mycurrentQuestion = 1;

    }

    void Update()
    {
        scoreText.text = score.ToString() + " Points";
        levelnumberText.text = "Level " + levelNumber;
        if (levelNumber == 1)
        {
            if (mycurrentQuestion <= totalQuestion)
            {
                questionCounter.text = mycurrentQuestion.ToString() + "/" + totalQuestion;
            }
            
        }
        if (levelNumber == 2)
        {
            if (mycurrentQuestion <= totalQuestion)
            {
                questionCounter.text = mycurrentQuestion.ToString() + "/" + totalQuestion;
            }
            
        }
        
    }
    void GenerateQuestion()
    {
        
        
        if (numberOfQs > 0)
        {
            for (int i = 0; i < Options.Length; i++)
            {
                Options[i].GetComponent<Button>().interactable = true;
            }
            
            if (levelNumber == 1)
            {
                currentQuestion = UnityEngine.Random.Range(0, QandA1.Count);
                QuestionText.text = QandA1[currentQuestion].Question;
            }
            
            if (levelNumber == 2)
            {
                currentQuestion = UnityEngine.Random.Range(0, QandA2.Count);
                QuestionText.text = QandA2[currentQuestion].Question;
            }
        }
        else
        {
            if (levelNumber == 1)
            {
                winText.text = "Press Next to Start Next Level";
            }
            else

            {
                SceneManager.LoadSceneAsync("Result");
            }
           
            for (int i = 0; i < Options.Length; i++)
            {
                Options[i].GetComponent<Button>().interactable = false;
            }

            nextLevelBtn.SetActive(true);
        }


        mycurrentQuestion++;
    }
    
    void SetAnswer()
    {
        if (levelNumber == 1)
        {
            for (int i = 0; i < Options.Length; i++)
            {
                isCorrect = false;
                Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    QandA1[currentQuestion].Answers[i];
            }
        }
        
        if (levelNumber == 2)
        {
            for (int i = 0; i < Options.Length; i++)
            {
                isCorrect = false;
                Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    QandA2[currentQuestion].Answers[i];
            }
        }
       
        
        
    }
    
    public void CheckAnswer()
    {
        if (levelNumber == 1)
        {
            if (QandA1[currentQuestion].correctAns.Contains(EventSystem.current.currentSelectedGameObject
                .transform
                .GetChild(0).GetComponent<TextMeshProUGUI>().text.ToUpper()))
            {
                OnCorrect();
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
                StartCoroutine("WaitAndLoadQ");
        
            }
            else
            {
                winText.text = "Wrong!!";
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.red;
                StartCoroutine("WaitAndResetWrong");
            }
        }
        if (levelNumber == 2)
        {
            if (QandA2[currentQuestion].correctAns.Contains(EventSystem.current.currentSelectedGameObject
                .transform
                .GetChild(0).GetComponent<TextMeshProUGUI>().text.ToUpper()))
            {
                OnCorrect();
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
                StartCoroutine("WaitAndLoadQ");
        
            }
            else
            {
                winText.text = "Wrong!!";
                winText.color = Color.red;
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.red;
                StartCoroutine("WaitAndResetWrong");
            }
        }
           
        
        
    
    }

    void OnCorrect()
    {
        winText.text = "Correct!!";
        winText.color = Color.green;
        score +=10;
        scoreText.text = score.ToString() + " Points";
    }
    
    

    IEnumerator WaitAndLoadQ()
    {


        yield return new WaitForSeconds(1.5f);
        
        winText.text = "";
        for (int i = 0; i < Options.Length; i++)
        {
            Options[i].GetComponent<Image>().color = Color.white;
        }

        if (numberOfQs > 0)
        {
            //QuestionGeneration
            if (levelNumber == 1)
            {
                QandA1.RemoveAt(currentQuestion);
            }
            if (levelNumber == 2)
            {
                QandA2.RemoveAt(currentQuestion);
            }
        }
       
        
        numberOfQs--;
        
        
        GenerateQuestion();
        SetAnswer();
    }
    
    IEnumerator WaitAndResetWrong()
    {
       
        yield return new WaitForSeconds(1f);
        
        winText.text = "";
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.white;
        
        if (numberOfQs > 0)
        {
            //QuestionGeneration
            if (levelNumber == 1)
            {
                QandA1.RemoveAt(currentQuestion);
            }
            if (levelNumber == 2)
            {
                QandA2.RemoveAt(currentQuestion);
            }
        }
        numberOfQs--;
        
        GenerateQuestion();
        SetAnswer();

    }

    public void NextLevelBtn()
    {
        totalQuestion = QandA2.Count;
        levelNumber++;
        numberOfQs = QandA2.Count;
        mycurrentQuestion = 1;
        GenerateQuestion();
        SetAnswer();
        winText.text = "";
    }

    public void ResetGame()
    {
        SceneManager.LoadSceneAsync("TriviaQuiz/Scenes/MainMenu");
    }
}
