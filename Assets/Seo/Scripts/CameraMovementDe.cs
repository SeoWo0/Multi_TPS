using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementDe : MonoBehaviour
{
    public Transform objectToFollow;
    public float followspeed = 10f;
    public float sensitivity = 100f;
    public float clampAngle = 50f;      // 위 아래 각도
    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 finalDir;
    // 장애물이 있을 경우의 변수
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10;

    private float rotX;
    private float rotY;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
    }

    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followspeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;
        bool check = Physics.Linecast(transform.position, finalDir, LayerMask.GetMask("Enemy"));

        if (!check)
        {
            if (Physics.Linecast(transform.position, finalDir, out hit))
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            else
                finalDistance = maxDistance;
            realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
        }
    }
}
