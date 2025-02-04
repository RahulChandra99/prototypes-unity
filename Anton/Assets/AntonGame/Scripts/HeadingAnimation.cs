using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadingAnimation : MonoBehaviour
{
    public GameObject sprite;
    public ParticleSystem particle;

    private void Start()
    {
        StartCoroutine(toWait());
        
    }

    IEnumerator toWait()
    {
        yield return new WaitForSeconds(1.5f);
        PlayThis();
    } 

    void PlayThis()
    {
        sprite.gameObject.SetActive(false);
        particle.Emit(9999);
    }
}
