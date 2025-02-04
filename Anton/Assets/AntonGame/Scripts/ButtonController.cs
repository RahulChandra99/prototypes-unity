using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonController : MonoBehaviour
{
    

    [SerializeField]
    private GameObject titlePanel;
    public GameObject frame6;
    

    [SerializeField]
    private AudioSource switchSound;

    public Animator[] objectswithanimations;

    public GameObject mission;
    private void Start()
    {
        Destroy(titlePanel, 7f);
        for (int i = 0; i < objectswithanimations.Length; i++)
        {
            objectswithanimations[i].enabled = false;
        }
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            switchSound.Play();
            for (int i = 0; i < objectswithanimations.Length; i++)
            {
                objectswithanimations[i].enabled = true;
            }
            StartCoroutine(forMissions());
        }
    }

    IEnumerator forMissions()
    {
        yield return new WaitForSeconds(3f);
        mission.SetActive(true);
        //this.gameObject.SetActive(false);
        frame6.SetActive(false);
    }
}


