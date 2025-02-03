using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using Facebook.Unity;

public class GM_Climbing : MonoBehaviour
    {
        
        [Header("Game UI Panels")] 
        public GameObject mainMenuUI;
        public GameObject playerSelectUI;
        public GameObject gameUI;
        public GameObject resultUI;
        public GameObject lobbyUI;
        public GameObject roundBanner;
        public GameObject loseScreen;
        public GameObject roundWinScreen;
        public GameObject inputTextField;
        public GameObject transitionScreen;
        public GameObject progressBar;
        public GameObject trphyImage;
        public GameObject leaderBoardUI;
        //public GameObject questionBox;

        [Header("True and False Category in TriviaPuzzle")]
        public GameObject triviaOption3;
        public GameObject triviaOption4;
      
        //Round Banner UI
        [Header("RoundBanner Variables")] 
        public TextMeshProUGUI countdownTimerText;
        public TextMeshProUGUI roundNumberText;
        public TextMeshProUGUI getReadyText;
        [Tooltip("Roundbanner Countdown Timer" )] public int countDownTimer;
        
        //LobbyUI
        [Header("Lobby Loadout")]
        public TextMeshProUGUI playerCounter;
        public int roundNumber;
        public int totalPlayersInLobby;
        public float countingToTotalPlayers;
        private float lastTimeStamp;
        
        //LoseScreen UI
        [Header("LosePanel")]
        public TextMeshProUGUI roundLossNumber;
        
        //Round Win UI
        [Header("RoundWin Screen")] 
        public TextMeshProUGUI roundCountText;
        public TextMeshProUGUI roundWonText;
        public int roundEndTimer;
        public TextMeshProUGUI roundEndTimerText;
        [HideInInspector]public bool isNextRoundStarted;

        //Match Result Screen
        [Header("Final Result Screen")] 
        public int r1C;
        public int r2C;
        public int r3C;
        
        [Header("Round1")]
        public int[] attemptQ;
        public TextMeshProUGUI[] attemptText;
        public int[] rightQ;
        public TextMeshProUGUI[] rightText;
        public int[] wrongQ;
        public TextMeshProUGUI[] wrongText;
        public float[] accuracy;
        public TextMeshProUGUI[] accuracyText;
        public int totalCorrect;

        //Top Climbing Part
        [Header("Prefabs for top Part")]
        public GameObject Round1;
        public GameObject PuzzleController;
        private GameObject topPart;

        //Trivia Screen UI
        [Header("Trivia Panel")]
        public GameObject TriviaCanvas;
        public GameObject[] triviaCategories;
        //public GameObject TriviaManager;
        
        [Header("Game Bool Variables")]
        public bool isGameStarted;
        private bool gameCompleteCalled;
        public bool TriviaGame;

        
        //For UI Canvas updates
        public VerticalLayoutGroup _vertLayoutGroup;
        public VerticalLayoutGroup _vertLayoutGroup2;
        public VerticalLayoutGroup _vertLayoutGroupTrivia;
   

        [Header("MainMenu")]
        public TMP_InputField playerName;
        //Character prefab choosen as player
        [HideInInspector]public int index;
        float top = 0;
        public int randomNumber,MatchNumber,levelDifficulty;

        public int numberOfTrophies;

        #region SINGLETON PATTERN
        public static GM_Climbing _instance;
        public static GM_Climbing Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GM_Climbing>();
             
                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GM_Climbing");
                        _instance = container.AddComponent<GM_Climbing>();
                    }
                }
     
                return _instance;
            }
        }

        public int GetLengthOfQuestions(int round)
        {
            return QuestionBank.instance.GetLengthOfQuestions(0,round);
        }
        
     
        #endregion

    
        IEnumerator Start()
        {
            yield return new WaitForSeconds(.05f);
            //for try again functionality
            if (PlayerPrefs.GetInt("retry") == 1)
            {
                while (mainMenuUI.activeInHierarchy)
                {
                    mainMenuUI.SetActive(false);
                    yield return null;
                }
                PlayerPrefs.SetInt("retry", 0);
                
                PlayBtnTrivia();
               //PlayBtn();
            }
        }

        private void Awake()
        {

            numberOfTrophies = 1;
            
            //MatchNumber = PlayerPrefs.GetInt("matchnumber");
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
            resultUI.SetActive(false);
            lobbyUI.SetActive(false);
            roundBanner.SetActive(false);
            loseScreen.SetActive(false);
            roundWinScreen.SetActive(false);
            transitionScreen.SetActive(false);
            TriviaCanvas.SetActive(false);
            leaderBoardUI.SetActive(false);
            //TriviaManager.SetActive(false);
            //questionBox.SetActive(false);
            

            //set all trivia concepts unactive
            foreach (var a in triviaCategories)
            {
                a.SetActive(false);
            }
        
            GameInitilisation();
            Application.targetFrameRate = 60;

        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }

    }

        void sendLevelStart(int matchNo,int roundNo)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        EventData eventData = new EventData();
        eventData.matchNo = matchNo;
        eventData.RoundNo = roundNo;
        eventData.status = "lvl_Start";
        data.Add("1",eventData);
        try
        {
            AppMetrica.Instance.ReportEvent("level_start ", data);
        }
        catch
        {

        }
       
        FB.LogAppEvent("level_Start", null, data);

    }

        void sendLevelEnd(int matchNo, int roundNo,string status)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        EventData eventData = new EventData();
        eventData.matchNo = matchNo;
        eventData.RoundNo = roundNo;
        eventData.status = status;
        data.Add("1", eventData);
        try
        {
            AppMetrica.Instance.ReportEvent("level_End", data);
        }
        catch
        {

        }

        FB.LogAppEvent("level_End", null, data);

    }
        
        public void crownChange()
        {
            StartCoroutine(checkCrown());
        }
        
        void GameInitilisation()
        {
            
            PuzzleController.SetActive(false);
            
            //GameIni
            isGameStarted = false;
            roundNumber = 1;
            totalPlayersInLobby = 27;
            countingToTotalPlayers = 0;
            gameCompleteCalled = false;
            TriviaGame = false;
            countDownTimer = 3;
            roundWonText.gameObject.SetActive(true);

            if (PlayerPrefs.GetString("playername").Length > 0)
            {
                playerName.text = PlayerPrefs.GetString("playername");
            }
            else
            {
                playerName.text = "Player" + UnityEngine.Random.Range(0, 5000);
            }
            
        }

        private void Update()
        {
            UpdateCanvasCont();
            
        }

        void UpdateCanvasCont()
        {
            //Update the Q panel Continously
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_vertLayoutGroup.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(_vertLayoutGroup2.GetComponent<RectTransform>());
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_vertLayoutGroupTrivia.GetComponent<RectTransform>());
            
        }

        public void PlayBtn()
        {
            
            Invoke("StartRoundIni",4f);
            RepeatCallLobby();
            Invoke("setPlayerSelectOff",.8f);
            isGameStarted = true;
            TriviaGame = false;
          
            SetPlayerName();
        }

     void setPlayerSelectOff()
    {
        if(playerSelectUI)
        playerSelectUI.SetActive(false);
    }
    
        public void PlayBtnTrivia()
        {
            TriviaCanvas.SetActive(true);
        Invoke("StartRoundIni",4f);
        RepeatCallLobby();
        Invoke("setPlayerSelectOff", .8f);
        
        isGameStarted = true;
        TriviaGame = true;
        
            //Setting Player Name
         SetPlayerName();
        }

        void SetPlayerName()
        {
         
                PlayerPrefs.SetString("playername",playerName.text);
            
        }

        void StartRoundIni()
        {
            if (TriviaGame)
            {
                TriviaCanvas.SetActive(true);   
            }
            
            roundBanner.SetActive(true);
            StartCoroutine("SetCountDownForRoundBanner");
            
            try
            {
                sendLevelStart(MatchNumber,roundNumber);
            }
            catch (Exception e)
            {
                Debug.LogError("Sending event issue" + e);
            }
        }

        IEnumerator SetCountDownForRoundBanner()
        {
            if (roundBanner)
            {
                if (roundNumber != 3)
                {
                    roundNumberText.text = "Round " + roundNumber.ToString();
                }
                else
                {
                    roundNumberText.text = "Final Round";
                }
                
            
                countdownTimerText.text = "" ;
                yield return new WaitForSeconds(1f);
                getReadyText.text = "Starting in ";
            
                while (countDownTimer > 0)
                {
                    if (!TriviaGame)
                    {
                        if (countDownTimer == 3)
                        {
                            if (roundNumber == 1)
                            {
                                Invoke("InGameUILoadOut", 0f);
                            }
                            if (roundNumber == 2)
                            {
                                Invoke("InGameUILoadOut", 0f);
                            }
                            if (roundNumber == 3)
                            {
                                Invoke("InGameUILoadOut", 0f);
                            }
                        } 
                    }
                    
                    countdownTimerText.text = countDownTimer.ToString() ;
                    yield return new WaitForSeconds(1f);
                    countDownTimer--;
                }

                countdownTimerText.text = "Go!";
                getReadyText.text = "";
                yield return new WaitForSeconds(1f);
        
                roundBanner.SetActive(false);

                if (TriviaGame)
                {
                    Invoke("InGameUILoadOut", 0f);
                }
                
            }
        
        }
    
        public void RepeatCallLobby()
        {
            roundEndTimer = 10;
            
            //InvokeRepeating("LoadLobby",0,0.01f);
            InvokeRepeating("LoadLobby2", 0f, 0.01f);

        }

        public void LoadLobby2()
        {
            
            if (roundNumber == 1)
            {
                if (Time.time - lastTimeStamp >= 0.15f)
                {
                    lastTimeStamp = Time.time;
                
                    if (countingToTotalPlayers <= totalPlayersInLobby)
                    {
                        lobbyUI.SetActive(true);
                        //Call the top Part function
                        if (topPart == null)
                        {
                            topPart = new GameObject("Top Part");
                            Instantiate(Round1, new Vector3(0, 0, 0), Quaternion.identity, topPart.transform);
                        }
                        
                        countingToTotalPlayers++;
                        playerCounter.text = (int) countingToTotalPlayers-1 + "/" + totalPlayersInLobby;
                    }

                    else
                    {
                        Invoke("LobbyUIDisable", 0.5f);
                        
                    }
                } 
            }
            
            if (roundNumber == 2)
            {
                if (Time.time - lastTimeStamp >= 0.2f)
                {
                    lastTimeStamp = Time.time;
                
                    if (countingToTotalPlayers <= totalPlayersInLobby)
                    {
                        lobbyUI.SetActive(true);
                        //Call the top Part function
                        if (topPart == null)
                        {
                            topPart = new GameObject("Top Part");
                            Instantiate(Round1, new Vector3(0, 0, 0), Quaternion.identity, topPart.transform);
                        }
                        
                        countingToTotalPlayers++;
                        playerCounter.text = (int) countingToTotalPlayers-1 + "/" + totalPlayersInLobby;
                    }

                    else
                    {
                        //Invoke("InGameUILoadOut", 0.3f);
                        Invoke("LobbyUIDisable", 0.5f);
                    }
                } 
            }
            
            if (roundNumber == 3)
            {
                if (Time.time - lastTimeStamp >= 0.8f)
                {
                    lastTimeStamp = Time.time;
                
                    if (countingToTotalPlayers <= totalPlayersInLobby)
                    {
                        lobbyUI.SetActive(true);
                        //Call the top Part function
                        if (topPart == null)
                        {
                            topPart = new GameObject("Top Part");
                            Instantiate(Round1, new Vector3(0, 0, 0), Quaternion.identity, topPart.transform);
                        }
                        
                        countingToTotalPlayers++;
                        playerCounter.text = (int) countingToTotalPlayers-1 + "/" + totalPlayersInLobby;
                    }

                    else
                    {
                        //Invoke("InGameUILoadOut", 0.6f);
                        Invoke("LobbyUIDisable", 0.5f);
                       
                    }
                } 
            }
            
        }

        void LobbyUIDisable()
        {
            lobbyUI.SetActive(false);
        }

        private List<int> doneConcepts = new List<int>();
        
        void InGameUILoadOut()
        {

            if (TriviaGame)
            {
                gameUI.SetActive(true);
                if (MatchNumber <= 2)
                {
                    if (roundNumber == 1)
                    {
                        levelDifficulty = 0;
                    }

                    if (roundNumber == 2)
                    {
                        levelDifficulty = UnityEngine.Random.Range(0, 1);
                    }

                    if (roundNumber == 3)
                    {
                        levelDifficulty = UnityEngine.Random.Range(1, 2);
                    }
                }

                else
                {
                    if (roundNumber == 1)
                    {
                        levelDifficulty = 1;
                    }

                    if (roundNumber == 2)
                    {
                        levelDifficulty = UnityEngine.Random.Range(1, 2);
                    }

                    if (roundNumber == 3)
                    {
                        levelDifficulty = 2;
                    }
                }
               
                if (roundNumber == 1)
                {
                    triviaCategories[0].SetActive(true);
                }
                if (roundNumber == 2)
                {
                    triviaCategories[1].SetActive(true);
                }
                if (roundNumber == 3)
                {
                    triviaCategories[UnityEngine.Random.Range(2,3)].SetActive(true);
                }

                
                
              /*  randomNumber = UnityEngine.Random.Range(2, 3);
                if (!doneConcepts.Contains(randomNumber))
                {
                    doneConcepts.Add(randomNumber);
                }
                else
                {
                   
                    Invoke("InGameUILoadOut",0.1f);
                    return;
                }  */
            }
            
            
            //GamePanel Load for Rounds
            //lobbyUI.SetActive(false);
            top = 0;
            //Which Game To Be Loaded
            if (!TriviaGame)
            {
               // if (lobbyUI.activeInHierarchy == false)
               // {
                    gameUI.SetActive(true);
                    //questionBox.SetActive(true);
                    Invoke("BottomGameUIEnable",3f);
                    //Load the puzzle controller gameobject and the script for the questions
                    PuzzleController.SetActive(true);
               // }
                
            }

            if (TriviaGame)
            {
                

                //Invoke("TriviaPanelsLoad", 0.5f);

            }
        
            CancelInvoke("LoadLobby2");
        }

        void BottomGameUIEnable()
        {
            Debug.Log("Keyboard On");
            keyboardOn = true;
            FindObjectOfType<WordPuzzleController_Climbing>().setKeyboardOn();
        }

        public bool keyboardOn;
        
        public void RoundFinish()
        {
           
            //FindObjectOfType<PlayfabManager>().SendLeaderBoard(playerName.text,r1C,numberOfTrophies);
            keyboardOn = false;
            progressBar.SetActive(false);
            trphyImage.SetActive(false);
            
            isNextRoundStarted = false;
            gameUI.SetActive(false);
            //questionBox.SetActive(false);
            
            
            foreach (var a in triviaCategories)
            {
                a.SetActive(false);
            }

            //RoundEnd Screen Setups

            if (roundNumber == 2)
            {
                roundCountText.fontSize = 96;
                roundWonText.gameObject.SetActive(false);
                roundCountText.text = "Final" + "\n" +"Round" ;
                
            }
            else
            {
                roundWonText.gameObject.SetActive(true);
                roundCountText.text = (roundNumber+1).ToString();
            }
            
            roundWinScreen.SetActive(true);
            StartCoroutine("roundFinishtimerCountDown");
        
        
        }

        IEnumerator roundFinishtimerCountDown()
        {
        
            if (roundWinScreen)
            {
            
                roundEndTimerText.text = roundEndTimer.ToString() + " s";
                yield return new WaitForSeconds(1f);
            
                while (roundEndTimer > 0)
                {
                    roundEndTimerText.text = roundEndTimer.ToString() + " s";
                    yield return new WaitForSeconds(1f);
                    roundEndTimer--;
                }
                
                transitionScreen.SetActive(true);
                yield return new WaitForSeconds(1f);
        
                roundWinScreen.SetActive(false);
               
                Destroy(topPart);
                //Next round setup
                StartCoroutine("WaitAndCallLobby");
            
            }
        
        }
        

        public void NextRoundBtn()
        {
            progressBar.SetActive(false);
            trphyImage.SetActive(false);
            
            isNextRoundStarted = false;
            gameUI.SetActive(false);
            //questionBox.SetActive(false);
            
            TriviaCanvas.SetActive(false);
            //TriviaManager.SetActive(false);
            
            
            roundWinScreen.SetActive(false);
            transitionScreen.SetActive(true);
            StopCoroutine("roundFinishtimerCountDown");
            isNextRoundStarted = true;
            Destroy(topPart);
        
            StartCoroutine("WaitAndCallLobby");
        }

        IEnumerator WaitAndCallLobby()
        {
            yield return new WaitForSeconds(2f);
            transitionScreen.GetComponent<Animator>().SetTrigger("Out");
            yield return new WaitForSeconds(0.3f);
            
            
            Invoke("StartRoundIni",2.4f);
            countDownTimer = 3;
        
            transitionScreen.SetActive(false);
            RepeatCallLobby();
        }
        public void MatchFinish()
        {
            if (!gameCompleteCalled)
            {
                try
                {
                    sendLevelEnd(MatchNumber,roundNumber,"win");
                }
                catch (Exception e)
                {
                    Debug.LogError("Sending event issue" + e);
                }
                
                
                gameCompleteCalled = true;
                Debug.Log("match finish called");
                progressBar.SetActive(false);
                trphyImage.SetActive(false);
            
                gameUI.SetActive(false);
                //questionBox.SetActive(false);
                
                resultUI.SetActive(true);
                totalCorrect = r1C + r2C + r3C;
                FindObjectOfType<PlayfabManager>().SendLeaderBoard(playerName.text,totalCorrect,numberOfTrophies);
                
                foreach (var a in triviaCategories)
                {
                    a.SetActive(false);
                }
                //TriviaManager.SetActive(false);
                TriviaCanvas.SetActive(false);
               // MatchNumber++;
            }
            
            
        }

        public void ResetComplete()
        {
            gameCompleteCalled = false;
        }
    
        public void MatchLost()
        {
            if (!gameCompleteCalled)
            {
                try
                {
                    sendLevelEnd(MatchNumber,roundNumber,"lose");
                }
                catch (Exception e)
                {
                    Debug.LogError("Sending event issue" + e);
                }
                
                
                roundLossNumber.text = roundNumber.ToString();
                Debug.Log("lose screen");
                
                progressBar.SetActive(false);
                trphyImage.SetActive(false);
                
                
                loseScreen.SetActive(true);
                gameUI.SetActive(false);
                //questionBox.SetActive(false);
                
                foreach (var a in triviaCategories)
                {
                    a.SetActive(false);
                }
               // TriviaManager.SetActive(false);
                TriviaCanvas.SetActive(false);
                gameCompleteCalled = true;
            }
        
        }
        
        public void CalculateAccuracy()
        {
            accuracy[roundNumber-1] = ((float)rightQ[roundNumber-1]/(float)attemptQ[roundNumber-1]) * 100;
        }

       public void ResultScreenUpdate()
        {
            
            attemptText[roundNumber-1].text = attemptQ[roundNumber-1].ToString();
            rightText[roundNumber-1].text = rightQ[roundNumber-1].ToString();
            wrongText[roundNumber-1].text = wrongQ[roundNumber-1].ToString();
            
            
            accuracyText[roundNumber-1].text =accuracy[roundNumber-1].ToString("F0") + "%";
            
        }
        
        public void Retry()
        {
            SceneManager.LoadScene(0);
            PlayerPrefs.SetInt("retry", 1);
            
        }


    public void NewMatch()
    {
        PlayerPrefs.SetInt("retry", 1);
        MatchNumber++;
        PlayerPrefs.SetInt("matchnumber", MatchNumber);
        SceneManager.LoadScene(0);
    }
        
        

        IEnumerator checkCrown()
        {
            while (true)
            {
                yield return null;
                
                
                var players = FindObjectsOfType<PlayerControls_2>();
                if (players.Length>0)
                {
                    foreach (var a in players)
                    {
                        if (top <= a.Yval)
                        {
                            if (a.Yval > 0)
                            {
                                top = a.Yval;
                                a.enableCrown(true);  
                            }
                            
                        }
                        else
                        {
                            a.enableCrown(false);
                        }
                        
                    }

                    yield return new WaitForSeconds(0.9f);
                }
            }
        }
        
        
        
    }

[System.Serializable]
public class EventData
{
   
    public int matchNo;
    public int RoundNo;
    public string status;
}
    

