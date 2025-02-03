using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;


    public int roadLane = 2;
    public static float moveSpeed = 7f;
    public float jumpForce = 10f;
    public bool isGrounded = false;
    public float distBWLane;

    public Animator animator;

    public Animator cameraShakeAnimator;

    public AudioSource coinAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (SoundManager.Instance.isStarted)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;

            moveSpeed += 0.001f;

            //Running Anim
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);
            animator.SetBool("isSlide", false);


            Swipe();
            Jump();
        }
       
    }

    void Swipe()
    {
        if (SwipeControls.swipeRight )
        {
            roadLane++;
            if (roadLane == 4)
                roadLane = 3;

            animator.SetBool("isRight", true);
            
        }

        if (SwipeControls.swipeLeft )
        {
            roadLane--;
            if (roadLane == 0)
                roadLane = 1;

            animator.SetBool("isLeft", true);
           
        }

        Vector3 targetPos = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (roadLane == 1)
        {
            targetPos += Vector3.left * distBWLane;
        }
        else if (roadLane == 3)
        {
            targetPos += Vector3.right * distBWLane;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, 5f * Time.deltaTime);

        if (SwipeControls.swipeDown)
            StartCoroutine(SlideDown());
    }

    void Jump()
    {
        if (isGrounded && SwipeControls.swipeUp)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
            
        }

    }


    IEnumerator SlideDown()
    {
        capsuleCollider.height = 54.3f;


        
        animator.SetBool("isSlide", true);

        yield return new WaitForSeconds(0.7f);

        capsuleCollider.height = 102.8902f;

    }





    private void OnCollisionEnter(Collision collision)
    {
        //Grounded Check Condition
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //Obstacle collision
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //decrease health by 1
            GameManager.Instance.playerHealth--;

            //Camera shake or slow down game or music slow
            StartCoroutine(CamShake());
            //destroy object + particle effect
            Destroy(other.gameObject);

        }

       else if (other.gameObject.CompareTag("Coin"))
        {
            coinAudio.Play();
        }

    }

    IEnumerator CamShake()
    {
        //Camera Shake Effect
        cameraShakeAnimator.SetBool("toShake", true);
        if(GameManager.Instance.playerHealth ==3 )
        {
            cameraShakeAnimator.speed = 1f;
        }
        if (GameManager.Instance.playerHealth == 2)
        {
            cameraShakeAnimator.speed = 1.3f;
        }
        if (GameManager.Instance.playerHealth == 1)
        {
            cameraShakeAnimator.speed = 1.5f;
        }

        //Slow Motion Effect
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.7f);
        Time.timeScale = 1f;
        cameraShakeAnimator.SetBool("toShake", false);

    }

    

}

