using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace puzzleIO
{
    public class AIController : MonoBehaviour
    {
        public GameObject platformPrefab;
        public Transform nextMarkerPosition;

        public UnityEvent PlayerIdle, OnPlayerLand, OnPlayerJump;
        private Collider[] cols;
        
        public float jumpSpeed;
        public float jumpForce;
        

        public float timeStartValue;
        public float timeEndValue;

        public float randomTime;
        [HideInInspector]
        public GameObject TopParent;
        [HideInInspector]
        public float Xval; // xpositions from TOpPArtSpawn.cs // get values from spawing the platfrom inside the loop




        private void Awake()
        {
            PlayerRaggedDollFix();
            
            randomTime = UnityEngine.Random.Range(timeStartValue, timeEndValue + 1);
            InvokeRepeating("PlatformSpawnNPlayerMove", randomTime, randomTime);
        }


        void Start()
        {
            SpawnPlatform();
        }


       

        //Callback Functions

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
                foreach (var x in rb)
                {
                    x.isKinematic = true;
                }

                this.GetComponent<Collider>().enabled = true;
                this.GetComponent<Rigidbody>().isKinematic = false;
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
            
            void PlatformSpawnNPlayerMove()
            {


           

            SpawnPlatform();
            Jump();

             randomTime = UnityEngine.Random.Range(timeStartValue, timeEndValue + 1);
            }

           
        void Jump()
        {
            
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * jumpForce);
            this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x,this.GetComponent<Rigidbody>().velocity.y * jumpSpeed,this.GetComponent<Rigidbody>().velocity.z);
            RandomJump();
        }

        public void SpawnPlatform()
        {

            if (TopParent)
            {
                Debug.Log("called for spawn platform");
                GameObject playaPlatform = null;
                if (nextMarkerPosition)
                {
                    playaPlatform = Instantiate(platformPrefab, new Vector3(nextMarkerPosition.position.x, nextMarkerPosition.position.y - 1, nextMarkerPosition.position.z + 1.5f), Quaternion.identity,
                    TopParent.transform);
                }
                else
                {
                    playaPlatform = Instantiate(platformPrefab, new Vector3(Xval, 0f, 0f), Quaternion.identity, // useage of XValues for platform only
                    TopParent.transform);
                }
                nextMarkerPosition = playaPlatform.transform;
            }
        }
    }
}

        
        
        
        

