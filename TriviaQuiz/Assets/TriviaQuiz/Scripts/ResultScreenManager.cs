using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;
    public Sprite[] badgesPrefabs;
    public Image badge;
    public TextMeshProUGUI badgeText;
    void OnEnable()
    {
        scoreTxt.text = GameManager.instance.score.ToString();

        if (GameManager.instance.score >= 50)
        {
            badge.sprite = badgesPrefabs[0];
            badgeText.text = "Gold Badge";
        }

        if (GameManager.instance.score <50 && GameManager.instance.score>=20)
        {
            badge.sprite = badgesPrefabs[1];
            badgeText.text = "Silver Badge";
        }

        if (GameManager.instance.score < 20)
        {
            badge.sprite = badgesPrefabs[2];
            badgeText.text = "Bronze Badge";
        }
            
            
    }
}
