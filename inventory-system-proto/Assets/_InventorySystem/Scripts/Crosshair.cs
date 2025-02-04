using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    
    public Sprite crosshairSprite; 
    public Vector2 crosshairSize = new Vector2(50, 50); 

    private Image crosshairImage;

    public GameObject crosshairGO;

    void Start()
    {
        crosshairGO = new GameObject("Crosshair");
        crosshairGO.transform.SetParent(GameObject.Find("Canvas").transform);
        
        crosshairImage = crosshairGO.AddComponent<Image>();
        crosshairImage.sprite = crosshairSprite;
        
        RectTransform rectTransform = crosshairImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = crosshairSize;
        rectTransform.anchoredPosition = Vector2.zero; 
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
    
}
