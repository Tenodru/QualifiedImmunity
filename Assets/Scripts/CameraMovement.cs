using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement current;
    public float dampTime = .15f; // free camera movement speed
    private Vector3 velocity = Vector3.zero; // camera not currently moving

    public Transform target;

    public bool playerFocus = true;

    private void Start()
    {
        current = this;
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 cameraTarget = new Vector3(target.position.x, target.position.y, transform.position.z);

        /*if (playerFocus)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraTarget, 1);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, cameraTarget, ref velocity, dampTime); // move camera toward target
        }*/

        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget, ref velocity, dampTime); // move camera toward target
    }
}
