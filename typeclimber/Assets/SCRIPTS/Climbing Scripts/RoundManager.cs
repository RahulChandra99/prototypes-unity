using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class RoundManager : MonoBehaviour
{
    public int numberOfBricks;
    public GameObject BrickPrefab;
    public GameObject parentBrick;
    public GameObject topRock;
    public GameObject topPanel;
    public GameObject Rope;
    public GameObject ropeParent;
    GameObject nextLocation;
    Rigidbody previousRigidBody;
    HingeJoint previousHingeJoint;
    [SerializeField]
    bool disableHiding;
    public TimelineAsset[] RoundAssets;
    public PlayableDirector director;
    public GameObject DeathCamera;
    public GameObject[] RoundOnePlayers,Round2Players,Round3Players;

   
    void Start()
    {
       // Debug.Log("requesting " + GM_Climbing.Instance.roundNumber);
       if (GM_Climbing.Instance.TriviaGame)
       {
           numberOfBricks = 6;

       }
       else
       {
           numberOfBricks = 6;
       }
        
        
        GenerateArena();
        if (!disableHiding)
        {

            if (GM_Climbing.Instance.roundNumber == 1)
            {
                //Debug.Log("playing timeline");
                director.playableAsset= RoundAssets[0];
                director.Play();
                //Debug.Log("timeline played");

               /* foreach (var x in RoundOnePlayers)
                {
                    x.transform.GetChild(0).gameObject.SetActive(false);
                }*/
            }
            if (GM_Climbing.Instance.roundNumber == 2)
            {

                director.playableAsset = RoundAssets[1];
                director.Play();
              /*  foreach (var x in Round2Players)
                {
                    x.transform.GetChild(0).gameObject.SetActive(false);
                }*/
            }

            if (GM_Climbing.Instance.roundNumber == 3)
            {
                director.playableAsset = RoundAssets[2];
                director.Play();
              /*  foreach (var x in Round3Players)
                {
                    x.transform.GetChild(0).gameObject.SetActive(false);
                }*/
            }
        }
    }

    void GenerateArena()
    {
        for (int i = 0; i < numberOfBricks; i++)
        {

            if (i == numberOfBricks - 1)
            {
                var a = Instantiate(topRock, new Vector3(0f, i * 1.5f, 0f), Quaternion.Euler(0f, 0f, 0f), parentBrick.transform);
                a.transform.localRotation = Quaternion.Euler(0, 0, 0);

            }

            else
            {
                var a = Instantiate(BrickPrefab, new Vector3(0f, i * 1.5f, 0f), Quaternion.Euler(0f, 0f, 0f), parentBrick.transform);
                a.transform.localRotation = Quaternion.Euler(0, 0, 0);

            }
        }

        GenerateRopeTile();
    }


    int count;
    public void GenerateRopeTile()
    {
        for (int j = -1; j < 2; j++)
        {
            for (int i = numberOfBricks; i >= 0; i--)
            {
                if (i == numberOfBricks)
                {
                    var a = Instantiate(Rope, ropeParent.transform);
                    a.transform.localPosition = new Vector3(j*1.2f, i * 1.70f, -2.16f);
                    nextLocation = a.transform.GetChild(1).GetChild(0).gameObject;
                    previousRigidBody = a.transform.GetChild(1).GetComponent<Rigidbody>();
                    previousRigidBody.isKinematic = true;
                }
                else
                {
                    count++;
                    var a = Instantiate(Rope, nextLocation.transform.position,Quaternion.identity,ropeParent.transform);
                    //a.transform.localPosition = new Vector3(0, count*-0.05f, 0);
                    previousHingeJoint = a.transform.GetChild(0).GetComponent<HingeJoint>();
                    previousHingeJoint.connectedBody = previousRigidBody;
                    nextLocation = a.transform.GetChild(1).GetChild(0).gameObject;
                    previousRigidBody = a.transform.GetChild(1).GetComponent<Rigidbody>();
                    previousRigidBody.isKinematic = true;
                    previousRigidBody.useGravity = true;

                }

            }
            count = 0;
        }
    }

    public void Die()
    {
        DeathCamera.SetActive(true);
    }
}
