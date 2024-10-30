using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    //지속적으로 회전

    //닿으면 어떤일이 생기는지 결정

    private Razor razor;

    private void Awake()
    {
        razor = GetComponentInChildren<Razor>();
        razor.CCTVPos = transform.position;
    }

    public float minRot = -160f;
    public float maxRot = -100f;
    public float rotSpeed = 1.0f;
    private float currentYRotation;
    private bool rotatingRight = true;


    private void FixedUpdate()
    {
        CCTVRotation();
    }

    private void CCTVRotation()
    {
        currentYRotation = transform.localEulerAngles.y;

        float rotationDelta = rotSpeed * Time.deltaTime;

        if (rotatingRight)
        {
            currentYRotation += rotationDelta;
            if(currentYRotation >= maxRot)
            {
                rotatingRight = false;
            }
        }
        else
        {
            currentYRotation -= rotationDelta;
            if(currentYRotation <= minRot)
            {
                rotatingRight=true;
            }
        }

        Vector3 rotation = transform.localEulerAngles;
        rotation.y = currentYRotation;
        transform.localEulerAngles = rotation;
    }
}
