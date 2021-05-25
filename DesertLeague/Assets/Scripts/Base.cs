using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private GameObject canvas;
    Rigidbody rigid;

    [SerializeField] private int max_health;
    private int cur_health;

    private HpBar hpBar;
    public bool isAlive;

    private int timer = 0;

 
    private void Start()
    {
        cur_health = max_health;
        isAlive = true;

        canvas = GameObject.Find("HpCanvas");
        GameObject targetHpBar = Instantiate(Resources.Load("HpBar") as GameObject, transform.position, Quaternion.identity, canvas.transform);

        hpBar = targetHpBar.GetComponent<HpBar>();
        hpBar.SetMinion(gameObject);

        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isAlive)
        {
            Hit(10);
        }

        if (!isAlive)
        {
            timer += 1;
            if (timer >= 500)
            {
                Destroy(gameObject);
            }
            return;
        }

    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
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
    }

}
