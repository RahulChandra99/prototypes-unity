using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSelection : MonoBehaviour
{
    private void OnMouseDown()
    {
        
        GameController.madeUpWord += GetComponent<TextMesh>().text;
        GameController.letterNum++;
        GameController.selectedLetter[GameController.letterNum] = GetComponent<TextMesh>().text;

        
        
      
    }
}
