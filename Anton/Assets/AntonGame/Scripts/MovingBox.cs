using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    public GameObject frameName;
    public GameObject frameName2;
    private void Update()
    {
        //if (frameName.activeInHierarchy ==true || frameName2.activeInHierarchy == true)
           // this.gameObject.SetActive(true);
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.transform.parent = transform;
        }

        else if (collision.gameObject.CompareTag("Platform"))
        {

            this.transform.parent = collision.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.transform.parent = null;
        }
        else if(collision.gameObject.CompareTag("Platform"))
        {

            this.transform.parent = collision.transform;
        }
    }
}
