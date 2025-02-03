using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static string madeUpWord;
    public TextMesh sampleText;

    public int numberOfWords;
    public int maxLengthOfWord;
    
    public static List<string> selectedLetter = new List<string>(){"","","","","","",""};
    
    public static int letterNum = 0;

    public Transform[] Word1;
    public Transform[] Word2;

    private string TestWord1 = "to";
    private string TestWord2 = "not";
    private int wordLength;

    private bool CheckWord;

    private void Update()
    {
        if (madeUpWord != null)
        {
            for (int i = 0; i < numberOfWords; i++)
            {
                if (madeUpWord.Length <= maxLengthOfWord)
                {
                    sampleText.text = madeUpWord;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            wordLength = sampleText.text.Length;

            madeUpWord = "";
            letterNum = 0;
        }
        
        if(wordLength == 2)
        {
            

            if (sampleText.text == TestWord1.ToUpper())
            {
                for (int i = 0; i < sampleText.text.Length; i++)
                {
                    Word1[i].GetComponent<TextMesh>().text = selectedLetter[i+1];
                }

                
            }

           
        }
        
        if(wordLength == 3)
        {
            

            if (sampleText.text == TestWord2.ToUpper())
            {
                for (int i = 0; i < sampleText.text.Length; i++)
                {
                    Word2[i].GetComponent<TextMesh>().text = selectedLetter[i+1];
                }
                
            }
           
        }
        
    }
}
