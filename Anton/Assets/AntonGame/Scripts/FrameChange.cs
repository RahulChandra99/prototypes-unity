using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameChange : MonoBehaviour
{
    public GameObject frame1;
    public GameObject frame2;

    


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("ETriggers"))
        {
            if (frame1.activeInHierarchy == true)
            {
                frame1.SetActive(false);
                frame2.SetActive(true);
            }

            else if (frame1.activeInHierarchy == false)
            {
                frame1.SetActive(true);
                frame2.SetActive(false);
            }
        }
        
        
          

           // toDestroy.SetActive(false);
        

        
        

        
    }

    


}
