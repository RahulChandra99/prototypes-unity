using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class LeadboardManager : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    public List<string> PlayerNames;
    private int playerIndex;

    private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTranformList;
    
    private void Awake()
    {
        entryContainer = transform.Find("Container");
        entryTemplate = entryContainer.Find("Template");
        
        entryTemplate.gameObject.SetActive(false);


        highScoreEntryList = new List<HighScoreEntry>()
        {
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "AAA"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "bbb"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "ccc"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "ddd"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "eee"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "fff"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "ggg"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "hhh"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "iii"},
            new HighScoreEntry{ score = UnityEngine.Random.Range(0,3000),name = "jjj"},
        };

        highScoreEntryTranformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry,entryContainer,highScoreEntryTranformList);
        }
    }
    
    
    void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry,Transform container,List<Transform> transformList)
    {
        float templateHeight = 150f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
            
            
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1: rankString = "1ST";
                break;
            case 2: rankString = "2ND";
                break;
            case 3: rankString = "3RD";
                break;
        }

        int score = highScoreEntry.score;
        entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();

        entryTransform.Find("Pos").GetComponent<TextMeshProUGUI>().text = rankString;

        string name = highScoreEntry.name;
        entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;
            
        transformList.Add(entryTransform);
    }

    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
