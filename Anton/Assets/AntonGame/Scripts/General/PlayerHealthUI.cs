using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    AntonPlayerController cc;

    public Sprite whiteHeart;
    public Sprite blackHeart;

    public Image[] hearts;

    public int health;
    //heart containers that will be visible 
    public int numberofhearts;
    
    private void Start()
    {
        cc = FindObjectOfType<AntonPlayerController>(); 
    }

    private void Update()
    {

        if(health>numberofhearts)
        {
            health = numberofhearts;
        }
        for(int i = 0;i<hearts.Length;i++)
        {
            if (i < health)
            {
                hearts[i].sprite = whiteHeart;
            }
            else
            {
                hearts[i].sprite = blackHeart;
            }
            
            if (i < numberofhearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if(cc.healthInc == true)
        {
            health++;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
