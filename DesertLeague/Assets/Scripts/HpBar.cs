using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Slider slider;
    private GameObject minion;

    Camera cam;

    public void SetMinion(GameObject minion)
    {
        this.minion = minion;
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;
        cam = Camera.main;

    }

    public void Damaged(float damage)
    {
        slider.value -= damage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.WorldToScreenPoint(minion.transform.position + new Vector3(0f, 6f, 0f));
    }
}
