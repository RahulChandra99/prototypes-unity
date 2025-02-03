using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDriver : MonoBehaviour
{

    public enum startType { Enable,Awake,Start,DelayedStart,DelayedEnabledStart,Disable }
    [Tooltip(" OnEnable - Execute at each time Object Enables, Awake - calls first time on Object Awake, Start - Execute at the First Frame only when object Enables, Delayed start - Execute with delay after Object enabled, DelayedEnabledStart - Execute with delay each time object gets Active")]
    public startType ExecutionMode;

    public List<string> EventNames = new List<string>();
    [Tooltip("Note- Please select delayed attribute above for enabling delay ")]
    public float Delay;
  
    // Start is called before the first frame update
    void Start()
    {
        if(ExecutionMode == startType.Start)
        {
            foreach(var x in EventNames)
            {
                if (x.Length > 0)
                {
                    Event_Listener.instance.InvokeEvent(x);
                }
            }
        }

        if(ExecutionMode == startType.DelayedStart)
        {
            Invoke("DelayedStart", Delay);
        }
    }


    void OnEnable()
    {
        if (ExecutionMode == startType.Enable)
        {
            foreach (var x in EventNames)
            {
                if (x.Length > 0)
                {
                    if (Event_Listener.instance)
                    {
                        Event_Listener.instance.InvokeEvent(x);
                    }
                    else
                    {
                      Invoke("OnEnable",.1f);
                        break;
                    }
                }
            }
        }

        if (ExecutionMode == startType.DelayedEnabledStart)
        {
            Invoke("DelayedStart", Delay);
        }
    }

    void Awake()
    {
        if (ExecutionMode == startType.Awake)
        {
            foreach (var x in EventNames)
            {
                if (x.Length > 0)
                {
                    Event_Listener.instance.InvokeEvent(x);
                }
            }
        }
    }


    void OnDisable()
    {
        if (ExecutionMode == startType.Disable)
        {
            foreach (var x in EventNames)
            {
                if (x.Length > 0)
                {
                    Event_Listener.instance.InvokeEvent(x);
                }
            }
        }
    }

    void DelayedStart()
    {
        foreach (var x in EventNames)
        {
            if (x.Length > 0)
            {
                Event_Listener.instance.InvokeEvent(x);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
