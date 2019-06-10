﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill : MonoBehaviour {

    public float power ;
    public static skill s1;
    public bool isActive;
    public float lastTime;

    // Use this for initialization
    void Awake () {
        power = 15;
        s1 = this;
        lastTime = 5.0f;
        isActive = false;
	}

    void Start()
    {
        StartCoroutine(skill_stay());
    }

    // Update is called once per frame
    void Update () {
        
    }

    //怪进入触发器
    void OnTriggerEnter(Collider other)
    {
        if(isActive)
        {
   //         GameObject hit = other.gameObject;
			//Monster monsters = hit.GetComponent<Monster>();
            if (other.gameObject.GetComponent<Monster>() != null)
            {
                StartCoroutine(attack(other.gameObject.GetComponent<Monster>()));
            }
			//Debug.Log("Skill hit");
        }
    }

    // 接触持续中
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Monster>() != null)
        {
            StopCoroutine(attack(other.gameObject.GetComponent<Monster>()));
        }
    }

    public void setActive()
    {
        isActive = true;
    }

    IEnumerator skill_stay()
    {
        while(lastTime > 0)
        {
            yield return new WaitForSeconds(2);
            lastTime--;
        }
        if (lastTime <= 0)
            Destroy(this.gameObject);
    }

    IEnumerator attack(Monster monsters)
    {
        while(monsters.health>0)
        {
            monsters.applyDamage(power);
            Debug.Log("圆形区域技能扣血");
            Debug.Log(monsters.name);
            yield return new WaitForSeconds(1);
        }

    }
}
