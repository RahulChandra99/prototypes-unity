using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCaller : MonoBehaviour
{

    public enum type
    {
        trigger,
        collision
    }

    public type Type;

    private void OnCollisionEnter (Collision other)
    {
        if (other.transform.GetComponent<EventHolder> () && Type == type.collision)
        {
            other.transform.GetComponent<EventHolder> ().Enter ();
        }
    }

    private void OnCollisionExit (Collision other)
    {
        if (other.transform.GetComponent<EventHolder> () && Type == type.collision)
        {
            other.transform.GetComponent<EventHolder> ().Exit ();
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.transform.GetComponent<EventHolder> () && Type == type.trigger)
        {
            other.transform.GetComponent<EventHolder> ().Enter ();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.transform.GetComponent<EventHolder> () && Type == type.trigger)
        {
            other.transform.GetComponent<EventHolder> ().Exit ();
        }
    }
}