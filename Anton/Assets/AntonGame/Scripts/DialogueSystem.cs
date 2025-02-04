using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textToDisplay;
    public string[] sentences;
    private int index;
    public float timeBetweenLetters;

    public GameObject continueBtn;
    public GameObject dialoguePanel;
   

    private void Start()
    {
        StartCoroutine(Type());
    }

    private void Update()
    {
        if (textToDisplay.text == sentences[index])
            continueBtn.SetActive(true);
    }
    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textToDisplay.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        
        
    }

    public void NextSentence()
    {
        
        continueBtn.SetActive(false);
        if(index < sentences.Length - 1)
        {
            index++;
            textToDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            continueBtn.SetActive(false);
            textToDisplay.text = "";
            dialoguePanel.SetActive(false);
            FindObjectOfType<AntonPlayerController>().enabled = true;
        }

       
            

        
    }
}
