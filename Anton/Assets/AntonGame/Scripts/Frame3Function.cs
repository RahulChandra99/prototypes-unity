using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame3Function : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AntonPlayerController>().jumpPower = 800;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
