using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInput playerInputActions;
    [SerializeField] private Transform[] LaneTranforms;
    private Vector3 _destination;
    private int _currentLaneIndex;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody _rigidbody;


    private void OnEnable()
    {
        playerInputActions = new PlayerInput();
        playerInputActions?.Enable();

        _rigidbody = GetComponent<Rigidbody>();
        
    }

    private void Start()
    {
        playerInputActions.Gameplay.Move.performed += MovePerformed;
        playerInputActions.Gameplay.Jump.performed += JumpPerformed;

        for (int i = 0; i < LaneTranforms.Length; i++)
        {
            if (LaneTranforms[i].position.x == transform.position.x)
            {
                _currentLaneIndex = i;
                _destination = LaneTranforms[i].position;
            }
        }
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        _rigidbody?.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.VelocityChange);
    }

    void MovePerformed(InputAction.CallbackContext obj)
    {
        float InputValue = obj.ReadValue<float>(); 
        if(InputValue < 0f) { MoveLeft(); }
        else if(InputValue > 0f) { MoveRight(); }
    }

    private void MoveLeft()
    {
        if(_currentLaneIndex == 0)
        {
            return;
        }

        _currentLaneIndex--;
        _destination = LaneTranforms[_currentLaneIndex].position;
    }

    private void MoveRight()
    {
        if (_currentLaneIndex == LaneTranforms.Length - 1)
        {
            return;
        }

        _currentLaneIndex++;
        _destination = LaneTranforms[_currentLaneIndex].position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position,new Vector3(_destination.x,transform.position.y,transform.position.z),moveSpeed * Time.deltaTime);
       
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
}
