using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    private void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successfull Login");
    }
    
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard updated");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error");
        Debug.Log(error.GenerateErrorReport());
    }
    
   

    public void GetLeaderboard()
    {
        GM_Climbing._instance.leaderBoardUI.SetActive(true);
        var request = new GetLeaderboardRequest
        {
            StatisticName = "TrophyLeaderBoard",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request,OnLeaderBoardGet,OnError);
    }
    
    public void SendLeaderBoard(string playerName,int score,int trophyCount)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlayerScore",
                    Value = score
                    
                },
                new StatisticUpdate
                {
                    StatisticName = "PlayerTrophyCount",
                    Value = trophyCount
                    
                }
            }
        };
        
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName
        }, result =>
        {
           
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public GameObject rowPrefab;
    public Transform rowsParent;
    
    void OnLeaderBoardGet(GetLeaderboardResult result)
    {
       /* foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }*/
        
        foreach (var item in result.Leaderboard)
        {
            
            
            GameObject newGO = Instantiate(rowPrefab, rowsParent);
            TextMeshProUGUI[] texts = newGO.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (item.Position+1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
            //texts[3].text = item
            
        }
    }

   
}
