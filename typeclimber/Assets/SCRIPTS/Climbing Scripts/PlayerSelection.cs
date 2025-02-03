using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSelection : MonoBehaviour
{
    
    public GameObject[] Avatars;
    int index;

    

    private void Awake()
    {
        foreach (var a in Avatars)
        {
            var rb = a.GetComponentsInChildren<Rigidbody>();
            foreach (var x in rb)
            {
                x.isKinematic = true;
            }
        }
    }

    void Start()
    {
        
        
        hideAll();
        index = Random.Range(0, Avatars.Length - 1);
        Avatars[index].SetActive(true);
        GM_Climbing.Instance.index = index;
        
        
    }
    
    public void ChangeLeft()
    {
        if (index > 0)
        {
            index--;
            GM_Climbing.Instance.index = index;
            hideAll();
            Avatars[index].SetActive(true);
        }
        else
        {
            index = Avatars.Length - 1;
            GM_Climbing.Instance.index = index;
            hideAll();
            Avatars[index].SetActive(true);
        }
    }
    public void Changeright()
    {
        if (index < Avatars.Length-1)
        {
            index++;
            GM_Climbing.Instance.index = index;
            hideAll();
            Avatars[index].SetActive(true);
        }
        else
        {
            index = 0;
            GM_Climbing.Instance.index = index;
            hideAll();
            Avatars[index].SetActive(true);
        }
    }
    public void hideAll()
    {
        foreach(var x in Avatars)
        {
            x.SetActive(false);
        }
    }
}
