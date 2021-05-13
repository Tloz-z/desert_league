using EasyJoystick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Joystick joystick;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    bool isBorder;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 2, LayerMask.GetMask("Wall"));
    }

    void FreezeVelocity()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }
    void FixedUpdate()
    {
        FreezeVelocity();
        StopToWall();
    }

    void Update()
    {
        hAxis = joystick.Horizontal();
        vAxis = joystick.Vertical();

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(!isBorder)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);

        transform.LookAt(transform.position + moveVec);
    }
}
