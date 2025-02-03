using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public bool isStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

    }

    public AudioSource musicAudio;

    public void SoundToggle()
    {
        if (musicAudio.isPlaying)
            musicAudio.Stop();
        else if (!musicAudio.isPlaying)
            musicAudio.Play();
    }
}
