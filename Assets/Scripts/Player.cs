using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum state : int
{
    idle = 0, run = 1, attack = 2, beAttacked = 3, dead = 4
}
enum colli : int
{
    enemy = 0, recover = 1
}



public class Player : MonoBehaviour
{
    private state playerState = 0;

    private Rigidbody PlayerRd;
    private Animator animator;
    private int preDirection;//默认往右走
    private Transform transformPlayer;

    private AudioSource playerAttackAudio;

    public Player GamePlayer;
    public float health = 100.0f;
    public Slider HpStrip;
    //public float blood = 100f;
    public float MoveSpeed;
    void Awake()
    {
        playerAttackAudio = gameObject.GetComponent<AudioSource>();
        GameObject player = GameObject.FindGameObjectWithTag("player");
        PlayerRd = GetComponent<Rigidbody>();
        animator = player.GetComponent<Animator>();
        playerState = state.idle;
        preDirection = 0;//默认往右走
    }
    private void Start()
    {
        HpStrip.value = HpStrip.maxValue = health;
    }
    void FixedUpdate()
    {
        if (!GameManager._instance.isPaused)
        {
            PlayerRd.velocity = new Vector3(0f, 0, 0f);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (!h.Equals(0) && !v.Equals(0))
            {
                int curDirection = h > 0 ? 0 : 1;
                if (curDirection != preDirection)
                    turnDirection(curDirection);
                int ht = h > 0 ? 1 : -1;
                int vt = v > 0 ? 1 : -1;
                PlayerRd.velocity = new Vector3(MoveSpeed * ht, 0, MoveSpeed * vt);
                playerState = state.run;
            }
            else if (!h.Equals(0))
            {
                int curDirection = h > 0 ? 0 : 1;
                if (curDirection != preDirection)
                    turnDirection(curDirection);
                int t = h > 0 ? 1 : -1;
                PlayerRd.velocity = new Vector3(MoveSpeed * t, 0, PlayerRd.velocity.z);//速度
                playerState = state.run;
            }
            else if (!v.Equals(0))
            {
                int t = v > 0 ? 1 : -1;
                PlayerRd.velocity = new Vector3(PlayerRd.velocity.x, 0, MoveSpeed * t);
                playerState = state.run;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                playerState = state.attack;
            }
            else
                playerState = state.idle;

            switch (playerState)
            {
                case state.idle:
                    animator.SetInteger("state", 0);
                    break;
                case state.run:
                    animator.SetInteger("state", 1);
                    break;
                case state.attack:
                    {
                        animator.SetInteger("state", 2);
                        Debug.Log("aaa");
                        playerAttackAudio.Play();
                        break;
                    }

                default:
                    animator.SetInteger("state", 0);
                    break;

            }
            if (health <= 0.0f)
            {
                //Debug.Log("aaaa");
                animator.SetInteger("state", 4);
                GameManager._instance.isPaused = true;
                //new WaitForSeconds(4);
                //Time.timeScale = 0;
            }
        }


    }
    private void Update()
    {
        //实现滑动血条
        Debug.Log(health);
        HpStrip.value = health;
        //用菜单中的滑动条控制角色攻击音效
        gameObject.GetComponent<AudioSource>().volume = GameManager._instance.voiceSlider.value;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if(other.CompareTag("Monster"))
        //  animator.SetInteger("state", 3);
    }
    private void turnDirection(int curDirection)
    {
        Quaternion rotator;
        if (curDirection == 1)
            rotator = Quaternion.Euler(0, 0, 0);
        else
            rotator = Quaternion.Euler(0, 180, 0);
        preDirection = curDirection;
        transform.rotation = rotator;
    }

    public void setAttackCoeffi(float delta)
    {
        GameObject sword = GameObject.FindGameObjectWithTag("sword");
        sword.GetComponent<normalAttack>().setAttackCoeffi(delta);
    }
    public void changeDamage(float delta)
    {
        GameObject sword = GameObject.FindGameObjectWithTag("sword");
        sword.GetComponent<normalAttack>().playerDamage += delta;
    }
    public float getBlood()
    {
        return health;
    }

    public void applyDamage(float damage)
    {

        if (health > damage)
        {
            health -= damage;
            Debug.Log(health);
        }
        else
        {
            health = 0;
            Debug.Log("Game Over!");
        }
    }

    public void addBlood(float delta)
    {
        health += delta;
    }
}
