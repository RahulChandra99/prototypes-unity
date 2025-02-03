using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using puzzleIO;
using TMPro;
using UnityEngine;

public class GM : MonoBehaviour
{
    [Header("Game UI Panels")] 
    public GameObject mainMenuUI;
    public GameObject gameUI;
    public GameObject resultUI;
    public GameObject lobbyUI;

    [Header("Lobby")]
    public TextMeshProUGUI roundCountText;
    public int roundNumber;
    public TextMeshProUGUI playerCounter;
    public int totalPlayersInLobby;
    public float countingToTotalPlayers;

    [Header("TopPart")]
    public CinemachineTargetGroup cmTG;
    public GameObject topPart;
    public GameObject bigPlatform;
    public GameObject player;
    public GameObject AIBots;
    public GameObject smallPlatform;
    
    [Header("Result Screen")] 
    public TextMeshProUGUI correctAns;
    public TextMeshProUGUI wrongAns;
    public TextMeshProUGUI time;

    public GameObject PuzzleController;

    [Header("Game Bool Variables")]
    public bool isGameStarted;
    
    
    #region SINGLETON PATTERN
    public static GM _instance;
    public static GM Instance
    {
        get {
            if (_instance == null)
            {
               // _instance = GameObject.FindObjectOfType<GM>();
             
               // if (_instance == null)
               // {
               //     GameObject container = new GameObject("GM");
               //     _instance = container.AddComponent<GM>();
               // }
            }
     
            return _instance;
        }
    }
    #endregion
    private void Awake()
    {
        //Game Start Panels load
        mainMenuUI.SetActive(true);
        gameUI.SetActive(false);
        resultUI.SetActive(false);
        lobbyUI.SetActive(false);
        
        //GameIni
        isGameStarted = false;
        roundNumber = 1;
        totalPlayersInLobby = 64;
        countingToTotalPlayers = 0;

        PuzzleController.SetActive(false);



    }

    public void PlayBtn()
    {
        isGameStarted = true;
        
        RepeatCallLobby();
    }

    public void RepeatCallLobby()
    {
        InvokeRepeating("LoadLobby",0,0.01f);
        
    }

    public void LoadLobby()
    {
       
        lobbyUI.SetActive(true);
        if (roundNumber == 1)
        {
            roundCountText.text = "Round " + roundNumber;
            if (countingToTotalPlayers <= totalPlayersInLobby)
            {
                countingToTotalPlayers += 7 * Time.deltaTime; 
                playerCounter.text = (int)countingToTotalPlayers + "/" + totalPlayersInLobby;
            }
            else
            {
                
                //GamePanel Load for Round 1
                lobbyUI.SetActive(false);
                gameUI.SetActive(true);
                //Call the top Part function
                TopPartLoad();
                //Load the puzzle controller gameobject and the script for the questions
                PuzzleController.SetActive(true);
                CancelInvoke("LoadLobby");
            }
            
        }
        
        if (roundNumber == 2)
        {
            roundCountText.text = "Round " + roundNumber;
            if (countingToTotalPlayers <= totalPlayersInLobby)
            {
                countingToTotalPlayers += 2 * Time.deltaTime; 
                playerCounter.text = (int)countingToTotalPlayers + "/" + totalPlayersInLobby;
            }
            else
            {
                Debug.Log("Calling round 2 lobby");
                //GamePanel Load for Round 1
                lobbyUI.SetActive(false);
                gameUI.SetActive(true);
                //Call the top Part function
                TopPartLoad();
                //Load the puzzle controller gameobject and the script for the questions
                PuzzleController.SetActive(true);
                CancelInvoke("LoadLobby");
            }
            
        }
        
        if (roundNumber == 3)
        {
            roundCountText.text = "Round " + roundNumber;
            if (countingToTotalPlayers <= totalPlayersInLobby)
            {
                countingToTotalPlayers += 1 * Time.deltaTime; 
                playerCounter.text = (int)countingToTotalPlayers + "/" + totalPlayersInLobby;
            }
            else
            {
                Debug.Log("Calling round 3 lobby");
                //GamePanel Load for Round 3
                lobbyUI.SetActive(false);
                gameUI.SetActive(true);
                //Call the top Part function
                TopPartLoad();
                //Load the puzzle controller gameobject and the script for the questions
                PuzzleController.SetActive(true);
                CancelInvoke("LoadLobby");
            }
            
        }
        
       
    }

    public void TopPartLoad()
    {
        if (topPart == null)
        {
            topPart = new GameObject("Top Part");
            //Load the initial hide animation
            PlayerSpawn();
            AISpawn();
        }
    }
    
    void PlayerSpawn()
    {
        if (topPart)
        {
            
            Instantiate(bigPlatform, new Vector3(0f, 0f, 0f), Quaternion.identity, topPart.transform);
            
            //GameObject.FindWithTag("BigPlatform").GetComponent<Animator>().SetTrigger("Hide");
            
            var playa = Instantiate(player, new Vector3(0.75f, 0f, 0f), Quaternion.identity, topPart.transform);
           // cmTG.m_Targets[1].target = playa.transform;
            
            PC.Instance.SpawnPlatform();

        }
       
    }

   
    
    void AISpawn()
    {
          for (int i = 0; i < 4; i++)
            {
                if (i != 1)
                {
                    
                    var Bot = Instantiate(AIBots, new Vector3(2.25f-(i*1.5f), 0f, 0f), Quaternion.identity,GM.Instance.topPart.transform);
                    float xvalue = 2.25f - (i * 1.5f);
                    AIC.Instance.Xval = xvalue; // values of Xside
                    cmTG.m_Targets[i].target = Bot.transform;
                    AIC.Instance.SpawnPlatform();
                }
                    
            }
         
    }
    
    public void NextRoundBtn()
    {
        resultUI.SetActive(false);
        PuzzleController.SetActive(true);
        RepeatCallLobby();
        
    }
}
