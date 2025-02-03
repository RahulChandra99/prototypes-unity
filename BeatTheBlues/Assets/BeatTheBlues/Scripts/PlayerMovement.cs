using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Rigidbody2D myBody;

    public float jumpForce = 100f;

    public float moveSpeed;

    public AudioSource jumpSound;

    

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.isKinematic = true;
        moveSpeed = 0f;

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            myBody.isKinematic = false;
            moveSpeed = 8f;
            Jump();
        }

        transform.Translate(Input.acceleration.x * moveSpeed * Time.deltaTime, 0,0);
        


    }



    void Jump()
    {
        jumpSound.Play();
        myBody.velocity = Vector2.up * jumpForce * Time.deltaTime;

    }
}
