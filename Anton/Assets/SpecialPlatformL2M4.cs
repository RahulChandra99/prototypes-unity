using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlatformL2M4 : MonoBehaviour
{
    public float moveSpeed;
    

    AntonPlayerController cc;

    private void Start()
    {
        cc = FindObjectOfType<AntonPlayerController>();
    }
    private void Update()
    {

       

        //going up
        if (cc.horzMov >= 0 )
        {
            transform.position = new Vector2(transform.position.x, transform.position.y  + moveSpeed * Time.deltaTime);
            
        }


        //going down
        if (cc.horzMov <= 0 )
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }

        


    }

    
    
}
