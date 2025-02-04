using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFall : MonoBehaviour
{
    public GameObject fadePanel;

    

    public GameObject checkPoint;

    public GameObject player;

    //public GameObject movingBox;

    private void Start()
    {
        fadePanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            fadePanel.SetActive(true);
            
                StartCoroutine(Respawn());
            
        }
    }

    IEnumerator Respawn()
    {
        
        yield return new WaitForSeconds(4f);
        player.transform.position = checkPoint.transform.position;
        //movingBox.transform.position = checkPoint.transform.position;
        fadePanel.SetActive(false);

    }
}
