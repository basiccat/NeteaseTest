using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMonster : Monster
{
    protected string attackAnimStateName = "SwordAttack"; //普通攻击的动画状态的名字
    protected string attackAnimParemeter = "SwordAttack"; //普通攻击的动画参数的名字
    protected string moveAnimStateName = "Run"; //移动的动画状态的名字
    protected string moveAnimParameter = "isRun"; //移动的动画参数的名字


    // Use this for initialization
    void Start()
    {
        //初始化赋值
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
    void Update()
    {
        if (!GameManager._instance.isPaused)
        {
            if (angryValue > angryValueBoarder)
            {
                chaseAndAttackPlayer();
            }
            else
            {
                //怒气值没有上升到阈值，冷静的向着城门进攻
                AttackDoor();
            }
        }

    }

    /*追踪玩家,到达指定位置进行攻击*/
    protected void chaseAndAttackPlayer()
    {
        Vector3 PlayerPosition;
        PlayerPosition = PlayerObject.GetComponent<Transform>().position;

        float xDistance = GetComponent<Transform>().position.x - PlayerPosition.x;
        float yDistance = GetComponent<Transform>().position.z - PlayerPosition.z;

        bool isMove = false;
        Vector3 translator = new Vector3();

        if (xDistance > PlayerAttackDistanceX)
        {
            //怪物在玩家的右侧且尚未接近
            isMove = true;
            turnLeft();
            translator.x = speed;
            direction = true;
        }
        else if (xDistance < -1.0f * PlayerAttackDistanceX)
        {
            //怪物在玩家的左侧且尚未接近
            isMove = true;
            turnRight();
            translator.x = speed;
            direction = false;
        }

        if (yDistance > PlayerAttackDistanceZ)
        {
            //怪物在玩家的上方侧且尚未接近
            isMove = true;
            if (direction)
            {
                //怪物面向左侧，z轴正方向向下
                translator.z = speed;
            }
            else
            {
                //怪物面向右侧，z轴正方向向上
                translator.z = -1.0f * speed;
            }

        }
        else if (yDistance < -1.0f * PlayerAttackDistanceZ)
        {
            //怪物在玩家的下方且尚未接近
            isMove = true;
            if (direction)
            {
                //怪物面向左侧，z轴正方向向下
                translator.z = -1.0f * speed;
            }
            else
            {
                //怪物面向右侧，z轴正方向向上
                translator.z = speed;
            }
        }

        MonsterAnimator.SetBool(moveAnimParameter, isMove);
        if (isMove)
        {
            //尚未到达攻击地点
            if (direction)
            {
                Quaternion quaternion = Quaternion.Euler(0, 180, 0);
                MonsterTransform.rotation = quaternion;
            }
            else
            {
                Quaternion quaternion = Quaternion.Euler(0, 0, 0);
                MonsterTransform.rotation = quaternion;
            }

            MonsterTransform.Translate(translator);
        }
        else
        {
            //已经到达攻击地点
            MonsterAnimator.SetTrigger(attackAnimParemeter); //播放攻击动画
        }
    }

    /*移动到城门前，攻击城门*/
    protected void AttackDoor()
    {
        turnLeft();
        Vector3 DoorPosition;
        DoorPosition = DoorObject.GetComponent<Transform>().position;

        float xDistance = this.GetComponent<Transform>().position.x - DoorPosition.x;
        if (xDistance > DoorAttackDistance)
        {
            // 尚未到达大门
            MonsterAnimator.SetBool("isRun", true);
            Vector3 translator = new Vector3(speed, 0.0f, 0.0f);
            MonsterTransform.Translate(translator);
        }
        else
        {
            //已经到达大门处
            MonsterAnimator.SetBool("isRun", false);
            MonsterAnimator.SetTrigger("SwordAttack"); //播放攻击动画
        }
    }

    /*怪物向左转*/
    protected void turnLeft()
    {
        Quaternion rotator = Quaternion.Euler(0, 180, 0);
        MonsterTransform.rotation = rotator;
        direction = true;
    }

    /*怪物向右转*/
    protected void turnRight()
    {
        Quaternion rotator = Quaternion.Euler(0, 0, 0);
        MonsterTransform.rotation = rotator;
        direction = false;
    }


}
