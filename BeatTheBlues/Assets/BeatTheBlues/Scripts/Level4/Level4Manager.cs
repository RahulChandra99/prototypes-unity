using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level4Manager : MonoBehaviour
{
    public Text scoreText;
    public static int scoreValue = 0;

    public Text timer;
    private float startTime;

    private int count = 1;

    private bool finished = false;


    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (coinCollision.flag == true)
        {
            scoreText.text = scoreValue + "/5";
        }

        if(PlayerMovement.myBody.isKinematic == false)
        {
            if (finished)
                return;

            float t = Time.time - startTime;
            
            string seconds = (t % 60).ToString("f2");

            timer.text = seconds;
            count++;
        }

        
    }

    public void Finish()
    {
        finished = true;
        timer.color = Color.cyan;
    }




}
