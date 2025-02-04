using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform ball;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private float initialY;
    private bool isShaking = false;

    void Start()
    {
        ball = GameManager.Instance.activatedBall.transform;
        initialY = transform.position.y;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(ball.position.x, initialY, ball.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void TriggerCameraShake(float duration, float magnitude)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isShaking = true;
        yield return StartCoroutine(CameraShake(duration, magnitude));
        isShaking = false;
    }
}