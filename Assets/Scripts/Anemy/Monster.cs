using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {

    /*需要知道的一些GameObject*/
    protected GameObject GameManagerObject;
    protected GameManager GameManager;
    protected GameObject PlayerObject;
    protected Player TargetPlayer;
    protected GameObject DoorObject;
    protected Door Door;

    /*自身的一些Component*/
    protected Animator MonsterAnimator;
    protected Transform MonsterTransform;

    /*属性值*/
    public bool alive=true; //怪物是否活着
    public float angryValue=0.0f; //怒气值,暂时对外暴露，调试好后为固定数值
    public float health = 100.0f; //生命值，暂时对外暴露，调试好后为固定数值
    public float speed =0.1f; //移动速度，暂时对外暴露，调试好后为固定数值
    public float angryValueBoarder=60.0f; //发怒所需要的怒气值,暂时对外暴露，调试好后为怪物的固定数值

    protected bool direction;  //true->left false->right
    
    /*规则相关*/
    public float DoorAttackDistance=5.0f; //移动到距离门多近时候会停止
    public float PlayerAttackDistanceX = 1.5f; //x方向上接近玩家多少就会停止接近
    public float PlayerAttackDistanceZ = 0.1f; //y方向上接近到玩家多少距离就会停止接近


    void Start () {
		//初始化赋值
		//health = 100.0f
		GameManagerObject = GameObject.FindGameObjectsWithTag("GameManager")[0];
        GameManager = GameManagerObject.GetComponent<GameManager>();
        PlayerObject = GameManager.Player;
        TargetPlayer = PlayerObject.GetComponent<Player>();
        DoorObject = GameManager.Door;
        Door = DoorObject.GetComponent<Door>();

        /*自身属性赋值*/
        MonsterAnimator = GetComponent<Animator>();
        MonsterTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
                
	}
    

    /*对外接口，用于实施伤害*/
    public void applyDamage(float damage)
    {
        if(alive)
        {
            if (health <= damage)
            {
				Debug.Log("health empty");
				health = 0;  //血量清零
                alive = false; //更新死亡状态
							   //播放死亡动画
				Destroy(gameObject);
				
			}
            else
            {
                health -= damage;
                angryValue += damage;
                //播放受击动画
            }
        }
		Debug.Log("damage receive, remain health: "+health);
        
    }
}
