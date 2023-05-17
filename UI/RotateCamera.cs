using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 0.01f;
    public float xSpeed = 120.0f;
    public bool RemoveControl = false;
    float x = 0.0f;
    float y = 0.0f;
    [HideInInspector] public float mouseX = 0f;
    float mouseY = 0f;
    float startX, startZ;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        startX = transform.rotation.eulerAngles.x;
        startZ = transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        if (!RemoveControl)
            GetMouseButtonDown_XY();
    }

    void LateUpdate()
    {
        if (target)
        {
            x += mouseX * xSpeed * distance * 0.02f;

            Quaternion rotation = Quaternion.Euler(startX, x, startZ);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    Vector3 mousePosPrev;
    void GetMouseButtonDown_XY()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosPrev = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 newMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (newMousePos.x < mousePosPrev.x)
            {
                mouseX = -1;
            }
            else if (newMousePos.x > mousePosPrev.x)
            {
                mouseX = 1;
            }
            else
            {
                mouseX = -0;
            }

            if (newMousePos.y < mousePosPrev.y)
            {
                mouseY = -1;
            }
            else if (newMousePos.y > mousePosPrev.y)
            {
                mouseY = 1;
            }
            else
            {
                mouseY = -0;
            }

            mousePosPrev = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
