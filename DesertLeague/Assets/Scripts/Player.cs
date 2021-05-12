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
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void FreezeVelocity()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }
    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void Update()
    {
        hAxis = joystick.Horizontal();
        vAxis = joystick.Vertical();

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);

        transform.LookAt(transform.position + moveVec);
    }
}
