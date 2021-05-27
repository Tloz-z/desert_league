using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour , IEnemy
{

    private GameObject canvas;
    Rigidbody rigid;

    [SerializeField] private int max_health;
    private int cur_health;

    private HpBar hpBar;
    private Animator animator;
    private bool isAlive;

    [SerializeField] private GameObject target; // 이동타겟
    [SerializeField] private int speed;

    private int timer = 0;

    [SerializeField] private TeamColor teamColor;

    // 이 밑으로 ai를 위한 변수들
    public float range; // 타게팅범위
    private float attackRange = 6.0f;  //사거리

    private bool isTargeting;
    public bool doingAttack = false;
    private bool isAttack;


    public int GetHp()
    {
        return this.cur_health;
    }

    public TeamColor GetTeamColor()
    {
        return this.teamColor;
    }
    void Start()
    {
        cur_health = max_health;
        isAlive = true;

        canvas = GameObject.Find("HpCanvas");
        GameObject targetHpBar = Instantiate(Resources.Load("HpBar") as GameObject, transform.position, Quaternion.identity, canvas.transform);

        hpBar = targetHpBar.GetComponent<HpBar>();
        hpBar.SetMinion(gameObject);

        animator = GetComponentInChildren<Animator>();

        rigid = GetComponent<Rigidbody>();

        // 이 밑으로 ai를 위한 초기화
        isTargeting = false;
        isAttack = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isAlive)
        {
            Debug.Log("doingAttack : " + doingAttack);
            Debug.Log("isAttack : " + isAttack);
            Debug.Log("Target : " + target);
            Debug.Log("isTargeting : " + isTargeting);
            //Hit(20);
        }

        if (!isAlive)
        {
            timer += 1;
            if (timer >= 1000)
            {
                Destroy(gameObject);
            }
            return;
        }

        // 이 밑으로 미니언 ai

        if (doingAttack)
        {
            return;
        }

        if (isAttack)
        {
            isAttack = false;
            doingAttack = true;
            StartCoroutine("Attack");
            return;
        }

        // 타겟팅이 안되어있을때 새로운 대상을 찾음
        if (!isTargeting || target == null ) {
            Targeting(); }
        else { CheckTarget(); }

        Chase(); // 네비게이션 AI 코드
    }


    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    public void Hit(int damage)
    {
        cur_health -= damage;
        hpBar.Damaged(((float)damage / max_health));
        if (cur_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        hpBar.Die();
        animator.SetBool("isDie", true);
    }

    void FreezeVelocity()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }

    // 이 밑으로 ai를 위한 함수들
    void Targeting()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
        GameObject[] targetbases = GameObject.FindGameObjectsWithTag("Base");
        GameObject[] targetplayers = GameObject.FindGameObjectsWithTag("Player");

        float shortestDistance = Mathf.Infinity; //가장 짧은 거리
        GameObject nearestTarget = null;  //가장 가까운 타겟

        foreach(GameObject minion in minions) // 가장 가까운 미니언 탐색
        {
           if(minion.GetComponent<IEnemy>().GetTeamColor() == this.teamColor ||
                 minion.GetComponent<IEnemy>().GetHp() <= 0) { continue; }

            float distanceToTarget = Vector3.Distance(transform.position, minion.transform.position);
            if(distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = minion;
            }
        }

        if(shortestDistance <= range) //미니언이 사정거리 내에 있을 경우
        {
            target = nearestTarget;
            isTargeting = true;
        }
        else // 미니언이 사정 거리에 없을 경우, 가장 가까운 건물을 탐색
        {
            shortestDistance = Mathf.Infinity;
            foreach (GameObject targetbase in targetbases)
            {
                if (targetbase.GetComponent<IEnemy>().GetTeamColor() == this.teamColor ||
                    targetbase.GetComponent<IEnemy>().GetHp() <= 0) { continue; }

                float distanceToTarget = Vector3.Distance(transform.position, targetbase.transform.position);
                if (distanceToTarget < shortestDistance)
                {
                    shortestDistance = distanceToTarget;
                    nearestTarget = targetbase;
                }
            }
            if (shortestDistance <= range) //건물이 사정거리 내에 있을 경우
            {
                target = nearestTarget;
                isTargeting = true;

            }
            else // 건물이 사정 거리에 없을 경우, 가장 가까운 플레이어 탐색
            {
                shortestDistance = Mathf.Infinity;
                foreach (GameObject targetplayer in targetplayers)
                {
                    if (targetplayer.GetComponent<IEnemy>().GetTeamColor() == this.teamColor ||
                        targetplayer.GetComponent<IEnemy>().GetHp() <= 0) { continue; }

                    float distanceToTarget = Vector3.Distance(transform.position, targetplayer.transform.position);
                    if (distanceToTarget < shortestDistance)
                    {
                        shortestDistance = distanceToTarget;
                        nearestTarget = targetplayer;
                    }
                }
                if (shortestDistance <= range) //적 플레이어가 사정거리 내에 있을 경우
                {
                    target = nearestTarget;
                    isTargeting = true;

                }
                else { target = GameObject.FindGameObjectWithTag("Nexus"); isTargeting = true; }
            }
        }
    }
    void CheckTarget()
    {
        if(target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if(distanceToTarget > range) // 타겟이 공격범위를 벗어났을 때
            {
                isTargeting = false;
            }

            if (distanceToTarget < attackRange)
            {
                isAttack = true;
            }
        }
    }
    private void Chase()
    {
        Vector3 movVec = target.transform.position - transform.position;
        transform.position += movVec.normalized * Time.deltaTime * speed;
        transform.LookAt(transform.position + movVec);
    }

    IEnumerator Attack()
    {
        GameObject attackTarget = this.target;
        
        animator.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.88f);

        attackTarget.GetComponent<IEnemy>().Hit(5);
        yield return new WaitForSeconds(1.12f);

        doingAttack = false;
        animator.SetBool("isAttack", false);

        if (attackTarget.GetComponent<IEnemy>().GetHp() - 5 <= 0)
        {
            this.target = null;
            isTargeting = false;
        }

        yield return null;
    }
}
