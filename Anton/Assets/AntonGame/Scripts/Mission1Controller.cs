using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mission1Controller : MonoBehaviour
{
    public GameObject playerOriginal;
    public GameObject fadeOutPanel;
    public GameObject upcomingMission;
    public GameObject currentMission;

    public bool gateopen = false;

    public GameObject toActivate;

    [SerializeField]
    private AudioSource switchSound;

    private void Start()
    {
        
        playerOriginal.SetActive(false);
        fadeOutPanel.SetActive(false);
        upcomingMission.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            switchSound.Play();

            //fadeoutanim
            fadeOutPanel.SetActive(true);

            StartCoroutine(nextMission());
            //load next mission
            //disable this mission
        }
    }

    IEnumerator nextMission()
    {
        yield return new WaitForSeconds(4f);
        upcomingMission.SetActive(true);
        if (upcomingMission.name == "Frame6")
            gateopen = true;
        toActivate.SetActive(true);
        Destroy(currentMission, 2f);
       
    }
}
