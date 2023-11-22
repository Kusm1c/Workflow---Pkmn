using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private PlayerControls playerInput;

    private bool isMoving;
    private Vector3 direction = Vector3.zero;
    private void Awake()
    {
        playerInput = new();
    }

    private void OnEnable()
    {
        playerInput.Movement.Enable();
        playerInput.Menu.Enable();
    }

    private void Update()
    {
        GetInput();
        Move();
    }

    private void GetInput()
    {
        isMoving = playerInput.Movement.Movement.IsPressed();
    }

    private void Move()
    {
        if (isMoving)
        {
            direction = playerInput.Movement.Movement.ReadValue<Vector2>();
            transform.position += speed * Time.deltaTime * direction ;
        }
        else
        {
            var snappedPos = new Vector3();
            
            snappedPos.x = direction.x > 0 ? Mathf.Ceil(transform.position.x) : Mathf.Floor(transform.position.x);
            snappedPos.y = direction.y > 0 ? Mathf.Ceil(transform.position.y) : Mathf.Floor(transform.position.y);
            snappedPos.z = transform.position.z;
            
            if((snappedPos - transform.position).magnitude < .1f)
                transform.position = snappedPos;
            else
                transform.position += speed * Time.deltaTime * direction ;
                
        }
    }
}
