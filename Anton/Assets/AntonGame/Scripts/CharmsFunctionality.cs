using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmsFunctionality : MonoBehaviour
{
    public static float charmNumber;
    public float count = 0;

    public GameObject abilityBar;

    

    private void Start()
    {
        abilityBar.SetActive(false);

    }
    private void Update()
    {
        if(charmNumber == 1 && count != 1)
        {
            
            //doubleJumpacivated
            if(Input.GetMouseButtonDown(1))
            {
                //ability
                abilityBar.SetActive(true);
                FindObjectOfType<AntonPlayerController>().jumpPower = 1200;
                //abiliy used
                Debug.Log("ability activated");
                
                
            }
            
        }

        
            



    }

}
