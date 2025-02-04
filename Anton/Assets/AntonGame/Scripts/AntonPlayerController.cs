using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonPlayerController : MonoBehaviour
{
    private Transform myTransform;
    private Rigidbody2D myBody;

    public float maxSpeed = 1f;

    public enum FacedDirection { FaceLeft = -1,FaceRight = 1};
    public FacedDirection Facing = FacedDirection.FaceRight;

    public float jumpPower = 1f;
    public float jumpTimeOut = 1f;
    private bool canJump = true;

    public CircleCollider2D feetCollider;
    public bool isGrounded;
    public LayerMask GroundLayer;

    public Animator myAnimator = null;
    private int motionVal = Animator.StringToHash("Motion");

    public bool inverted = false;

    public ParticleSystem dust;

    public PhysicsMaterial2D originalMaterial;
    public PhysicsMaterial2D bouncyMaterial;

    public GameObject torchLight;
    public float horzMov;

    public bool healthInc = false;
    public GameObject whiteHeartCanvas;
    public GameObject whiteHeart;

    public AudioSource bedJumpSound;
    [SerializeField]
    private AudioSource torchLightToggle;

    public int a = 10;

    AntonPlayerController()
    {
        Debug.Log(a + "constructor");
    }


    private void Awake()
    {
        myTransform = GetComponent<Transform>();
        myBody = GetComponent<Rigidbody2D>();
        torchLight.SetActive(false);
        whiteHeartCanvas.SetActive(true);

        Debug.Log(a + "awake");
    }

    private void Start()
    {
        Debug.Log(a + "start");
    }

    private void OnEnable()
    {
        Debug.Log(a + "enable");
    }

    private bool GetGrounded()
    {
        Vector2 CircleCenter = new Vector2(myTransform.position.x, myTransform.position.y) + feetCollider.offset;
        Collider2D[] HitColliders = Physics2D.OverlapCircleAll(CircleCenter, feetCollider.radius, GroundLayer);
        if (HitColliders.Length > 0) return true;
        return false;
    }

    private void FixedUpdate()
    {

        Physics2D.IgnoreLayerCollision(0,11);
        isGrounded = GetGrounded();

        horzMov = Input.GetAxis("Horizontal"); 
        if(horzMov == 1 || horzMov == -1)
            CreateDust();

        if (inverted == true)
        {
            myBody.AddForce(Vector2.left * horzMov * maxSpeed);
        }
        else
            myBody.AddForce(Vector2.right * horzMov * maxSpeed);

        //CLAMP VELOCITY    
        myBody.velocity = new Vector2(Mathf.Clamp(myBody.velocity.x, -maxSpeed, maxSpeed),
                                        Mathf.Clamp(myBody.velocity.y, -maxSpeed, maxSpeed));

        //JUMP
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            Jump();



        if (inverted == true)
        {
            if ((horzMov > 0f && Facing != FacedDirection.FaceLeft) || (horzMov < 0f && Facing != FacedDirection.FaceRight))
                FlipDirection();
        }
        else
        {   //FLIPPING THE DIRECTION BY CHANGING THE LOCAL SCALE ALONG X DIRECTION
            if ((horzMov < 0f && Facing != FacedDirection.FaceLeft) || (horzMov > 0f && Facing != FacedDirection.FaceRight))
                FlipDirection();
        }

        //CHANGING THE MOTION VALUE FOR BLEND TREE ANIMATION
        myAnimator.SetFloat(motionVal, Mathf.Abs(horzMov), 0.1f, Time.fixedDeltaTime);


        
        
     }

    void FlipDirection()
    {
        Facing = (FacedDirection)((int)Facing * -1f);
        Vector3 LocalScale = myTransform.localScale;
        LocalScale.x *= -1f;
        myTransform.localScale = LocalScale;
    }

    private void Jump()
    {
        if (!isGrounded || !canJump) return;

        myBody.AddForce(Vector2.up * jumpPower);
        canJump = false;
        Invoke("ActivateJump", jumpTimeOut);
    }

    void ActivateJump()
    {
        canJump = true;
    }

    
    void CreateDust()
    {
        dust.Play();
    }


    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bed"))
        {
            GetComponent<CircleCollider2D>().sharedMaterial = bouncyMaterial;
            bedJumpSound.Play();
        }

        if(collision.gameObject.CompareTag("Heart"))
        {
            healthInc = true;
            if (collision.gameObject.activeInHierarchy == true)
            {
                whiteHeartCanvas.SetActive(true);
            }
            Destroy(whiteHeart, 5f);

            Destroy(whiteHeartCanvas, 5f);
                       

        }

        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bed"))
        {
            GetComponent<CircleCollider2D>().sharedMaterial = originalMaterial;
            
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (torchLight.activeInHierarchy == false)
            {
                torchLightToggle.Play();
                torchLight.SetActive(true);

            }

            else if (torchLight.activeInHierarchy == true)
            {
                torchLightToggle.Play();
                torchLight.SetActive(false);
            }

        }
    }

    
}
