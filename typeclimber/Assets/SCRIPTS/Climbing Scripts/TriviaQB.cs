using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using puzzleIO;
using System.Linq;
using System;
using System.IO;
using Doozy.Engine.Utils.ColorModels;

public class TriviaQB : MonoBehaviour
{
    
    public List<difficultyT> LevelOfDiff = new List<difficultyT>();
    public static TriviaQB instance;
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        

    }

    private void Start()
    {
        initQuestionMCQ();
        initQuestionWordOrder();
        initQuestionTrivia1();
        initQuestionTrivia2();
    }

    #region MultipleQuestion
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="qNumber"> place question Number</param>
    /// <returns></returns>
    ///
    int QuestionCounter;

    public int categoryNumber;

    public void initQuestionMCQ()
    {
        for (int i = 0; i < 2; i++)
        {
            //random category for mcq
            LevelOfDiff[i].MCQCategories.Shuffle(7);
            //shuffle the questions
            LevelOfDiff[i].MCQCategories[categoryNumber].categoryMCQ.Shuffle( LevelOfDiff[i].MCQCategories[categoryNumber].categoryMCQ.Count); 
        }
        
    }
    public string getMCQQuestion(int difficultyLvl,int category,int qNumber)
    {
       // return matches[matchIndex].Rounds[roundNo].mulitpleChoice[category].categoryMCQ[qNumber].Question;
      return LevelOfDiff[difficultyLvl].MCQCategories[category].categoryMCQ[qNumber].Question;
    }
   
    public List<string> getMCQOptions(int difficultyLvl,int category, int qNumber)
    {
       // return matches[matchIndex].Rounds[roundNo].mulitpleChoice[category].categoryMCQ[qNumber].Answers.ToList();
       return LevelOfDiff[difficultyLvl].MCQCategories[category].categoryMCQ[qNumber].Answers.ToList();
    }
   
    public bool CheckMCQAnswer(int difficultyLvl,int category,string Answer,int qnumber)
    {
        if (LevelOfDiff[difficultyLvl].MCQCategories[category].categoryMCQ[qnumber].correctAns.Contains(Answer))
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
    /// <param name="qNumber"> enter your question No</param>
    /// <returns></returns>
    public List<string> getWOoptions(int difficultyLvl, int qNumber)
    {
        LevelOfDiff[GM_Climbing._instance.levelDifficulty].wordOrderQs[qNumber].Options.Shuffle( LevelOfDiff[GM_Climbing._instance.levelDifficulty].wordOrderQs[qNumber].Options.Length);
       return LevelOfDiff[difficultyLvl].wordOrderQs[qNumber].Options.ToList();
    }
   

    public void initQuestionWordOrder()
    {
        for (int i = 0; i < 2; i++)
        {
            LevelOfDiff[i].wordOrderQs.Shuffle( LevelOfDiff[i].wordOrderQs.Count);
        }
          
    }
   
    public bool CheckWOAnswer(int difficultyLvl, string Answer,int qnumber)
    {
      
        if (string.Compare(LevelOfDiff[difficultyLvl].wordOrderQs[qnumber].correctOrder.ToUpper(), Answer.ToUpper()) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
 
    }

    #endregion
    
    #region EmojiLetter

  public void initQuestionTrivia1()
  {
      for (int i = 0; i < 2; i++)
      {
          LevelOfDiff[i]
              .emojiLetterQs.Shuffle(LevelOfDiff[i]
                  .emojiLetterQs.Count);
      }
      
  }
  
  public Array getTopLetterBoxes(int difficultyLvl, int qNumber)
  {
      LevelOfDiff[GM_Climbing._instance.levelDifficulty].emojiLetterQs[qNumber].bottomLetterBoxes.Shuffle(LevelOfDiff[GM_Climbing._instance.levelDifficulty].emojiLetterQs[qNumber].bottomLetterBoxes.Length);
      return LevelOfDiff[difficultyLvl].emojiLetterQs[qNumber].topLetterInputBoxes;
  }
  

  #endregion
  
  #region WordEmoji

  public void initQuestionTrivia2()
  {
      for (int i = 0; i < 2; i++)
      {
          LevelOfDiff[i]
              .wordEmojiQs.Shuffle(LevelOfDiff[i]
                  .wordEmojiQs.Count); 
      }
      
  }
  
  public void shuffleImages( int qNumber)
  {
      
      LevelOfDiff[GM_Climbing._instance.levelDifficulty]
          .wordEmojiQs[qNumber].row1Emos.Shuffle(LevelOfDiff[GM_Climbing._instance.levelDifficulty]
              .wordEmojiQs[qNumber].row1Emos.Length);
      
      LevelOfDiff[GM_Climbing._instance.levelDifficulty]
          .wordEmojiQs[qNumber].row1Emos.Shuffle(LevelOfDiff[GM_Climbing._instance.levelDifficulty]
              .wordEmojiQs[qNumber].row2Emos.Length);
      
      LevelOfDiff[GM_Climbing._instance.levelDifficulty]
          .wordEmojiQs[qNumber].row1Emos.Shuffle(LevelOfDiff[GM_Climbing._instance.levelDifficulty]
              .wordEmojiQs[qNumber].row3Emos.Length);
  }
  

  #endregion

 /* #region WordEmoji

  public void initQuestionTrivia2()
  {
      LevelOfDiff[GM_Climbing._instance.levelDifficulty]
          .wordEmojiQs.Shuffle( LevelOfDiff[GM_Climbing._instance.levelDifficulty]
              .wordEmojiQs.Count);
  }

  public int getEmoji3Index(int difficultyLvl,int qNumber)
  {
      return LevelOfDiff[difficultyLvl].Word[qNumber].rowNumber;
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
  

  #endregion*/
    
}

public class MCQ
{
    public string Question;
    public string[] Answers;
    public string correctAns;
    
}

[System.Serializable]
public class CategorisedMCQ
{
    public string categoryName;
    public List<MCQ> categoryMCQ;
}

[System.Serializable]
public class CorrectOrder
{
    public string name;
    public string[] Options;
    public string correctOrder;
}

[System.Serializable]
public class EmojiLetters
{
    public string name;
    public string[] topLetterInputBoxes;
    public string[] bottomLetterBoxes;
    public List<string> correctAnswer;
    public string CAnswer;
    public int numberOfMissingLetters;
    public Sprite[] emos;
   
}

[System.Serializable]
public class WordEmoji
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
public class EmojiEmoji
{
    public int rowNumber;
    public Sprite[] row1Images;
    public Sprite[] row2Images;
    public Sprite[] row3Images;
    public Sprite[] correctAnswer;
    public Sprite[] inputImages;
   
}

[System.Serializable]
public class difficultyT
{
    public string LevelDiff;
    public List<CategorisedMultipleChoice> MCQCategories = new List<CategorisedMultipleChoice>();
    public List<CorrectOrder> wordOrderQs = new List<CorrectOrder>();
    public List<EmojiLetters> emojiLetterQs = new List<EmojiLetters>();
    public List<WordEmoji> wordEmojiQs = new List<WordEmoji>();
    public List<EmojiEmoji> emojiEmojiQs = new List<EmojiEmoji>();
}