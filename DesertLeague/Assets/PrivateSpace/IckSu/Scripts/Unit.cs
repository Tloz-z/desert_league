using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit: MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public Transform target; // 네비게이션 추적 타겟

    Rigidbody rigid;
    BoxCollider boxCollider;
    NavMeshAgent nav;

    public bool isChase;
    bool isAttack;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();

        ChaseStart();//따라가기를 실행함
    }

    void Update()
    {
        if(isChase)
            nav.SetDestination(target.position); // target의 위치로 따라감
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.angularVelocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
        }
    }

    void ChaseStart()
    {
        isChase = true;
    }
}
