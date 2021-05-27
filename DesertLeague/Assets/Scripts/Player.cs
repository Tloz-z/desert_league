using EasyJoystick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IEnemy
{

    [SerializeField] private int max_health;
    [SerializeField] private TeamColor teamColor;
    private int cur_health;

    [SerializeField] private float speed;
    [SerializeField] private Joystick joystick;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    bool isBorder;

    public int GetHp()
    {
        return this.cur_health;
    }

    public TeamColor GetTeamColor()
    {
        return this.teamColor;
    }

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

    public void Hit(int damage)
    {
        return;
    }
}
