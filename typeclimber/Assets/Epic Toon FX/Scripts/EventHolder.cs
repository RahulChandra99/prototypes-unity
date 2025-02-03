using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHolder : MonoBehaviour
{
    public float delay;

    public UnityEvent OnEnter;

    public float delay1;

    public UnityEvent OnExit;

    public void Enter ()
    {
        StartCoroutine (EventTrigger (true));
        Debug.Log("entered called");
    }

    public void Exit ()
    {
        StartCoroutine (EventTrigger ());
        Debug.Log("exit called");
    }

    IEnumerator EventTrigger (bool enter = false)
    {
        if (enter)
        {

            yield return new WaitForSeconds (delay);
            if (OnEnter != null)
                OnEnter.Invoke ();
        }
        else
        {
            yield return new WaitForSeconds (delay1);
            if (OnExit != null)
                OnExit.Invoke ();
        }

    }

}