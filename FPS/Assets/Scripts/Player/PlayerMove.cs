using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float lookSensitivity, cameraLimit;

    private float speed;

    private float horizontalMove, verticalMove;
    private float mouseX, mouseY;
    private float rotateX, currentRotationX;
    private Vector3 moveDir;
    
    private Rigidbody playerRigid;
    private CapsuleCollider playerCollider;
    private Camera playerCamera;
    
    private void Start()
    {
        playerCamera = Camera.main;
        playerCollider = GetComponent<CapsuleCollider>();
        playerRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Rotate();
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
    }

    private void Rotate()
    {
        rotateX = mouseY * lookSensitivity;

        currentRotationX -= rotateX;
        currentRotationX = Math.Clamp(currentRotationX, -cameraLimit, cameraLimit);

        playerCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
    }

    private void MovePlayer()
    {
        moveDir = new Vector3(horizontalMove, 0f, verticalMove);
        
        playerRigid.MovePosition(transform.position + transform.TransformDirection(moveDir.normalized) * (Time.fixedDeltaTime * speed));
    }
}
