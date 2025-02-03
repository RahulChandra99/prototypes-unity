using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollision2 : MonoBehaviour
{

    public static bool flag = false;

    public AudioSource coinCollect;

    [System.Obsolete]
    void Start()
    {
    }

    void Update()
    {

    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Level2Manager.scoreValue++;
            coinCollect.Play();
            flag = true;
            Destroy(gameObject);
        }
    }
}
