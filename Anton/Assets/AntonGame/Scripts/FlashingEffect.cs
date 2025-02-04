using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingEffect : MonoBehaviour
{
    public GameObject myLight;

    public bool isFlickering;
    public float mintimeToWait = 0.5f;
    public float maxtimeToWait = 0.5f;

    private void Start()
    {
        isFlickering = false;
    }
    private void Update()
    {
        if (isFlickering == false)
        {

            StartCoroutine(Flashing());
        }
    }

    IEnumerator Flashing()
    {
        isFlickering = true;
            myLight.SetActive(false);
            yield return new WaitForSeconds(Random.Range(mintimeToWait,maxtimeToWait));
            myLight.SetActive(true);
        yield return new WaitForSeconds(Random.Range(mintimeToWait, maxtimeToWait));
        isFlickering = false;

    }
}
