using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Climbing_Scripts.Type_of_Games;
using Random = UnityEngine.Random;

public class PlayerControls_2 : MonoBehaviour
{
    
    public bool isAI;
    public delegate void onPlayerDoneClimbing();
    public  event onPlayerDoneClimbing playerdone;
    public CoreData assets;
    int myCorrectAnswers;
    float randomTime;
    public float speed = .1f;
    
    PlayableDirector director;

    public GameObject[] AvatarPrefabs;
    public Transform spawnPlace;
   
    public TimelineAsset [] timelineAssets;
    public string changeTheBindingName= "Animation Track(1)";
    GameObject model;
    public List<GameObject> trackList = new List<GameObject>();
    public UnityEvent E_StartClimbing,StopClimbing;

    public GameObject slider;
    public Transform place;
    public Slider mySlider;
    public int side;
    bool gotmyslider;
    public TMPro.TextMeshProUGUI AIScoreText;

    public GameObject rockTop;
    public GameObject typingUi;

    public TextMeshProUGUI myName;

    private float[] arr = new float[]{0.5f,1f,1.5f,2f,2.5f};
    
    public List<string> PlayerNames;  //your list of playernames
    int playerindex;
    
    [Header("Round Settings")]
    public float Yval;
    public float round1Height;
    public float round2Height;
    public float round3Height;
    public GameObject temp;

    public GameObject[] crownPrefab;
    public Transform crownPosition;
    private GameObject currentCrown;

    public GameObject playerUI;
    public GameObject ai1UI;
    public GameObject ai2UI;
    public Material WrongAnswerMat, RightAnswerMat;
    public UnityEvent rightAnswer, WrongAnswer;
    public UnityEvent OnPlayerWin, OnPlayerLose;
    [Header("Univeral Events Trigger")]
    public string RightAnswerEventName;
    public string WrongAnswerEventName;
    public GameObject head;
    public GameObject stickmanFace;

    void OnEnable()
    {
        //Progress Bar
        GM_Climbing.Instance.progressBar.SetActive(false);
        GM_Climbing.Instance.trphyImage.SetActive(false);
        
        //Set the names
        if (!isAI)
        {
            myName.text = PlayerPrefs.GetString("playername");
        }

        if (isAI)
        {
            myName.text = getplayerName();
        }
    }
    void Awake()
    {
        PlayerNames = PlayerNames.OrderBy(i => Guid.NewGuid()).ToList();
        
        director = GetComponent<PlayableDirector>();
        if (!isAI)
        {
            model = Instantiate(AvatarPrefabs[GM_Climbing.Instance.index], spawnPlace);
        }
        else
        {
            model = Instantiate(AvatarPrefabs[Random.Range(0, AvatarPrefabs.Length - 1)], spawnPlace);
        }

        head = GetComponentInChildren<head>().gameObject;
        currentCrown = Instantiate(crownPrefab[GM_Climbing.Instance.roundNumber - 1], head.transform);
        currentCrown.SetActive(false);
        try
        {
            stickmanFace = model.transform.GetChild(0).Find("Stickman_face").gameObject;
        }
        catch
        {

        }
        trackList.Add(model);
        setBindingOnPlayer();
        DisableDynamicsOfPLayer();

        if (typingUi)
        {
            typingUi.SetActive(false);  
        }
        
        playerUI.SetActive(true);
        ai1UI.SetActive(true);
        ai2UI.SetActive(true);
        
    }
    
    void Start()
    {
        
        Yval = transform.position.y;
        if (isAI)
        {
            Invoke("StartClimbing", UnityEngine.Random.Range(assets.intitalAiTime,assets.intitalAITimeMax));
        }
        // transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Animator>().enabled = false;
    }
    
    
    
    void StartClimbing()
    {
        AIClimbingTimeSetup();
        InvokeRepeating("climb", randomTime, randomTime);
    }
    
    //AI Climbing time values
    void AIClimbingTimeSetup()
    {
        if (GM_Climbing.Instance.roundNumber == 1)
        {
            randomTime = UnityEngine.Random.Range(assets.timeStartValue1, assets.timeEndValue1 + 1);
        }
            
        if (GM_Climbing.Instance.roundNumber == 2)
        {
            randomTime = UnityEngine.Random.Range(assets.timeStartValue2, assets.timeEndValue2 + 1);
        }
        if (GM_Climbing.Instance.roundNumber == 3)
        {
            randomTime = UnityEngine.Random.Range(assets.timeStartValue3, assets.timeEndValue3 + 1);
        }
    }
    
    
    
    void climb()
    {
        //Debug.Log("Calling climb");
        if (isAI)
        {
            Yval += arr[Random.Range(0, arr.Length - 1)];

            AIClimbingTimeSetup();

            myCorrectAnswers++;
            var a = Yval * 100;
            if (AIScoreText)
            {
                if(Yval <= 0)
                {
                    AIScoreText.text = "";
                }
                else
                {
                    AIScoreText.text = String.Format("{0:C0}",a);
                }
            }
        }

       
      
        if (!isAI)
        {
            // Invoke("CompleteClimbing", 2);
            rightAnswer.Invoke();
        }
        
       

        
        StartCoroutine(UpdatePos());
        
    }

    public void Won()
    {
        
//        Debug.Log("calling win");
        if (!GM_Climbing.Instance.TriviaGame) //used only when keyboard game is active
        {
            FindObjectOfType<WordPuzzleController_Climbing>().TurnOffKeyboard();
        } 
        
        if (!isAI)
        {
            var a = FindObjectsOfType<PlayerControls_2>();
            foreach (var x in a)
            {
                if (x != this)
                {
                    x.EnablePlayerDynamics();
                    //Destroy(x.gameObject, 1);
                }
                DestroyImmediate(x.temp);
            }
        }

        if (isAI)
        {
            var a = FindObjectsOfType<PlayerControls_2>();
            foreach(var x in a)
            {
                if(x != this)
                {
                    x.EnablePlayerDynamics();
                       
                }
            }
        }
        
        if (isAI)
        {
            //When Player Loses 
            Debug.Log("You Lost");
            OnPlayerLose.Invoke();
            FindObjectOfType<RoundManager>().Die();
            GM_Climbing.Instance.MatchLost();
            
        }
        
        this.GetComponent<PlayableDirector>().Play(assets.playerWin);
        rockTop.SetActive(true);
        
       
        if(!isAI)
        {
            OnPlayerWin.Invoke();
            Invoke("PlayerWin", 0f);
        }

        playerUI.SetActive(false);
        ai1UI.SetActive(false);
        ai2UI.SetActive(false);
        
        
    }
    
    void PlayerWin()
    {
       
        if (GM_Climbing.Instance.TriviaGame)
        {
            var a = FindObjectOfType<TriviaManager>();
            if (a)
            {
                a.NextRoundPrep();
            }
            var b = FindObjectOfType<WordOrderManager>();
            if (b)
            {
                b.NextRoundPrep();
            }
            var c = FindObjectOfType<EmojiTwoManager>();
            if (c)
            {
                c.NextRoundPrep();
            }
            var d = FindObjectOfType<EmojiOneManager>();
            if (d)
            {
                d.NextRoundPrep();
            }
            var e = FindObjectOfType<EmojiThreeGM>();
            if (e)
            {
               // e.NextRoundPrep();
            }
            
        }
        else
        {
            FindObjectOfType<WordPuzzleController_Climbing>().NextRoundPrep();
        }
        
    }
    
    

    void GetSlider()
    {
        var b = GameObject.FindWithTag("ProgressBar");
        
        if (b)
        {
            place = GameObject.FindWithTag("ProgressBar").transform;

            if (place)
                
            {
                if (!mySlider)
                {
                    
                
                var a = Instantiate(slider, place);
                mySlider = a.transform.GetChild(0).GetComponent<Slider>();
                    temp = a;
                    gotmyslider = true;
                    switch (side)
                    {
                        case -1:
                            if (GM_Climbing.Instance.roundNumber == 1)
                            {
                                mySlider.maxValue = round1Height;
                            }
                            if (GM_Climbing.Instance.roundNumber == 2)
                            {
                                mySlider.maxValue = round2Height;
                            }
                            if (GM_Climbing.Instance.roundNumber == 3)
                            {
                                mySlider.maxValue = round3Height;
                            }
                           
                            mySlider.transform.position = new Vector3(mySlider.transform.position.x-10.5f,mySlider.transform.position.y,mySlider.transform.position.z);
                            mySlider.transform.GetChild(0).GetComponent<Image>().enabled = false;
                            mySlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.black;
                            mySlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
                            break;
                        case 0:
                            if (GM_Climbing.Instance.roundNumber == 1)
                            {
                                mySlider.maxValue = round1Height;
                            }
                            if (GM_Climbing.Instance.roundNumber == 2)
                            {
                                mySlider.maxValue = round2Height;
                            }
                            if (GM_Climbing.Instance.roundNumber == 3)
                            {
                                mySlider.maxValue = round3Height;
                            }
                            mySlider.transform.position = new Vector3(mySlider.transform.position.x,mySlider.transform.position.y,mySlider.transform.position.z);
                            break;
                        case 1:
                            if (GM_Climbing.Instance.roundNumber == 1)
                            {
                                mySlider.maxValue = round1Height;
                            }
                            if (GM_Climbing.Instance.roundNumber == 2)
                            {
                                mySlider.maxValue = round2Height;
                            }
                            if (GM_Climbing.Instance.roundNumber == 3)
                            {
                                mySlider.maxValue = round3Height;
                            }
                            mySlider.transform.position = new Vector3(mySlider.transform.position.x+10.5f,mySlider.transform.position.y,mySlider.transform.position.z);
                            mySlider.transform.GetChild(0).GetComponent<Image>().enabled = false;
                            mySlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = Color.black;
                            mySlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().enabled = false;
                            break;
                    }

                    if (!isAI)
                    {
                        Invoke("setLastdelayed", .5f);
                    }
                    
                }
                
            }
            
        }
        
        
    }
   
    void setUIlastSibling(GameObject g)
    {
        temp = g;
        Invoke("setLastdelayed", .1f);
    }

   void setLastdelayed()
    {
        //Debug.Log("set last sibling");
       temp.transform.SetSiblingIndex(9);
    }

   int ai1Score;
   int ai2Score;
   int playerScore;
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAI)
                //climb();
                onPause();
            
            temp.transform.SetAsLastSibling();
        }
        if (!gotmyslider)
        {
            GetSlider();
            Invoke("SetSliderActive", 6f);
            
        }

        if (!rockTop)
        {
            var top = GameObject.FindWithTag("RockTop");
            if(top) 
            {
                rockTop = top.transform.GetChild(3).gameObject; 
            }
           
        /*    if (GM_Climbing.Instance.roundNumber == 1)
            {
                
                round1Height =QBTrivia.instance.GetLengthOfQuestions(GM_Climbing.Instance.roundNumber-1) + 6;
            }
            
            if (GM_Climbing.Instance.roundNumber == 2)
            {
                
                round2Height =QBTrivia.instance.GetLengthOfQuestions(GM_Climbing.Instance.roundNumber-1) + 6;
            }
            
            if (GM_Climbing.Instance.roundNumber == 3)
            {
                
                round3Height =QBTrivia.instance.GetLengthOfQuestions(GM_Climbing.Instance.roundNumber-1) + 6;
            }*/
        }
        
        
        

        

      /*  playerScore = int.Parse(GameObject.FindWithTag("PlayerScore").GetComponent<TextMeshProUGUI>().text);
        ai1Score = int.Parse(GameObject.FindWithTag("AiScore1").GetComponent<TextMeshProUGUI>().text);
        ai2Score = int.Parse(GameObject.FindWithTag("AiScore2").GetComponent<TextMeshProUGUI>().text);

        int maxValue = Mathf.Max(playerScore, ai1Score, ai2Score);

        
        if (maxValue == playerScore)
        {
            Instantiate(GM_Climbing.Instance.crowns[GM_Climbing.Instance.roundNumber - 1], this.crownPosition);
        }
        */

    }

    void SetSliderActive()
    {
        GM_Climbing.Instance.progressBar.SetActive(true);
        GM_Climbing.Instance.trphyImage.SetActive(true);
    }

    bool win;
   public void DisableDynamicsOfPLayer()
    {
        var a = model.GetComponentsInChildren<Rigidbody>();
        foreach(var x in a)
        {
            x.isKinematic = true;
        }
    }


   public void EnablePlayerDynamics()
    {
        var a = model.GetComponentsInChildren<Rigidbody>();
        foreach (var x in a)
        {
            x.isKinematic = false;
        }
        Destroy(temp);
        if (mySlider)
        {
            Destroy(mySlider.gameObject);
        }
        
        model.GetComponent<Animator>().enabled = false;

        CancelInvoke("climb");
        this.enabled = false;
        
    }

    IEnumerator UpdatePos()
    {
        GM_Climbing.Instance.crownChange();
        
        E_StartClimbing.Invoke();
        //director.Play(timelineAssets[1]);

        if (isAI)
        {
            DisableTyping();
        }
        
        
        while (trackList[0].transform.position.y<Yval)
        {
            mySlider.value = trackList[0].transform.position.y;

            if (GM_Climbing.Instance.roundNumber == 1)
            {
                if (trackList[0].transform.position.y >= round1Height)
                {
                    Yval = round1Height;
                } 
            }
            
            if (GM_Climbing.Instance.roundNumber == 2)
            {
                if (trackList[0].transform.position.y >= round2Height)
                {
                    Yval = round2Height;
                } 
            }
            
            if (GM_Climbing.Instance.roundNumber == 3)
            {
                if (trackList[0].transform.position.y >= round3Height)
                {
                    Yval = round3Height;
                } 
            }
                
            
                // Debug.Log("climbing man");
                yield return null;
        }
        onPause();
        StopClimbing.Invoke();
        director.Play(timelineAssets[1]);
        if (!isAI)
        {
            CompleteClimbing();
        }

        if (GM_Climbing.Instance.roundNumber == 1)
        {
            if (trackList[0].transform.position.y >= round1Height)
            {
                Yval = round1Height;
                trackList[0].transform.position = new Vector3(transform.position.x, Yval, transform.position.z);

                GM_Climbing.Instance.CalculateAccuracy();

                Invoke("Won", .02f);

            }  
        }
        
        if (GM_Climbing.Instance.roundNumber == 2)
        {
            if (trackList[0].transform.position.y >= round2Height)
            {
                Yval = round2Height;
                trackList[0].transform.position = new Vector3(transform.position.x, Yval, transform.position.z);

                GM_Climbing.Instance.CalculateAccuracy();

                Invoke("Won", .02f);

            }  
        }
        
        if (GM_Climbing.Instance.roundNumber == 3)
        {
            if (trackList[0].transform.position.y >= round3Height)
            {
                Yval = round3Height;
                trackList[0].transform.position = new Vector3(transform.position.x, Yval, transform.position.z);

                GM_Climbing.Instance.CalculateAccuracy();

                Invoke("Won", .02f);

            }  
        }

          
    }

    void onPause()
    {
        
            if (isAI)
            {
               Invoke("EnableTyping",0.5f);
            }
        
        director.playableAsset = timelineAssets[1];
        var outputs = director.playableAsset.outputs;

        foreach (var itm in outputs)
        {

          //  Debug.Log(itm.streamName);

            if (itm.streamName.Contains(changeTheBindingName))
            {
                //Debug.Log("done");
                director.SetGenericBinding(itm.sourceObject, trackList[1].GetComponent<Animator>());
            }
        }
    }

    public void EnableTyping()
    {
        typingUi.SetActive(true);
    }
    
    public void DisableTyping()
    {
        typingUi.SetActive(false);
    }
    

    
    

    

    void CompleteClimbing()
    {
       
        
        if (playerdone != null)
        {
            playerdone();
        }
        else
        {
            
            var a = FindObjectOfType<WordPuzzleController_Climbing>();
            if (a)
            {
                a.QuestionGenerationBasedOnRoundNumber();
            }

            var b = FindObjectOfType<TriviaManager>();
            if (b)
            {
                b.ResetQuestion();
            }
        }
        
    }


    

    


    public void CorrectAnswer(int points)
    {
        
        Yval += points * 0.01f;
        
        Invoke("climb", 0f);
        if (RightAnswerEventName.Length > 0)
        {
            Event_Listener.instance.InvokeEvent(RightAnswerEventName);
        }

        if (stickmanFace)
        {
            if (RightAnswerMat)
            {
                if (stickmanFace.GetComponent<SkinnedMeshRenderer>())
                {
                    stickmanFace.GetComponent<SkinnedMeshRenderer>().material = RightAnswerMat;
                }
            }
        }

    }

    public void WrongAnswerFeedback()
    {
        WrongAnswer.Invoke();
        if(WrongAnswerEventName.Length > 0)
        {
            Event_Listener.instance.InvokeEvent(WrongAnswerEventName);
        }

        if (stickmanFace)
        {
            if (RightAnswerMat)
            {
                if (stickmanFace.GetComponent<SkinnedMeshRenderer>())
                {
                    stickmanFace.GetComponent<SkinnedMeshRenderer>().material = WrongAnswerMat;
                }
            }
        }
    }

    void setBindingOnPlayer()
    {
       
        foreach (var x in timelineAssets)
        {
            director.playableAsset = x;
            var outputs = director.playableAsset.outputs;

            foreach (var itm in outputs)
            {

               //Debug.Log(itm.streamName);

                if (itm.streamName.Contains(changeTheBindingName))
                {
                    //Debug.Log("done");
                    director.SetGenericBinding(itm.sourceObject, trackList[1].GetComponent<Animator>());
                }
            }
        }
    }
    
    public string getplayerName() //call this on start of AI player Controls 2
    {
        return PlayerNames[playerindex];
        playerindex++; 
        
    }

    public void enableCrown(bool value)
    {
        currentCrown.SetActive(value);
    }

    private void OnDisable()
    {
        if(rockTop)
        rockTop.SetActive(false);
    }
}


[System.Serializable]
public class CoreData
{
    public TimelineAsset player;
    public TimelineAsset playerWin;
    public float intitalAiTime = 2f;
    public float intitalAITimeMax = 6f;
    [Header("Round 1 AIValues")] 
    public float timeStartValue1;
    public float timeEndValue1;
    [Header("Round 2 AIValues")] 
    public float timeStartValue2; 
    public float timeEndValue2;
    [Header("Round 3 AIValues")] 
    public float timeStartValue3;
    public float timeEndValue3;
}
