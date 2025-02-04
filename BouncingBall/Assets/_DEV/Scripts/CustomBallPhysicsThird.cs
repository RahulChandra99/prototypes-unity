using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustomBallPhysicsThird : MonoBehaviour
{
    public float gravity = -20f;
    public float bounceVelocity = 10f;
    public float increasedBounceVelocity = 15f;
    public Vector3 velocity;
    public float ballRadius = 0.5f;

    private Vector3 spawnPosition;
    private bool isGrounded;

    public float movementSpeed = 5f;
    public float bounceDampening = 0.9f;

    private Rigidbody rb;
    private GameManager _gameManager;

    private bool increaseNextBounce;

    private Camera mainCamera;
    private bool inputAllowed;
    private bool wasGamePaused;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        mainCamera = Camera.main;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        spawnPosition = transform.position;
        ResetBall();

        StartCoroutine(AllowInputAfterDelay(0.5f));
    }

    void Update()
    {
        if (wasGamePaused && !_gameManager.isGamePaused)
        {
            StartCoroutine(AllowInputAfterDelay(0.5f));
        }
        wasGamePaused = _gameManager.isGamePaused;

        if (_gameManager.isGamePaused || !inputAllowed)
            return;

        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && IsTouchInBottomHalf())
        {
            if (!_gameManager.isGameStarted)
            {
                _gameManager.isGameStarted = true;
                _gameManager.isGameOver = false;

                _gameManager.GameStarted();
            }

            if (_gameManager.isGameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (Input.GetMouseButtonUp(0) || IsTouchLifted())
        {
            if (!_gameManager.isGameOver)
            {
                GameOver();
            }
        }

        if (IsBallOutOfScreen() && !_gameManager.isGameOver)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        if (!_gameManager.isGameStarted || _gameManager.isGameOver)
            return;

        if (!isGrounded)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
            ApplyInputControl();
            Vector3 nextPosition = transform.position + velocity * Time.fixedDeltaTime;
            HandleCollisions(ref nextPosition);
            transform.position = nextPosition;
        }
    }

    public void ResetBall()
    {
        transform.position = spawnPosition;
        velocity = Vector3.zero;
        isGrounded = false;
        _gameManager.isGameOver = false;
    }

    void GameOver()
    {
        _gameManager.isGameOver = true;
        velocity = Vector3.zero;
        Debug.Log("Game Over");

        _gameManager.GameOver();

        if (_gameManager.totalStreak > _gameManager.highestStreak)
            PlayerPrefs.SetInt("HighScore", _gameManager.totalStreak);
    }

    void ApplyInputControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float distance;

            if (groundPlane.Raycast(ray, out distance))
            {
                Vector3 touchPosition = ray.GetPoint(distance);
                Vector3 direction = (touchPosition - transform.position).normalized;
                velocity.x = direction.x * movementSpeed;
                velocity.z = direction.z * movementSpeed;
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float distance;

            if (groundPlane.Raycast(ray, out distance))
            {
                Vector3 mousePosition = ray.GetPoint(distance);
                Vector3 direction = (mousePosition - transform.position).normalized;
                velocity.x = direction.x * movementSpeed;
                velocity.z = direction.z * movementSpeed;
            }
        }
    }

    void HandleCollisions(ref Vector3 nextPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, ballRadius + 0.1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                if (velocity.y < 0)
                {
                    nextPosition.y = hit.point.y + ballRadius;

                    float currentBounceVelocity = bounceVelocity;
                    if (increaseNextBounce)
                    {
                        currentBounceVelocity = increasedBounceVelocity;
                        increaseNextBounce = false;
                    }

                    velocity.y = currentBounceVelocity * bounceDampening;

                    Platform platformScript = hit.collider.gameObject.GetComponent<Platform>();
                    if (platformScript != null)
                    {
                        platformScript.bounceValue--;
                        if (platformScript.bounceValue == 1)
                        {
                            increaseNextBounce = true;
                        }
                    }

                    _gameManager.totalStreak++;
                    Debug.Log("Bounced on: " + hit.collider.gameObject.name);
                    _gameManager.ballbounce_AS.PlayOneShot(_gameManager.ballBounceSFX[_gameManager.activatedBallNumber]);
                }
            }
        }
    }

    bool IsTouchInBottomHalf()
    {
        float touchYPosition = Input.touchCount > 0 ? Input.GetTouch(0).position.y : Input.mousePosition.y;
        return touchYPosition < Screen.height / 2;
    }

    bool IsTouchLifted()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                return true;
            }
        }
        return false;
    }

    bool IsBallOutOfScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collision") && !_gameManager.isGameOver)
        {
            GameOver();
        }
    }

    private IEnumerator AllowInputAfterDelay(float delay)
    {
        inputAllowed = false;
        yield return new WaitForSeconds(delay);
        inputAllowed = true;
    }
}
