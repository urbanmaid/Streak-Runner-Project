using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10f; // 전진 속도
    public float turnSpeed = 100f; // 조향 속도
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 입력 받기
        float moveInput = Input.GetAxis("Vertical"); // W/S 또는 Up/Down 화살표
        float turnInput = Input.GetAxis("Horizontal"); // A/D 또는 Left/Right 화살표

        // 전진 및 후진
        Vector3 movement = transform.forward * moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // 조향
        Quaternion turn = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }
}
