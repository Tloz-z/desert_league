using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private GameObject canvas;

    [SerializeField] private int max_health;
    private int cur_health;

    private HpBar hpBar;
    private Animator animator;
    private bool isAlive;

    [SerializeField] private GameObject target;
    [SerializeField] private int speed;

    private int timer = 0;

    // 이 밑으로 ai를 위한 변수들

    // Start is called before the first frame update
    void Start()
    {
        cur_health = max_health;
        isAlive = true;

        canvas = GameObject.Find("HpCanvas");
        GameObject targetHpBar = Instantiate(Resources.Load("HpBar") as GameObject, transform.position, Quaternion.identity, canvas.transform);

        hpBar = targetHpBar.GetComponent<HpBar>();
        hpBar.SetMinion(gameObject);

        animator = GetComponentInChildren<Animator>();

        // 이 밑으로 ai를 위한 초기화

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(20);
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
        Vector3 movVec = target.transform.position - transform.position;
        transform.position += movVec.normalized * Time.deltaTime * speed;
        transform.LookAt(transform.position + movVec);
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

    // 이 밑으로 ai를 위한 함수들


}
