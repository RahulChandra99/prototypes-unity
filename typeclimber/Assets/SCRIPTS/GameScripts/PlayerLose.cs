using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLose : MonoBehaviour
{
    public GameObject[] avatars;
    public Transform place;
    public Material FaceMat;
    public string facename;
    
    IEnumerator Start()
    {
        
        var a = Instantiate(avatars[GM_Climbing.Instance.index], place);
        if (FaceMat && a != null)
        {
            var t = a.transform.GetChild(0).Find(facename);
            if (t)
            {
                t.GetComponent<SkinnedMeshRenderer>().material = FaceMat;
            }
            else
            {
                Debug.LogError("child not found");
            }
        }
        yield return new WaitForSeconds(.5f);
        if (a.GetComponent<Animator>())
        {
            a.GetComponent<Animator>().enabled = false;
        }
        




    }


    
}
