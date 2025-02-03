using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using puzzleIO;
using System.Linq;
using System;
using System.IO;
using Doozy.Engine.Utils.ColorModels;

public class QBTrivia : MonoBehaviour
{


    public List<MatchesT> matches = new List<MatchesT>();
    public static QBTrivia instance;
   

    void Awake()
    {
        instance = this;

        
    }


    #region MultipleQuestion
    
 /// <summary>
 /// 
 /// </summary>
 /// <param name="matchIndex"> place match index</param>
 /// <param name="roundNo"> place round index</param>
 /// <param name="qNumber"> place question Number</param>
 /// <returns></returns>
 ///
int QuestionCounter;

 public int categoryNumber;

 public void initQuestionMCQ()
 {
     
     //random category
     matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
        .mulitpleChoice.Shuffle(7);
     //random question
        matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
         .mulitpleChoice[categoryNumber].categoryMCQ.Shuffle(matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
             .mulitpleChoice[categoryNumber].categoryMCQ.Count);
 }
  public string getMCQQuestion(int matchIndex,int roundNo,int category,int qNumber)
  {
      return matches[matchIndex].Rounds[roundNo].mulitpleChoice[category].categoryMCQ[qNumber].Question;
      
  }
   
  public List<string> getMCQOptions(int matchIndex, int roundNo,int category, int qNumber)
  {
      return matches[matchIndex].Rounds[roundNo].mulitpleChoice[category].categoryMCQ[qNumber].Answers.ToList();
  }
   
  public bool CheckMCQAnswer(int matchIndex, int roundNo,int category,string Answer,int qnumber)
  {
      if (matches[matchIndex].Rounds[roundNo].mulitpleChoice[category].categoryMCQ[qnumber].correctAns.Contains(Answer))
      {
          return true;
      }
      else
      {
          return false;
      }
 
  }

  #endregion
  
  #region WordOrder
  
   /// <summary>
   /// here you will get the options for your UI
   /// </summary>
   /// <param name="matchIndex">paste your match index</param>
   /// <param name="roundNo"> enter your round number</param>
   /// <param name="qNumber"> enter your question No</param>
   /// <returns></returns>
  public List<string> getWOoptions(int matchIndex, int roundNo, int qNumber)
  {
      return matches[matchIndex].Rounds[roundNo].wordOrder[qNumber].Options.ToList();
  }
   

   public void initQuestionWordOrder()
   {
       matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
           .wordOrder.Shuffle(matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
               .wordOrder.Count);
   }
   
  public bool CheckWOAnswer(int matchIndex, int roundNo, string Answer,int qnumber)
  {
      
      if (string.Compare(matches[matchIndex].Rounds[roundNo].wordOrder[qnumber].correctOrder.ToUpper(), Answer.ToUpper()) == 0)
      {
          return true;
      }
      else
      {
          return false;
      }
 
  }

  #endregion
  
  #region TriviaEmoji1

  public void initQuestionTrivia1()
  {
      matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
          .triviaEmoji1.Shuffle(matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
              .triviaEmoji1.Count);
  }
  
  public Array getTopLetterBoxes(int matchIndex, int roundNo, int qNumber)
  {
      return matches[matchIndex].Rounds[roundNo].triviaEmoji1[qNumber].topLetterInputBoxes;
  }
  

  #endregion
  
  #region TriviaEmoji3

  public void initQuestionTrivia2()
  {
      matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
          .triviaEmoji2.Shuffle(matches[GM_Climbing._instance.MatchNumber].Rounds[GM_Climbing._instance.roundNumber-1]
              .triviaEmoji2.Count);
  }

  public int getEmoji3Index(int matchIndex,int roundnumber,int qNumber)
  {
      return matches[matchIndex].Rounds[roundnumber].triviaEmoji3[qNumber].rowNumber;
  }
  
  public Sprite getEmoji3SpriteRow1(int matchIndex,int roundnumber,int qNumber,int indexArray)
  {
      return matches[matchIndex].Rounds[roundnumber].triviaEmoji3[qNumber].row1Images[indexArray];
  }
  public Sprite getEmoji3SpriteRow2(int matchIndex,int roundnumber,int qNumber,int indexArray)
  {
      return matches[matchIndex].Rounds[roundnumber].triviaEmoji3[qNumber].row2Images[indexArray];
  }
  
  public Sprite getEmoji3SpriteRow3(int matchIndex,int roundnumber,int qNumber,int indexArray)
  {
      return matches[matchIndex].Rounds[roundnumber].triviaEmoji3[qNumber].row3Images[indexArray];
  }
  
  public Sprite getEmoji3SpriteInputImages(int matchIndex,int roundnumber,int qNumber,int indexArray)
  {
      return matches[matchIndex].Rounds[roundnumber].triviaEmoji3[qNumber].inputImages[indexArray];
  }
  
  public String CheckEmoji3Answer(int matchIndex, int roundNo,int qnumber,Sprite spriteName)
  {
      if (matches[matchIndex].Rounds[roundNo].triviaEmoji3[qnumber].correctAnswer.Contains(spriteName))
      {
          return spriteName.name;
      }
      else
      {
          return "";
      }
 
  }
  

  #endregion

}

[System.Serializable]
public class MultipleChoice
{
    public string Question;
    public string[] Answers;
    public string correctAns;
    
}

[System.Serializable]
public class CategorisedMultipleChoice
{
    public string categoryName;
    public List<MultipleChoice> categoryMCQ;
}

[System.Serializable]

public class WordOrder
{
    public string[] Options;
    public string correctOrder;
}

[System.Serializable]
public class TriviaEmoji1
{
    public string[] topLetterInputBoxes;
    public string[] bottomLetterBoxes;
    public List<string> correctAnswer;
    public string CAnswer;
    public int numberOfMissingLetters;
    public Sprite[] emos;
   
}

[System.Serializable]
public class TriviaEmoji2
{
    public String questionText;
    public int numberOfRows;
    public Sprite[] row1Emos;
    public Sprite[] row2Emos;
    public Sprite[] row3Emos;
    public string correctAnsR1;
    public string correctAnsR2;
    public string correctAnsR3;
}

[System.Serializable]
public class TriviaEmoji3
{
    public int rowNumber;
    public Sprite[] row1Images;
    public Sprite[] row2Images;
    public Sprite[] row3Images;
    public Sprite[] correctAnswer;
    public Sprite[] inputImages;
   
}

[System.Serializable]
public class roundsT
{
    public string roundName;
    public List<CategorisedMultipleChoice> mulitpleChoice = new List<CategorisedMultipleChoice>();
    public List<WordOrder> wordOrder = new List<WordOrder>();
    public List<TriviaEmoji1> triviaEmoji1 = new List<TriviaEmoji1>();
    public List<TriviaEmoji2> triviaEmoji2 = new List<TriviaEmoji2>();
    public List<TriviaEmoji3> triviaEmoji3 = new List<TriviaEmoji3>();
}

[System.Serializable]
public class MatchesT
{
    public string MatchName;
    public List<roundsT> Rounds;
}

