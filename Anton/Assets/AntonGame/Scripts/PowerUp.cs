using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
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
        if (collision.gameObject.CompareTag("ToTrigger"))
        {
            Debug.Log("PopUpActive");
            popUpGO.SetActive(true);
            popUp.gameObject.SetActive(true);
            popUp.SetBool("goUp", true);

           
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
                    FindObjectOfType<AntonPlayerController>().enabled = false;
                }
                                
            }
           if (Input.GetKeyDown(KeyCode.Return) && this.gameObject.CompareTag("PowerUp") && ObjectToActivate.activeInHierarchy == true)
            {
                CharmsFunctionality.charmNumber = 1;
                ObjectToActivate.SetActive(false);
                FindObjectOfType<AntonPlayerController>().enabled = true;
                this.gameObject.GetComponent("SpriteRenderer").gameObject.SetActive(false);

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
