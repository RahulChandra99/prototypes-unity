using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Event_Listener : MonoBehaviour
{
    public static Event_Listener instance
    {
        get; set;
    }
    public EventHolders[] events;
   

  

    void Awake()
    {
        //Debug.Log("called");
        instance = this;
        //initializeQuestions();
        //LoadFromFile();

    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }


    public void InvokeEvent(string eventName)
    {
        foreach( var x in events)
        {
            if(x.EventName == eventName)
            {
                x.events.Invoke();
            }
        }
    }
}

[Serializable]
public class EventHolders
{
    public string EventName;
    [Space]
    [Space]
    [Space]
    [Space]
    public UnityEvent events;
}
