using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Frame1Manager : MonoBehaviour
{
    public GameObject envTitle;
    public GameObject torchPanel;

    private void Start()
    {
        envTitle.SetActive(true);
        torchPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("TorchTrigger"))
        {
            torchPanel.SetActive(true);
            //Destroy(envTitle, 20f);
            //Destroy(torchPanel, 20f);
        }
    }
}
