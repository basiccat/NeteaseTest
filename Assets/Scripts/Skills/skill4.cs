using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill4 : MonoBehaviour {

    public float power;//增加的攻击力
    public static skill4 s1;
    public bool isActive;
    //public float lastTime;
    public float buffDamage;
    public float buff_lastTime;//buff的持续时间
    public float height;//特效的高度，要根据不同特效调才好放到地上

    // Use this for initialization
    void Awake()
    {
        power = 15;
        s1 = this;
        //lastTime = 5.0f;
        buff_lastTime = 5.0f;
        isActive = false;
        height = 3.0f;
    }

    void Start()
    {
    //    StartCoroutine(skill_stay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //怪进入触发器
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name=="Player")
        {
            
            GameObject hit = other.gameObject;
            Player player = hit.GetComponent<Player>();
            Debug.Log("chi");
            player.changeDamage(buffDamage);
            Destroy(this.gameObject);
            //在buff到时间之后buff消失
            StartCoroutine(buff_stay(player));

        }
    }


    public void setActive()
    {
        isActive = true;
    }
    /*
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
    */
    IEnumerator buff_stay(Player p)
    {

        yield return new WaitForSeconds(5);
        p.setAttackCoeffi(-5);
    }
}
