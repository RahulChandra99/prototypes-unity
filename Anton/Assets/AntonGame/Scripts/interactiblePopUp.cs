using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactiblePopUp : MonoBehaviour
{
    public Animator popUp;
    public GameObject popUpGO;

    public GameObject ObjectToActivate;

    

   

    private void Start()
    {
        popUpGO.SetActive(false);
        popUp.gameObject.SetActive(false);
        ObjectToActivate.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("ToTrigger"))
        {
            Debug.Log("PopUpActive");
            popUpGO.SetActive(true);
            popUp.gameObject.SetActive(true);
            popUp.SetBool("goUp",true);

           /* if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (ObjectToActivate.activeInHierarchy == false)
                {
                    ObjectToActivate.SetActive(true);
                    FindObjectOfType<AntonPlayerController>().enabled = true;

                }

                else if (ObjectToActivate.activeInHierarchy == true)
                {
                    ObjectToActivate.SetActive(false);
                    FindObjectOfType<AntonPlayerController>().enabled = false;

                }
                */


            }

        }

        
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ETriggers"))
        {
            
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                //play tv audio
                Debug.Log("hello//");
                if (ObjectToActivate.activeInHierarchy == false)
                {
                    
                    ObjectToActivate.SetActive(true);
                   // FindObjectOfType<AntonPlayerController>().enabled = false;
                }
                else if (ObjectToActivate.activeInHierarchy == true)
                {
                    ObjectToActivate.SetActive(false);
                   // FindObjectOfType<AntonPlayerController>().enabled = true;
                }

               
                }

            }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ToTrigger"))
        {
            Debug.Log("PopUpInactive");
            popUp.SetBool("goUp", false);
        }
    }

    private void Update()
    {
        
        

    }
}
