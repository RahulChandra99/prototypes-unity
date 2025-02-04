using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialButton : MonoBehaviour
{
    public GameObject platform1;

    private void Start()
    {
        platform1.GetComponent<Animator>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            platform1.GetComponent<Animator>().enabled = true;

        }
    }
}
