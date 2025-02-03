using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class WordPuzzleController_Climbing : MonoBehaviour
{
    [Header("WordPuzzleGame Section")]
    public TextMeshProUGUI questionTXT;
    public TextMeshProUGUI textInput;

    [Header("Timer Variables")]
    public float maxTime;
    public Slider slider;
    private float timer;
    public Gradient timerGradient;
    public Image timerFillArea;
    [HideInInspector] public bool isTimerOver;

    [Header("Misc Variables")]
    public float vibrationIntensity;
    public GameObject enterBtn;
    public GameObject alphaPad;
    // public GameObject numberPad;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
    // public GameObject keyPad;
    public CinemachineVirtualCamera cmvc;
    [HideInInspector] public GameObject Player;
    public TimelineAsset Players;
    public TimelineAsset PlayerWin;

    [Header("Scoring System")]
    [SerializeField] float AttemptTime;
    public int ComboCount;
    public bool timerEnabled;
    public GameObject FText;
    public GameObject SText;
    public GameObject TText;
    [HideInInspector] public TextMeshProUGUI playerTotalScore;
    public GameObject comboBar;
    public TextMeshProUGUI comboText;

    public bool changeQuestion;
    List<string> answersCorrect = new List<string>();
   
    TouchScreenKeyboard keyboard;
    bool keyboardMode = true;

    private void OnEnable()
    {
        QuestionBank.instance.initializeQuestions();
        GM_Climbing.Instance.ResetComplete();

        keyboardMode = true;
        answersCorrect.Clear();

        isTimerOver = false;

        PuzzleIni();

        InitialCheckRoundNumber();

        ResetTimer();

        SetKeysActive();

        findPlayer();
        QuestionBank.instance.changeRound += NextRoundPrep;

       

    }

    void Awake()
    {
       // QuestionBank.instance.initializeQuestions();
    }

    string answer;
    public void setKeyboardOn()
    {
      
      
#if !UNITY_EDITOR
        if (keyboardMode)
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
            

        }
        else
        {
            //KeyboardBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -250f);
        }
#endif

    }
    bool keyboardDone;
    void checkKeyboardStatus()
    {
        if (keyboard != null)
        {
            if (keyboardMode && GM_Climbing._instance.keyboardOn)
            {

                if (keyboard.status == TouchScreenKeyboard.Status.Done)
                {
                    if (!keyboardDone)
                    {

                        keyboardDone = true;
                        textInput.text = keyboard.text;
                        EnterBtn(answer);
                        Debug.LogError("keyboard is done with " + answer);
                        //keyboard = null;
                        
                        setKeyboardOn();
                        
                    }
                }

                if (keyboard.status == TouchScreenKeyboard.Status.LostFocus || keyboard.status == TouchScreenKeyboard.Status.Canceled)
                {
                    keyboard = null;
                    answer = "";
                    setKeyboardOn();
                    keyboardDone = true;

                }



                if (keyboard.status == TouchScreenKeyboard.Status.Visible)
                {
                    answer = keyboard.text;
                    keyboardDone = false;
                }
               
                
            }
        }

    }

    void PuzzleIni()
    {
        QuestionBank.instance.ResetQCounter();
        timerEnabled = false;
        ComboCount = 0;
        GM_Climbing.Instance.inputTextField.SetActive(false);

        comboBar.SetActive(false);
        FText.gameObject.SetActive(false);
        SText.gameObject.SetActive(false);
        TText.gameObject.SetActive(false);
    }

    void SetKeysActive()
    {
        var c = FindObjectsOfType<Button>();
        foreach (var key in c)
        {
            key.GetComponent<Button>().interactable = true;
        }


    }

    void Start()
    {
        //Camera Follow
        cmvc.Follow = Player.transform.GetChild(0);
        cmvc.LookAt = Player.transform.GetChild(0);
        //QuestionBank.instance.initializeQuestions();
    }

    void findPlayer()
    {
        var a = FindObjectsOfType<PlayerControls_2>();
        foreach (var x in a)
        {
            if (!x.isAI)
            {
                Player = x.gameObject;
                x.playerdone += QuestionGenerationBasedOnRoundNumber; //subscribe
                                                                      // Debug.Log("subscribe");
            }
        }
    }

    private void Update()
    {

        //Keyboard PC

#if UNITY_EDITOR
        PckeyboardInput();
#endif

        EnableNDisableInputField();
        checkKeyboardStatus();

    }

    void PckeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            EnterBtn(textInput.text.ToUpper());

        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in keyCodes)
            {
                if (Input.GetKey(keyCode))
                {
                    // Debug.Log("KeyCode down: " + keyCode);
                    if (keyCode.ToString() != "Return" && keyCode.ToString() != "Mouse0")
                    {
                        DetectKeyboardLetter(keyCode.ToString());
                    }
                    break;
                }
            }
        }
    }

    void EnableNDisableInputField()
    {
        //Input field enable and disable
        if (textInput.text == "")
        {
            var a = FindObjectsOfType<PlayerControls_2>();
            if (a.Length > 0 && a != null)
            {
                foreach (var x in a)
                {
                    if (!x.isAI)
                    {
                        x.DisableTyping();
                    }
                }
            }
            GM_Climbing.Instance.inputTextField.SetActive(false);
        }
        if (textInput.text != "")
        {
            var a = FindObjectsOfType<PlayerControls_2>();
            if (a.Length > 0 && a != null)
            {
                foreach (var x in a)
                {
                    if (!x.isAI)
                    {
                        x.EnableTyping();
                    }
                }
            }
            GM_Climbing.Instance.inputTextField.SetActive(true);
            // turn on native keyboard.
        }
    }

    void FixedUpdate()
    {
        if (!isTimerOver)
        {
            if (GM_Climbing._instance.keyboardOn)
            {
                if (GM_Climbing._instance.roundBanner.activeInHierarchy == false)
                {
                    Invoke("StartTimer", 0f);
                }
                
            }
            
        }
    }



    void StartTimer()
    {

        timer += Time.fixedDeltaTime;
        float time = maxTime - timer;
        AttemptTime = time;
        if (time <= 0)
        {
            isTimerOver = true;
            //textInput.text = "";
            //QuestionGenerationBasedOnRoundNumber();

            // slider.maxValue = maxTime;
            //slider.value = maxTime;
            //isTimerOver = false;
            //timer = 0f;

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

    private void InitialCheckRoundNumber()
    {
        enterBtn.GetComponent<Button>().interactable = true;
        questionTXT.text = QuestionBank.instance.getQuestion(GM_Climbing._instance.MatchNumber, GM_Climbing.Instance.roundNumber - 1);
    }


    public void QuestionGenerationBasedOnRoundNumber()
    {
        //keypad turn on
        timerEnabled = true;
        timer = 0;
        Invoke("setKeyboardOn", .5f);
        //enable Enter key
        enterBtn.GetComponent<Button>().interactable = true;

        Invoke("SetScoreAnimationOff", 1.5f);

        SetKeysActive();

        if (changeQuestion)
        {
            questionTXT.text = QuestionBank.instance.getQuestion(GM_Climbing._instance.MatchNumber, GM_Climbing.Instance.roundNumber - 1);
        }




    }

    void SetScoreAnimationOff()
    {
        FText.gameObject.SetActive(false);
        SText.gameObject.SetActive(false);
        TText.gameObject.SetActive(false);

    }

    //Vibration on touch
    void VibrateOnPress()
    {
        long vibTime = Convert.ToInt64(vibrationIntensity);
        Vibration.Vibrate(vibTime);
    }

    //Numpad and alphabet toggle

    //For detecting letter on key press
    GameObject pressed;
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

        pressed = EventSystem.current.currentSelectedGameObject.gameObject;

        pressed.transform.GetChild(1).gameObject.SetActive(true);
        Invoke("RemoveExtraLetter", 0.01f);

    }

    void RemoveExtraLetter()
    {
        Debug.Log("removed Letter");
        pressed.transform.GetChild(1).gameObject.SetActive(false);
    }

    void DetectKeyboardLetter(string key)
    {
        if (key == "Space")
        {
            textInput.text += " ";
        }
        else if (key == "Backspace")
        {
            textInput.text = textInput.text.Remove(textInput.text.Length - 1);
        }
        else
        {
            textInput.text += key;
        }

    }

    //Change between 2 keypads
    /* public void ToggleNumPad()
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
     }*/

    //Skip Question button
    public void SkipQuestion()
    {
        //For Skip Button
        QuestionGenerationBasedOnRoundNumber();
        textInput.text = "";

        ResetTimer();
        comboBar.SetActive(false);

    }



    //Enter and check answers
    public void EnterBtn(string answer)
    {
        
        if (answer != "")
        {
            //setting keypad off
            // numberPad.SetActive(false);
            Debug.Log("Answer Recieved is " + answer);
            enterBtn.GetComponent<Button>().interactable = false;
            isTimerOver = true;
            keyboardDone = false;


            Invoke("ResetTimer", 2f);

            
            //Correct Answer
            if (QuestionBank.instance.CheckAnswer(GM_Climbing._instance.MatchNumber, answer.ToUpper(), GM_Climbing.Instance.roundNumber - 1))
            {
                var a = FindObjectsOfType<Button>();
                foreach (var key in a)
                {
                    key.GetComponent<Button>().interactable = false;
                }

                GM_Climbing.Instance.rightQ[GM_Climbing.Instance.roundNumber - 1]++;
                GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber - 1]++;
                CorrectAnswer();
                Debug.Log("Answer Correct");
            }
            //Wrong Answer
            else
            {
                //attempted the question
                GM_Climbing.Instance.attemptQ[GM_Climbing.Instance.roundNumber - 1]++;
                GM_Climbing.Instance.wrongQ[GM_Climbing.Instance.roundNumber - 1]++;
                WrongAnswer();
                enterBtn.GetComponent<Button>().interactable = true;
                Debug.Log("Answer Wrong");
            }
 
        }
        

    }

    void CorrectAnswer()
    {
        Debug.Log("went inside correctAnswer() loop");
        if (!changeQuestion)
        {
            if (answersCorrect.Contains(textInput.text.ToUpper()))
            {
                var c = FindObjectsOfType<Button>();
                foreach (var key in c)
                {
                    key.GetComponent<Button>().interactable = true;
                }
                return;
            }
            else
            {
                answersCorrect.Add(textInput.text.ToUpper());
            }
        }


        timerEnabled = false;
        ComboCount++;
        if (ComboCount >= 5)
        {
            ComboCount = 5;
        }

        if (changeQuestion)
        {
            questionTXT.text = "......";
        }

        textInput.text = "";

        GM_Climbing.Instance.totalCorrect++;


        int points = 0;
        Debug.Log("Attempt Time:" + AttemptTime);
        if (AttemptTime >= 17)
        {
            points = 100;

            FText.gameObject.SetActive(true);

        }
        else if (AttemptTime >= 7 && AttemptTime < 17)
        {
            points = 70;

            SText.gameObject.SetActive(true);
        }
        else if (AttemptTime > 0 && AttemptTime < 7)
        {
            points = 40;

            TText.gameObject.SetActive(true);
        }
        else
        {
            points = 0;
        }
        Debug.Log("points got based on attempt: " + points);

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


            playerTotalScore = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
            GM_Climbing.Instance.r1C += points;
            playerTotalScore.text = GM_Climbing.Instance.r1C.ToString();
        }
        if (GM_Climbing.Instance.roundNumber == 2)
        {


            playerTotalScore = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
            GM_Climbing.Instance.r2C += points;
            playerTotalScore.text = GM_Climbing.Instance.r2C.ToString();
        }
        if (GM_Climbing.Instance.roundNumber == 3)
        {


            playerTotalScore = GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>();
            GM_Climbing.Instance.r3C += points;
            playerTotalScore.text = GM_Climbing.Instance.r3C.ToString();
        }
        Debug.Log("correct answer method before giving it to player");
        /*  var a = FindObjectsOfType<PlayerControls_2>();
          PlayerControls_2 player = null;
          foreach (var x in a)
          {
              if (!x.isAI)
              {
                  player = x;
              }
          }*/

        if (Player)
        {
            Debug.Log("Givin player points");
            Player.GetComponent<PlayerControls_2>().CorrectAnswer(points);
        }
        else
        {
            Debug.Log("player not found yet trying to give it ");
            findPlayer();
            Player.GetComponent<PlayerControls_2>().CorrectAnswer(points);
        }
        Debug.Log("correct answer method after giving it to player");
        //Combo Counter
        if (comboBar)
        {
            comboText.text = ComboCount.ToString() + "X";

            if (ComboCount > 1)
            {
                comboBar.SetActive(true);
                Invoke("OffCombo", 2.5f);
            }
            else
            {
                comboBar.SetActive(false);
            }
        }

    }

    public void TurnOffKeyboard()
    {
        if (keyboard != null)
        {
            keyboard.active = false;
            keyboardMode = false;
            GM_Climbing._instance.keyboardOn = false;

        }
       
    }

    public void TurnKeyboardInputModeON()
    {
        keyboardMode = true;
    }

    void OffCombo()
    {
        comboBar.SetActive(false);
    }

    void WrongAnswer()
    {
        Debug.Log("You are Wrong");
        ComboCount = 0;
        textInput.text = "";
        comboBar.SetActive(false);
        VibrateOnPress();
        var g = FindObjectsOfType<PlayerControls_2>();
        foreach (var x in g)
        {
            if (!x.isAI)
            {
                x.WrongAnswerFeedback();
            }
        }
    }


    public void NextRoundPrep()
    {
        GM_Climbing.Instance.ResultScreenUpdate();
        StartCoroutine(WaitAndRoundFinish());

    }

    IEnumerator WaitAndRoundFinish()
    {

        yield return new WaitForSeconds(0.1f);
        if (Player)
        {
            // Player.GetComponent<PlayerControls_2>().Won();
        }
        else
        {
            findPlayer();
            //Player.GetComponent<PlayerControls_2>().Won();
        }

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

    void OnDisable()
    {
        if (Player)
        {
            Player.GetComponent<PlayerControls_2>().playerdone -= QuestionGenerationBasedOnRoundNumber; //subscribe
        }
        QuestionBank.instance.changeRound -= NextRoundPrep;

        ResetTimer();
    }


    public float GetHeightofKeyboard()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);

                return Screen.height - Rct.Call<int>("height");
            }
        }
#elif UNITY_IOS
        return (int)TouchScreenKeyboard.area.height;
#elif UNITY_EDITOR
        return 15;
#endif
        return 0;
    }

}
