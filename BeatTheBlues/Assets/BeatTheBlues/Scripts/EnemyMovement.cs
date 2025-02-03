using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform rotationCentre;

    public float rotationRadius = 1.5f, angularSpeed = .5f;

    float posX, posY;
    public float angle = 0f;
    public float angle2;

    private void Update()
    {
        posX = rotationCentre.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCentre.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed;

        if (angle >= 360f)
            angle = angle2;

    }


}
