using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Scene number to change to
    public int SceneNumber = 0;

    public bool FadeOnStart = true;

    public Animator fadeAnimator = null;

    public GameObject AntonTitlePanel;
    public AudioSource BGThunderSound;
    public float customTime;

    //trigger names
    private int FadeOutTrigger = Animator.StringToHash("FadeIn");
    private int FadeInTrigger = Animator.StringToHash("FadeOut");

    private void Start()
    {
        fadeAnimator = GetComponent<Animator>();
        AntonTitlePanel.SetActive(false);

        if (FadeOnStart && fadeAnimator != null)
            fadeAnimator.SetTrigger(FadeInTrigger);
    }

    void SceneChange()
    {
        StartCoroutine(SceneDelay());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        AntonTitlePanel.SetActive(true);
        BGThunderSound.Stop();
        FindObjectOfType<AntonPlayerController>().enabled = false;
        FindObjectOfType<PlayerHealthUI>().numberofhearts = 0;
        fadeAnimator.SetTrigger(FadeOutTrigger);
    }

    IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(customTime);
        SceneManager.LoadScene(SceneNumber);

    }
}
