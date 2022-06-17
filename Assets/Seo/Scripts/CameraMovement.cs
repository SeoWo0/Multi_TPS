﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [Header("OBJ Setting")]
    [SerializeField]
    private Transform objectFollow;         // 카메라가 따라갈 대상
    [SerializeField]
    private Transform selectCamera;         // 따라오는 카메라

    [Header("Camera Setting")]
    [SerializeField]
    private float followSpeed = 10;         // 카메라 이동 속도
    [SerializeField]
    private float smoothness = 10;          // 카메라 이동 부드러움
    [SerializeField]
    private float minDistance = 0;          // 카메라 와 장애물 사이 인식 최소 거리
    [SerializeField]    
    private float maxDistance = 3;          // 카메라 와 장애물 사이 인식 최대 거리

    [Header("Sensitivity")]
    [SerializeField]
    private float rotXCamAxisSpeed = 400;   // 카메라 x축 회전 속도
    [SerializeField]
    private float rotYCamAxisSpeed = 400;   // 카메라 y축 회전 속도
    private float limitMinX = -80;          // 카메라 x축 회전 최소범위
    private float limitMaxX = 50;           // 카메라 x축 회전 최대범위
    private float eulerAngleY;
    private float eulerAngleX;
    private Vector3 dirNormalized;
    private Vector3 finalDir;
    private float finalDistance;

    private void Awake() {
        Cursor.visible      = false;                    // 마우스 커서 지우기
        Cursor.lockState    = CursorLockMode.Locked;    // 마우스 위치 Lock
    }

    private void Start() {      // 초기화 작업
        dirNormalized = selectCamera.localPosition.normalized;
        finalDistance = selectCamera.localPosition.magnitude;
    }

    private void Update() {
        UpdateRotate();
    }

    private void LateUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, objectFollow.position, followSpeed);  
        finalDir = transform.TransformPoint(dirNormalized * maxDistance);  

        RaycastHit hit;

        if(Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        selectCamera.localPosition = Vector3.Lerp(selectCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

    public void UpdateRotate()
    {
        eulerAngleY += inputManager.mouseX * rotYCamAxisSpeed * Time.deltaTime;   // 마우스 좌/우 이동으로 카메라 y축 회전
        eulerAngleX -= inputManager.mouseY * rotXCamAxisSpeed * Time.deltaTime;   // 마우스 위/아래 이동으로 카메라 x축 회전
        
        // 카메라 x축 회전의 경우 회전 범위를 설정
        eulerAngleX = Mathf.Clamp(eulerAngleX, limitMinX, limitMaxX);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }
    
    
}
