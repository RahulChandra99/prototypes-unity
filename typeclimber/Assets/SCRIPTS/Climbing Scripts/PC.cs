using System;
using System.Collections;
using System.Collections.Generic;
using puzzleIO;
using UnityEngine;

public class PC : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static PC _instance;
    public static PC Instance
    {
        get {
            if (_instance == null)
            {
                //_instance = GameObject.FindObjectOfType<PC>();
             
               // if (_instance == null)
               // {
              //      GameObject container = new GameObject("PC");
               //     _instance = container.AddComponent<PC>();
               // }
            }
     
            return _instance;
        }
    }
    #endregion
    
    private Collider[] cols;
    public Transform nextMarkerPosition;
    public float jumpForce;
    public float jumpSpeed;
    public float timeSpeed;

    private void Awake()
    {
        PlayerRaggedDollFix();
        Time.timeScale = timeSpeed;
    }
    
    //Ragged Doll fixes
    void PlayerRaggedDollFix()
    {
        //playerGO ragged doll things
        cols = this.GetComponentsInChildren<Collider>();
        Rigidbody[] rbs = this.GetComponentsInChildren<Rigidbody>();
        foreach (Collider x in cols)
        {
            x.enabled = false;
        }
        Rigidbody[] rb = this.GetComponentsInChildren<Rigidbody>();
        foreach(var x in rb)
        {
            x.isKinematic = true;
        }

        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
    
    //Choose from 3 main jumps
  
    

    
    public void SpawnPlatform()
    {
                     
        if (GM.Instance.topPart)
        {
            
            GameObject playaPlatform = null;
            if (nextMarkerPosition)
            {
                playaPlatform = Instantiate(GM.Instance.smallPlatform, new Vector3(nextMarkerPosition.position.x, nextMarkerPosition.position.y-1, nextMarkerPosition.position.z + 1.5f), Quaternion.identity,
                    GM.Instance.topPart.transform);
            }
            else
            {
                playaPlatform = Instantiate(GM.Instance.smallPlatform, new Vector3(0.75f, 0f, 0f), Quaternion.identity,
                    GM.Instance.topPart.transform.transform);
            }
            nextMarkerPosition = playaPlatform.transform;
        }
    }

    //Apply force and jump
    public void JumpForward()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * jumpForce);
        GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x,this.GetComponent<Rigidbody>().velocity.y * jumpSpeed,this.GetComponent<Rigidbody>().velocity.z);
        RandomJump();
    }
    
    public void RandomJump()
    {

        int rand = UnityEngine.Random.Range(1, 4);
      
        if (rand == 1)
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Jump_1");
        if (rand == 2)
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Jump_2");
        if (rand == 3)
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Jump_3");
     
    }
}
