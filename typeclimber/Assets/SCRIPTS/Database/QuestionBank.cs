using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using puzzleIO;
using System.Linq;
using System;
using System.IO;

public class QuestionBank : MonoBehaviour
{

    public List<dataJSON> QuestionsBank;
    DATA data;

    public delegate void OnQuestionsComplete();
    public event OnQuestionsComplete changeRound; 
    public static QuestionBank instance
    {
        get;set;
    }

    void Awake()
    {
        instance = this;
        //initializeQuestions();
        //LoadFromFile();

    }
   

    public void SaveQuestion(Questionaires questionData)
    {
       
      //  QuestionsBank.Add(questionData);
        
    }

    public int GetLengthOfQuestions(int MatchIndex,int index)
    {
        return QuestionsBank[MatchIndex].Match[index].Questions.Count-1;
    }

    public void initializeQuestions()
    {
        foreach (var y in QuestionsBank)
        {
            foreach (var x in y.Match)
            {
                x.Questions = x.Questions.OrderBy(i => Guid.NewGuid()).ToList();
            }
        }
        
    }
    int QuestionCounter;
    
    
    public string getQuestion(int match,int round)
    {
        
        if (QuestionCounter < QuestionsBank[match].Match[round].Questions.Count)
        {
            
            var a = QuestionsBank[match].Match[round].Questions[QuestionCounter].Question;
            QuestionCounter++;
            return a;
        }
        else
        {
            QuestionCounter = 0;
            return "";
        }
    }
    

    public void ResetQCounter()
    {
        QuestionCounter = 0;
        
    }

    public bool CheckAnswer(int match,string Answer,int round)
    {
          if (QuestionsBank[match].Match[round].Questions[QuestionCounter-1].Answer.Contains(Answer))
          {
              return true;
          }
          else
          {
              return false;
          }
        
    }
    

    public void saveToFile()
    {
        data = new DATA();
        data.data = QuestionsBank;
        string s = JsonUtility.ToJson(data);
        Debug.Log("<color=#00FF00><b>JSON file: </b></color>  " + s);
        Debug.Log(Application.streamingAssetsPath);
        if (!Directory.Exists(Application.dataPath + "/StreamingAssets"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            File.WriteAllText(Application.streamingAssetsPath + "/DataBase.JSON", s);
        }
       
        File.WriteAllText(Application.streamingAssetsPath + "/DataBase.JSON",s);
        
        
    }


    public void LoadFromFile()
    {
        QuestionsBank = new List<dataJSON>();
        data = new DATA();
        string retreiveData=null;
        if (File.Exists(Application.streamingAssetsPath + "/DataBase.JSON"))
        {
             retreiveData = File.ReadAllText(Application.streamingAssetsPath + "/DataBase.JSON");
        }
        try
        {
            data = JsonUtility.FromJson<DATA>(retreiveData);
        }
        catch
        {
            Debug.LogError("Corrupt Data: " + retreiveData);
        }
        if (data != null)
        {
            QuestionsBank = data.data;
        }

        initializeQuestions();
    }
}

[System.Serializable]
public class Questionaires
{
    public string roundName;
    public  List<QuestionAndAns> Questions;
    
    
}
[System.Serializable]
public class dataJSON
{
    public string MatchName;
    public List<Questionaires> Match;
}

public class DATA
{
    public List<dataJSON> data;
}
