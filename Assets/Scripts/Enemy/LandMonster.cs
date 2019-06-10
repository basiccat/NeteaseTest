using UnityEngine;

public class LandMonster : Monster
{
    /**********外部变量************/
    /*动画状态机*/
    protected string attackAnimStateName = "SwordAttack"; //普通攻击的动画状态的名字
    protected string attackAnimParemeter = "SwordAttack"; //普通攻击的动画参数的名字
    protected string moveAnimStateName = "Run"; //移动的动画状态的名字
    protected string moveAnimParameter = "isRun"; //移动的动画参数的名字
    protected string IdleAnimStateName = "Idle";

    
    /*规则相关*/
    public float DoorAttackDistance = 5.0f; //移动到距离门多近时候会停止
    public float PlayerAttackDistanceX = 1.5f; //x方向上接近玩家多少就会停止接近
    public float PlayerAttackDistanceZ = 0.1f; //y方向上接近到玩家多少距离就会停止接近

    /*普通攻击相关*/
    public GameObject LM_Sword; //攻击使用的🗡
    public float LM_attackDegree = 60.0f; //普通攻击的角度     
    /**********内部变量************/

    //技能形态
    private enum LM_Status
    {        
        Chase,
        Attack
    }
    private LM_Status status;

    //普通攻击的状态机
    private enum LM_AttackStatus
    {
        StopMoving,
        StartAttacking,
        StopAttacking
    }
    private LM_AttackStatus attackStatus;
    Transform LM_SwordTransform; //🗡的transform组件
    private float LM_attack_totalTime; //攻击使用的时间
    private bool LM_attack_isStart=false; //是否已经开始攻击
    private float LM_attack_timeNow; //当前攻击状态的时间
    private float LM_attack_startDegree; //攻击开始的角度
    /**********函数************/
    void Start()
    {
        //初始化赋值
        GameManagerObject = GameObject.FindGameObjectsWithTag("GameManager")[0];
        GameManager = GameManagerObject.GetComponent<GameManager>();
        PlayerObject = GameManager.Player;
        TargetPlayer = PlayerObject.GetComponent<Player>();
        DoorObject = GameManager.Door;
        Door = DoorObject.GetComponent<Door>();
        LM_SwordTransform = LM_Sword.GetComponent<Transform>();        
        /*自身属性赋值*/
        MonsterAnimator = GetComponent<Animator>();
        MonsterTransform = GetComponent<Transform>();

        status = LM_Status.Chase;
        attackStatus = LM_AttackStatus.StopMoving;
    }

    void Update()
    {
        if (!GameManager._instance.isPaused)
        {            
            if (angryValue > angryValueBoarder)
            {                
                switch(status)
                {
                    case LM_Status.Chase:
                        {            
                            if(!ChasePlayer())
                            {
                                Attack();
                            }
                            break;
                        }
                    case LM_Status.Attack:
                        {
                            Attack();
                            break;
                        }
                }                
            }
            else
            {
                //怒气值没有上升到阈值，冷静的向着城门进攻
                AttackDoor();
            }
        }

    }

    /*追踪玩家*/
    protected bool ChasePlayer()
    {
        Vector3 PlayerPosition;
        PlayerPosition = PlayerObject.GetComponent<Transform>().position;

        float xDistance = GetComponent<Transform>().position.x - PlayerPosition.x;
        float yDistance = GetComponent<Transform>().position.z - PlayerPosition.z;

        bool shouldMove = false;
        Vector3 translator = new Vector3();

        if (xDistance > PlayerAttackDistanceX)
        {
            //怪物在玩家的右侧且尚未接近
            shouldMove = true;
            TurnLeft();
            translator.x = speed;
            direction = true;
        }
        else if (xDistance < -1.0f * PlayerAttackDistanceX)
        {
            //怪物在玩家的左侧且尚未接近
            shouldMove = true;
            TurnRight();
            translator.x = speed;
            direction = false;
        }

        if (yDistance > PlayerAttackDistanceZ)
        {
            //怪物在玩家的上方侧且尚未接近
            shouldMove = true;
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
            shouldMove = true;
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

        MonsterAnimator.SetBool(moveAnimParameter, shouldMove);
        if (shouldMove)
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
        return shouldMove;
    }

    /*普通攻击*/
    protected void  Attack()
    {        
        if (status != LM_Status.Attack)
        {
            //开始攻击
            status = LM_Status.Attack;
            MonsterAnimator.SetBool(moveAnimParameter, false);
        }
        else
        {
            AnimatorStateInfo AnimatorStateInfo = MonsterAnimator.GetCurrentAnimatorStateInfo(0); //储存当前的动画状态                        
            switch (attackStatus)
            {
                case LM_AttackStatus.StopMoving:
                    {
                        //正在从跑步状态返回Idle状态
                        if (AnimatorStateInfo.IsName(IdleAnimStateName))
                        {
                            //成功回到Idle状态
                            attackStatus = LM_AttackStatus.StartAttacking; //开始从Idle状态变成进攻
                            MonsterAnimator.SetBool(attackAnimParemeter,true);
                        }
                        break;
                    }
                case LM_AttackStatus.StartAttacking:
                    {
                        //正在从静止状态变成Attack状态                        
                        if (AnimatorStateInfo.IsName(attackAnimStateName))
                        {
                            //成功变成攻击状态                            
                            LM_attack_totalTime = AnimatorStateInfo.length; //取当前攻击动画的总时长作为整个攻击的时长
                            //开始攻击
                            if(!LM_attack_isStart)
                            {                                
                                LM_attack_timeNow = 0.0f;
                                LM_attack_isStart = true;
                                LM_Sword.GetComponent<Sword>().Blade.SetActive(true); //激活剑刃                                
                                if(direction)
                                {
                                    //因为受LandMonster自身的transform影响，导致子节点Sword的transform会绕y轴旋转180
                                    LM_attack_startDegree = -180.0f + (-1.0f * (LM_attackDegree / 2.0f));
                                    LM_SwordTransform.rotation = Quaternion.Euler(0.0f, LM_attack_startDegree, 0.0f);
                                }
                                else
                                {
                                    //面向右边
                                    LM_attack_startDegree =(-1.0f * (LM_attackDegree / 2.0f));
                                    LM_SwordTransform.rotation = Quaternion.Euler(0.0f, LM_attack_startDegree, 0.0f);
                                }
                               
                            }
                            else
                            {
                                LM_attack_timeNow += Time.deltaTime;                                
                                LM_SwordTransform.rotation = Quaternion.Euler(0.0f, LM_attack_startDegree + (LM_attack_timeNow / LM_attack_totalTime) * LM_attackDegree, 0.0f);                                
                            }                           
                        }
                        if (LM_attack_isStart && LM_attack_timeNow >= LM_attack_totalTime)
                        {
                            //已经完成了攻击
                            MonsterAnimator.SetBool(attackAnimParemeter, false);
                            attackStatus = LM_AttackStatus.StopAttacking;
                            LM_Sword.GetComponent<Sword>().Blade.SetActive(false);
                            LM_attack_isStart = false; //停止攻击
                            LM_attack_timeNow = 0.0f;
                        }
                        break;
                    }
                case LM_AttackStatus.StopAttacking:
                    {
                        //正在从攻击状态变成Idle状态
                        
                        if (AnimatorStateInfo.IsName(IdleAnimStateName))
                        {
                            //成功回到idle状态，完成普通攻击                            
                            attackStatus = LM_AttackStatus.StopMoving;  //reset状态
                            status = LM_Status.Chase;                            
                        }
                        break;
                    }
            }
        }
    }

    /*移动到城门前，攻击城门*/
    protected void AttackDoor()
    {
        TurnLeft();
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
    protected void TurnLeft()
    {
        Quaternion rotator = Quaternion.Euler(0, 180, 0);
        MonsterTransform.rotation = rotator;
        direction = true;
    }

    /*怪物向右转*/
    protected void TurnRight()
    {
        Quaternion rotator = Quaternion.Euler(0, 0, 0);
        MonsterTransform.rotation = rotator;
        direction = false;
    }


}
