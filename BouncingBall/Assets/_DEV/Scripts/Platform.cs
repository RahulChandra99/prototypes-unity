using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    public int bounceValue;
    public TextMeshPro bounceValue_txt;
    public bool bigBounce;

    [SerializeField] private ClickOrTapToExplode _explode;
    [SerializeField] private GameManager _gameManager;
    private void Start()
    {
        bounceValue = UnityEngine.Random.Range(2, 6);
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        bounceValue_txt.text = bounceValue.ToString();

        if (bounceValue == 0)
        {
            bigBounce = true;
            BreakPlatform();
        }
    }

    private void BreakPlatform()
    {
        _gameManager.platformBreaking_AS.Play();
        _gameManager.TriggerVibration(0.3f);
        _explode.DestroyStuff();
        //this.gameObject.SetActive(false);
    }
}
