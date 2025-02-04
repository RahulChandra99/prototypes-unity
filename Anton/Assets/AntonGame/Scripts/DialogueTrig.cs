using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrig : MonoBehaviour
{
    public GameObject DS;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DS.SetActive(true);
            FindObjectOfType<AntonPlayerController>().enabled = false;
            this.gameObject.SetActive(false);
        }
    }

    
}
