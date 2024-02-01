using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotating_Platform : MonoBehaviour
{
    private Vector3 targetAngle;

    private void Start()
    {
        targetAngle = transform.localEulerAngles;
        PlayerController.jumpPerformed += OnJump;
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetAngle), 4f);
    }

    void OnJump()
    {
        targetAngle.z = Mathf.Abs(targetAngle.z) == 180 ? 0 : 180;
    }
}
