using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float weight;
    public string itemName = "default";
    public string identifier = "01";
    public string type;
    public Vector3 itemOriginalPosition;

    [HideInInspector]public TextMeshPro itemNameTXT;

    private void Awake()
    {
        itemNameTXT = this.transform.GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        itemNameTXT.text = itemName;
        itemOriginalPosition = transform.position;
    }
}
