using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill3 : MonoBehaviour {

    public static skill3 s1;
    public bool isActive;
    public float lastTime;
    public float height;//特效的高度，要根据不同特效调才好放到地上

    // Use this for initialization
    void Awake()
    {
        s1 = this;
        lastTime = 5.0f;
        isActive = false;
        height = 0.0f;
    }

    void Start()
    {
        StartCoroutine(skill_stay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //怪进入触发器
    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            GameObject hit = other.gameObject;
            Monster monsters = hit.GetComponent<Monster>();
            if (monsters != null)
            {
                StartCoroutine(attack(monsters));
            }
            //Debug.Log("Skill hit");
        }
    }

    // 接触持续中
    void OnTriggerExit(Collider other)
    {
        GameObject hit = other.gameObject;
        Monster monsters = hit.GetComponent<Monster>();
        if (monsters != null)
        {
            StopCoroutine(attack(monsters));
        }
    }

    public void setActive()
    {
        isActive = true;
    }

    IEnumerator skill_stay()
    {
        while (lastTime > 0)
        {
            yield return new WaitForSeconds(1);
            lastTime--;
        }
        if (lastTime <= 0)
        {
            GameObject[] Monster =GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in Monster){
                monster.GetComponent<Monster>().CanMove();
            }
            Destroy(this.gameObject);
        }

            
    }

    IEnumerator attack(Monster monsters)
    {
        while (monsters.health > 0)
        {
            
            //定身效果
            Debug.Log("定身");
            monsters.CantMove();
            yield return new WaitForSeconds(1);
        }

    }
}
