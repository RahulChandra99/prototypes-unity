using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIC : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static AIC _instance;
    public static AIC Instance
    {
        get {
            if (_instance == null)
            {
               // _instance = GameObject.FindObjectOfType<AIC>();
             
               // if (_instance == null)
               // {
               //     GameObject container = new GameObject("AIC");
               //     _instance = container.AddComponent<AIC>();
               // }
            }
     
            return _instance;
        }
    }
    #endregion
    
    public Transform nextMarkerPosition;
    public float Xval; // xpositions from TOpPArtSpawn.cs // get values from spawing the platfrom inside the loop
    private Collider[] cols;
        
    public float jumpSpeed;
    public float jumpForce;
    
    public float timeStartValue;
    public float timeEndValue;
    public float randomTime;
    private void Awake()
    {
        PlayerRaggedDollFix();
            
        randomTime = UnityEngine.Random.Range(timeStartValue, timeEndValue + 1);
        InvokeRepeating("PlatformSpawnNPlayerMove", randomTime, randomTime);
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
    
    public void SpawnPlatform()
    {

        if (GM.Instance.topPart)
        {
            GameObject playaPlatform = null;
            if (nextMarkerPosition)
            {
                playaPlatform = Instantiate(GM.Instance.smallPlatform, new Vector3(nextMarkerPosition.position.x, nextMarkerPosition.position.y - 1, nextMarkerPosition.position.z + 1.5f), Quaternion.identity,
                    GM.Instance.topPart.transform);
            }
            else
            {
                playaPlatform = Instantiate(GM.Instance.smallPlatform,new Vector3(Xval, 0f, 0f), Quaternion.identity, // useage of XValues for platform only
                    GM.Instance.topPart.transform);
            }
            nextMarkerPosition = playaPlatform.transform;
        }
    }
    
    void RandomJump()
    {

        int rand = UnityEngine.Random.Range(1, 4);
      
        if (rand == 1)
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Jump_1");
        if (rand == 2)
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Jump_2");
        if (rand == 3)
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Jump_3");
     
    }
    
    
    
    void Jump()
    {
            
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * jumpForce);
        this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x,this.GetComponent<Rigidbody>().velocity.y * jumpSpeed,this.GetComponent<Rigidbody>().velocity.z);
        RandomJump();
    }
    
    void PlatformSpawnNPlayerMove()
    {
        SpawnPlatform();
        Jump();

        randomTime = UnityEngine.Random.Range(timeStartValue, timeEndValue + 1);
    }
}
