using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill2 : MonoBehaviour {

    public float power;
    public static skill2 s1;
    public bool isActive;
    public float lastTime;
    public float height;//特效的高度，要根据不同特效调才好放到地上

    // Use this for initialization
    void Awake()
    {
        
        s1 = this;
        lastTime = 5.0f;
        isActive = false;
        height = 0f;
    }

    void Start()
    {
        StartCoroutine(skill_stay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //角色进入触发器
    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            GameObject hit = other.gameObject;
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                StartCoroutine(cure(player));
            }
            //Debug.Log("Skill hit");
        }
        
    }

    /*
    void OnTriggerStay(Collider other)
    {
        Debug.Log("碰撞");
    }
    */

    // 离开触发器
    void OnTriggerExit(Collider other)
    {
        GameObject hit = other.gameObject;
        Player player = hit.GetComponent<Player>();
        if (player != null)
        {
            StopCoroutine(cure(player));
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
            Destroy(this.gameObject);
    }

    IEnumerator cure(Player player)
    {
        while(true)
        {
            player.addBlood(power);
            //Debug.Log("加血");
            yield return new WaitForSeconds(1);
        }

    }
}
