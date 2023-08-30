using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerMove : MonoBehaviour
{

    private float horizontalMove, verticalMove;
    private float mouseX, mouseY;
    
    private Vector3 moveDir, playerRotateY;

    private float currentCameraX;
    private Quaternion cameraRotate;

    [SerializeField] private float moveSpeed, lookSensitivity, cameraRotationLimit;
    
    private Rigidbody playerRigid;
    private CapsuleCollider playerCol;
    private Camera playerCamera;
    
    private void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        playerCol = GetComponent<CapsuleCollider>();
        playerCamera = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotateCamera();
        RotatePlayer();
    }

    private void GetInputs()
    {
        verticalMove = Input.GetAxisRaw("Vertical");
        horizontalMove = Input.GetAxisRaw("Horizontal");

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
    }

    private void MovePlayer()
    {
        moveDir = new Vector3(horizontalMove, 0f, verticalMove).normalized * (Time.fixedDeltaTime * moveSpeed);
        
        playerRigid.MovePosition(transform.position + transform.TransformDirection(moveDir));
    }

    private void RotateCamera()
    {
        currentCameraX -= mouseY;
        currentCameraX = Math.Clamp(currentCameraX, -cameraRotationLimit, cameraRotationLimit);

        cameraRotate.eulerAngles = this.transform.rotation +   Vector3(currentCameraX, 0f, 0f);

        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, cameraRotate, lookSensitivity * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        playerRotateY = new Vector3(0f, mouseX, 0f) * lookSensitivity;
        playerRigid.MoveRotation(playerRigid.rotation * Quaternion.Euler(playerRotateY));
    }
}
