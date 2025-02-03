using System;
using System.Collections;
using System.Collections.Generic;
using puzzleIO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AIC_Climbing : MonoBehaviour
{
   [Header("Time variables")] 
   public float timeStartValue;
   public float timeEndValue;
   [SerializeField]private float randomTime;

 
   public TimelineAsset Players;
   public TimelineAsset PlayerWin;

   public float timeAfterStartAI;
   [SerializeField]private int correctAnswers;
   
   private void Awake()
   {
      
      
      StartCoroutine("WaitAndAIStart");

      correctAnswers = 0;

   }

   void AIClimb()
   {
      this.GetComponent<PlayableDirector>().Play(Players);
      this.GetComponent<PlayableDirector>().RebuildGraph();
      randomTime = UnityEngine.Random.Range(timeStartValue, timeEndValue + 1);
      correctAnswers++;
     /* if (correctAnswers == WordPuzzleController_Climbing.totalCorrectAnswers)
      {
         StartCoroutine("WaitAndWin");
         Debug.Log("You Lost");
         
      }*/
   }

   IEnumerator WaitAndAIStart()
   {
      yield return new WaitForSeconds(timeAfterStartAI);
      
      randomTime = UnityEngine.Random.Range(timeStartValue, timeEndValue + 1);
      InvokeRepeating("AIClimb", randomTime, randomTime);
   }

   IEnumerator WaitAndWin()
   {
      yield return new WaitForSeconds(2f);
      this.GetComponent<PlayableDirector>().Play(PlayerWin);
      CancelInvoke("AIClimb");
   }

   
}
