using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateTrigger : MonoBehaviour
{
    Mission1Controller rc;
    public GameObject gateAnim;

    private void Start()
    {
        gateAnim.GetComponent<Animator>().enabled = false;
        rc = FindObjectOfType<Mission1Controller>();
    }
    private void Update()
    {
        if (rc.gateopen == true)
        {
            Debug.Log("hello");
            gateAnim.GetComponent<Animator>().enabled = true;
        }
    }
}
