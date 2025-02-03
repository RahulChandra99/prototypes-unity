using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetToFollow;

    private void Update()
    {
        transform.position = new Vector2(targetToFollow.position.x, Mathf.Clamp(targetToFollow.position.y, -4f, 10f));
    }
}
