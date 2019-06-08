using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum state : int
{
    idle = 0, run = 1, attack = 2, beAttacked = 3,dead=4
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

    public Player GamePlayer;
    public float health = 100.0f;
    //public float blood = 100f;
    public float MoveSpeed;
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        PlayerRd = GetComponent<Rigidbody>();
        animator = player.GetComponent<Animator>();
        playerState = state.idle;
        preDirection = 0;//默认往右走
    }


    void FixedUpdate()
    {
        if (!GameManager._instance.isPaused)
        {
            PlayerRd.velocity = new Vector3(0f, 0, 0f);
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (!h.Equals(0))
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
                    animator.SetInteger("state", 2);
                    break;
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
    private float calculateDamage()
    {
        GameObject sword = GameObject.FindGameObjectWithTag("sword");
        float damage = sword.GetComponent<normalAttack>().playerDamage;
        return damage;
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
}
