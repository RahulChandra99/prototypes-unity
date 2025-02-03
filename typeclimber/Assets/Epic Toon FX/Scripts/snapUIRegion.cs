using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class snapUIRegion : MonoBehaviour
{
    [Header("Apply this script to 3D object")]
    [Tooltip("UI element which you want to follow(Canvas must be screen Space Overlay Only).")]
    [SerializeField]
    GameObject uiElement;

    Camera cam;

    
    public GameObject Canvas;
    
    void Awake()
    {
        Canvas.SetActive(false);
        
        cam = Camera.main;
        
    }

    void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(this.transform.position);
        if (uiElement)
        {
            uiElement.transform.position = pos;
           
        }

        
        Invoke("EnableCanvas",5f);
    }
    /// <summary>
    /// here you have to replace the ui element when needed.
    /// </summary>
    /// <param name="g">Target element</param>
    public void UpdateTargetOfUIElement(GameObject g)
    {
        uiElement = g;
    }

    void EnableCanvas()
    {
        Canvas.SetActive(true);
        
    }
}
