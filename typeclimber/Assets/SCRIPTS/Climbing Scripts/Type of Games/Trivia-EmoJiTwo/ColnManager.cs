using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColnManager : MonoBehaviour
{
    public GameObject[] buttons;
    public Sprite selectedS, deselectedS;
    
    public void DisableAllBtns()
    {
        foreach (var a in buttons)
        {
            a.GetComponent<Image>().sprite = deselectedS;
        }
    }

    public void ActivateButton(int value)
    {
        DisableAllBtns();
        buttons[value].GetComponent<Image>().sprite = selectedS;
    }
}
