using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollision5 : MonoBehaviour
{
    public Transform myParticle;

    public GameObject mainCharToDestroy;

    public AudioSource deathSound;
    

    [System.Obsolete]
    void Start()
    {
        myParticle.GetComponent<ParticleSystem>().enableEmission = false;
    }

    void Update()
    {
        
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            deathSound.Play();
            myParticle.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(ExampleCoroutine());
        }
    }

    IEnumerator ExampleCoroutine()
    {
        mainCharToDestroy.gameObject.SetActive(false);
        SendMessageUpwards("Finish");
        yield return new WaitForSeconds(2);
        Level5Manager.scoreValue = 0;
        SceneManager.LoadSceneAsync("Level5");
        

    }
}

