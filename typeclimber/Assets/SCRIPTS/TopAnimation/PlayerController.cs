
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace puzzleIO
{
    public class PlayerController : MonoBehaviour
    {
        
        
        public GameObject platformPrefab;
        public Transform nextMarkerPosition;

        public UnityEvent OnPlayerLand,OnPlayerJump;
        private Collider[] cols;
        public float jumpForce;
        public float timeSpeed = 1f;
        
       
        public float jumpSpeed;
        
       // private TopPartSpawn topPartSpawn;
     
        [HideInInspector]
       public GameObject TopParent;

        private void Awake()
        {
            PlayerRaggedDollFix();
           
            //topPartSpawn.GetComponent<TopPartSpawn>();
           
            //PlayerIdle.Invoke();
            Time.timeScale = timeSpeed;

        }

        void Start()
        {
            SpawnPlatform();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerPlatform"))
            {
         
                //print("yes");
                OnPlayerLand.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerPlatform"))
            {
                //print("no");
                OnPlayerJump.Invoke();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("space called");
                RightAnswer();
            }
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
        
        public void RightAnswer()
        {

            //Platform Spawn

            // GameObject platform = Instantiate(platformPrefab, nextMarkerPosition.position, Quaternion.identity);
            //estAnswerRight = false;
            //nextMarkerPosition.position = new Vector3(nextMarkerPosition.position.x,nextMarkerPosition.position.y - 1,nextMarkerPosition.position.z + 1.5f);

            //PlayerJump+MoveForward


            //this.GetComponent<Rigidbody>().AddForce(Vector3.up*upward,ForceMode.Impulse);
            // StartCoroutine("WaitNJump");
            SpawnPlatform();
            Jump();

           

        }


        void Jump()
        {
            
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * jumpForce);
            this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x,this.GetComponent<Rigidbody>().velocity.y * jumpSpeed,this.GetComponent<Rigidbody>().velocity.z);
            RandomJump();
        }
        
        
       

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
                     
            if (TopParent)
            {
                Debug.Log("called for spawn platform");
                GameObject playaPlatform = null;
                if (nextMarkerPosition)
                {
                     playaPlatform = Instantiate(platformPrefab, new Vector3(nextMarkerPosition.position.x, nextMarkerPosition.position.y-1, nextMarkerPosition.position.z + 1.5f), Quaternion.identity,
                     TopParent.transform);
                }
                else
                {
                    playaPlatform = Instantiate(platformPrefab, new Vector3(0.75f, 0f, 0f), Quaternion.identity,
                    TopParent.transform);
                }
                nextMarkerPosition = playaPlatform.transform;
            }
        }
    }

}
