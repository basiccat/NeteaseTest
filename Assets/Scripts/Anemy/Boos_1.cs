using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boos_1 : LandMonster
{

    /**************外部变量****************/

    public float Near = 6.0f;
    public float Close = 8.0f;
    public float Far = 12.0f;

    private string ProjectFireAnimStateName = "ProjectFire";
    private string ProjectFireAnimParameter = "projectFire";

    private string DashAnimStateName = "Dash";
    private string DashAnimParameter = "dash";

    private string DashPoseAnimStateName = "DashPose";
    private string DashPoseAnimParameter = "dashPose";

    private string DashAttackAnimStateName = "DashAttack";

    public GameObject FlameObject;
    public float FlameSpawnOffsetX = 5.0f;
    public float ProjectFlameWidth = 3.0f;

    public float AttackCD = 2.0f;
    public float ProjectFireCD = 2.0f;
    public float DashAttackCD = 2.0f;

    public float DashSpeed = 0.05f;

    /*************内部实现变量****************/

    //技能形态
    private enum SkillStatus
    {
        Ready,
        Busy,
        Attack,
        DashAttack,
        ProjectFire
    }

    //普通攻击的状态机
    private enum AttackStatus
    {
        StopMoving,
        StartAttacking,
        StopAttacking
    }

    //喷射火焰的状态机
    private enum ProjectFireStatus
    {
        Chasing,
        StopMoving,
        StartProjecting,
        StopProjecting
    }

    //冲刺攻击的状态机
    private enum DashAtackStatus
    {
        StopMoving,
        StartDashing,
        Dashing,
        StopDashing
    }

    //当前的技能状态
    private SkillStatus skillStatus = SkillStatus.Ready;

    //普通攻击相关
    private AttackStatus attackStatus = AttackStatus.StopMoving; //普通攻击的状态机，初始状态为停止移动

    //喷射火焰相关
    private ProjectFireStatus projectFireStatus = ProjectFireStatus.Chasing; //喷射火焰的状态机，初始状态为追踪玩家    
    private GameObject SpawnedFlame; //产生的火焰
    private bool isFireSpawn = false; //火焰喷射状态    

    //冲刺攻击相关
    private DashAtackStatus dashAttackStatus = DashAtackStatus.StopMoving; //冲刺攻击的状态机，初始状态为冲向玩家位置
    private Vector3 dashPosition;    //冲向的位置


    /**************函数****************/
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


        //设定boss的动画状态机信息
        attackAnimStateName = "Attack"; //普通攻击的动画状态的名字
        attackAnimParemeter = "attack"; //普通攻击的动画参数的名字
        moveAnimStateName = "Move"; //移动的动画状态的名字
        moveAnimParameter = "isMove"; //移动的动画参数的名字
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager._instance.isPaused)
        {
            switch (skillStatus)
            {
                case SkillStatus.Ready:
                    {
                        //如果当前没有施放技能，则根据当前状态选择技能
                        float distance = caculateTheDistance();
                        float choose = Random.Range(0.0f, 1.0f);
                        if (distance < Near)
                        {
                            //就在Player附近

                            if (choose < 0.2f)
                            {
                                //使用喷射火焰
                                projectFire();
                            }
                            else
                            {
                                //普通攻击
                                attack();
                            }
                        }
                        else if (distance < Close)
                        {
                            //中等距离                        
                            if (choose < 0.5f)
                            {
                                //使用喷射火焰
                                projectFire();
                            }
                            else
                            {
                                //冲刺攻击
                                dashAttack();
                            }
                        }
                        else if (distance < Far)
                        {
                            //较远距离                        
                            if (choose < 0.6f)
                            {
                                //喷射火焰
                                dashAttack();
                            }
                            else
                            {
                                //使用喷射火焰
                                projectFire();
                            }
                        }
                        else
                        {
                            //非常遥远                                                
                            if (choose < 0.9f)
                            {
                                //冲刺攻击
                                dashAttack();
                            }
                            else
                            {
                                //使用喷射火焰
                                projectFire();
                            }
                        }
                        break;
                    }
                case SkillStatus.Busy:
                    {
                        chaseAndAttackPlayer();
                        break;
                    }
                case SkillStatus.DashAttack:
                    {
                        dashAttack();
                        break;
                    }
                case SkillStatus.Attack:
                    {
                        attack();
                        break;
                    }
                case SkillStatus.ProjectFire:
                    {
                        projectFire();
                        break;
                    }
            }
        }
        
    }

    /*计算与玩家的直线距离*/
    protected float caculateTheDistance()
    {
        Vector3 PlayerPosition = PlayerObject.GetComponent<Transform>().position;

        float xDistance = MonsterTransform.position.x - PlayerPosition.x;
        float yDistance = MonsterTransform.position.z - PlayerPosition.z;

        float distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

        return distance;
    }

    /*普通攻击*/
    protected void attack()
    {

        if (skillStatus != SkillStatus.Attack)
        {
            //开始攻击
            skillStatus = SkillStatus.Attack;
            MonsterAnimator.SetBool(moveAnimParameter, false);
        }
        else
        {
            AnimatorStateInfo AnimatorStateInfo = MonsterAnimator.GetCurrentAnimatorStateInfo(0); //储存当前的动画状态            
            switch (attackStatus)
            {
                case AttackStatus.StopMoving:
                    {
                        //正在从跑步状态返回Idle状态
                        if (AnimatorStateInfo.IsName("Idle"))
                        {
                            //成功回到Idle状态
                            attackStatus = AttackStatus.StartAttacking; //开始从Idle状态变成进攻
                            MonsterAnimator.SetTrigger(attackAnimParemeter);
                        }
                        break;
                    }
                case AttackStatus.StartAttacking:
                    {
                        //正在从静止状态变成Attack状态
                        if (AnimatorStateInfo.IsName(attackAnimStateName))
                        {
                            //成功变成攻击状态
                            attackStatus = AttackStatus.StopAttacking;
                        }
                        break;
                    }
                case AttackStatus.StopAttacking:
                    {
                        //正在从攻击状态变成Idle状态
                        if (AnimatorStateInfo.IsName("Idle"))
                        {
                            //成功回到idle状态，完成普通攻击
                            attackStatus = AttackStatus.StopMoving;  //reset状态
                            skillStatus = SkillStatus.Busy;
                            StartCoroutine(recoverFromSkill(AttackCD));
                        }
                        break;
                    }
            }
        }
    }

    /*冲刺爪击*/
    protected void dashAttack()
    {
        //Debug.Log(dashAttackStatus);
        if (skillStatus != SkillStatus.DashAttack)
        {
            skillStatus = SkillStatus.DashAttack;
            MonsterAnimator.SetBool(moveAnimParameter, false); //停止跑步动作
            dashPosition = TargetPlayer.GetComponent<Transform>().position;
        }
        else
        {
            AnimatorStateInfo AnimatorStateInfo = MonsterAnimator.GetCurrentAnimatorStateInfo(0); //储存当前的动画状态            
            switch (dashAttackStatus)
            {
                case DashAtackStatus.StopMoving:
                    {
                        if (AnimatorStateInfo.IsName("Idle"))
                        {
                            //成功回到Idle状态
                            dashAttackStatus = DashAtackStatus.StartDashing;
                            MonsterAnimator.SetTrigger(DashAnimParameter);
                        }
                        break;
                    }
                case DashAtackStatus.StartDashing:
                    {
                        if (AnimatorStateInfo.IsName(DashAnimStateName))
                        {
                            //正在做dash动作
                            dashAttackStatus = DashAtackStatus.Dashing;
                            MonsterAnimator.SetBool(DashPoseAnimParameter, true);
                        }
                        break;
                    }
                case DashAtackStatus.Dashing:
                    {
                        if (AnimatorStateInfo.IsName(DashPoseAnimStateName))
                        {
                            //正在维持dash的姿势，这时应该让boss冲刺到dash位置
                            float xDistance = MonsterTransform.position.x - dashPosition.x;
                            float yDistance = MonsterTransform.position.z - dashPosition.z;

                            bool isMove = false;
                            Vector3 translator = new Vector3();

                            if (xDistance > DashSpeed)
                            {
                                //怪物在玩家的右侧且尚未接近
                                isMove = true;
                                turnLeft();
                                translator.x = DashSpeed;
                                direction = true;
                            }
                            else if (xDistance < -1.0f * DashSpeed)
                            {
                                //怪物在玩家的左侧且尚未接近
                                isMove = true;
                                turnRight();
                                translator.x = DashSpeed;
                                direction = false;
                            }

                            if (yDistance > DashSpeed)
                            {
                                //怪物在玩家的上方侧且尚未接近
                                isMove = true;
                                if (direction)
                                {
                                    //怪物面向左侧，z轴正方向向下
                                    translator.z = DashSpeed;
                                }
                                else
                                {
                                    //怪物面向右侧，z轴正方向向上
                                    translator.z = -1.0f * DashSpeed;
                                }

                            }
                            else if (yDistance < -1.0f * DashSpeed)
                            {
                                //怪物在玩家的下方且尚未接近
                                isMove = true;
                                if (direction)
                                {
                                    //怪物面向左侧，z轴正方向向下
                                    translator.z = -1.0f * DashSpeed;
                                }
                                else
                                {
                                    //怪物面向右侧，z轴正方向向上
                                    translator.z = DashSpeed;
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
                                MonsterAnimator.SetBool(DashPoseAnimParameter, false); //结束冲刺pose，开始播放攻击动画
                                dashAttackStatus = DashAtackStatus.StopDashing;
                            }
                        }
                        break;
                    }
                case DashAtackStatus.StopDashing:
                    {
                        if (AnimatorStateInfo.IsName("Idle"))
                        {
                            //成功播放完攻击动画，回到Idle状态
                            dashAttackStatus = DashAtackStatus.StopMoving; //reset状态
                            skillStatus = SkillStatus.Busy;
                            StartCoroutine(recoverFromSkill(DashAttackCD));
                        }
                        break;
                    }
            }
        }
    }

    /*喷射火焰*/
    protected void projectFire()
    {
        if (skillStatus != SkillStatus.ProjectFire)
        {
            skillStatus = SkillStatus.ProjectFire;
            MonsterAnimator.SetBool(moveAnimParameter, false);
        }
        else
        {
            AnimatorStateInfo AnimatorStateInfo = MonsterAnimator.GetCurrentAnimatorStateInfo(0); //储存当前的动画状态
            switch (projectFireStatus)
            {
                case ProjectFireStatus.Chasing:
                    {
                        Vector3 playerPosition = PlayerObject.GetComponent<Transform>().position;
                        float zOffset = playerPosition.z - MonsterTransform.position.z;
                        if (Mathf.Abs(zOffset) < ProjectFlameWidth)
                        {
                            //z轴上和玩家的距离已经进入了范围
                            projectFireStatus = ProjectFireStatus.StopMoving;
                        }
                        else
                        {
                            //还没进入射程，向玩家靠近
                            Vector3 translator = new Vector3();
                            if (direction)
                            {
                                //boss面向左边,z轴朝下
                                translator.z = -1.0f * speed;
                            }
                            else
                            {
                                //boos面向右边，z轴朝上
                                translator.x = speed;
                            }
                            MonsterTransform.Translate(translator);
                        }
                        break;
                    }

                case ProjectFireStatus.StopMoving:
                    {
                        if (AnimatorStateInfo.IsName("Idle"))
                        {
                            //已经回到Idle状态
                            projectFireStatus = ProjectFireStatus.StartProjecting;
                            MonsterAnimator.SetTrigger(ProjectFireAnimParameter); //开始播放喷火动画
                        }
                        break;
                    }
                case ProjectFireStatus.StartProjecting:
                    {
                        if (AnimatorStateInfo.IsName(ProjectFireAnimStateName))
                        {
                            //已经开始播放喷火动画
                            projectFireStatus = ProjectFireStatus.StopProjecting;
                            if (!isFireSpawn)
                            {
                                //还没生成火焰,则在Boss身前生成一个火焰球       
                                Vector3 SpawnPosition = MonsterTransform.position;
                                SpawnPosition.x = direction ? (MonsterTransform.position.x + -1.0f * FlameSpawnOffsetX) : (MonsterTransform.position.x + FlameSpawnOffsetX);

                                Quaternion SpawnDirection = direction ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
                                SpawnedFlame = GameObject.Instantiate(FlameObject, SpawnPosition, SpawnDirection);
                                SpawnedFlame.SetActive(true);
                                isFireSpawn = true;
                            }
                        }
                        break;
                    }
                case ProjectFireStatus.StopProjecting:
                    {
                        if (AnimatorStateInfo.IsName("Idle"))
                        {
                            //已经停下了喷射火焰的动作
                            Destroy(SpawnedFlame);
                            isFireSpawn = false;
                            projectFireStatus = ProjectFireStatus.StopMoving;//重置喷射火焰状态机
                            skillStatus = SkillStatus.Busy;
                            StartCoroutine(recoverFromSkill(ProjectFireCD));
                        }
                        break;
                    }
            }

        }

    }

    //一定时间后恢复到可以释放技能的状态
    protected IEnumerator recoverFromSkill(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        skillStatus = SkillStatus.Ready;
    }

}
