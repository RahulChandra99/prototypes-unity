using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvLevelTitle : MonoBehaviour
{
    public GameObject title;

    public GameObject titleTrigger;

    private void Start()
    {
        title.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            title.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
