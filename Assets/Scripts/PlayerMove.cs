using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    Animator Animator; //当前player的动作控制组件
    Transform PlayerTransform; //当前player的几何变换

    bool direction=true;  //true->right false->left

	// Use this for initialization
	void Start () {
        Animator = GetComponent<Animator>();
        //Animator.SetFloat("Speed", 0f);

        PlayerTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        bool move = false;

        float move_LR = Input.GetAxis("Horizontal");  //-1(左) 1（右）
        float speedBuf_LR =  Mathf.Abs(move_LR * 0.1f);

        float move_UD = Input.GetAxis("Vertical"); //-1下，1（上）
        Vector3 translator=new Vector3();
        float speedBuf_UD = Mathf.Abs(move_UD * 0.1f);

        if (move_LR>0.0f) //player打算往右走
        {
            if(direction)
            {
                //player本来就正在往右走                                         
            }
            else
            {
                //player本来正在往左走
                turnRight();                                
            }
            translator.x = speedBuf_LR;
            move = true;
        }
        else if(move_LR<0.0f) //player打算往左走
        {
            if (direction)
            {
                //player本来正在往右走
                turnLeft();                
                
            }
            else
            {
                //player本来正在往左走                             
            }
            translator.x = speedBuf_LR;
            move = true;
        }
       
        if(move_UD > 0.0f)
        {
            //角色试图往上走
            if(direction)
            {
                //这时候角色朝右，z轴朝上方
                translator.z = speedBuf_UD;
            }
            else
            {
                //这时候角色朝左，z轴朝下方
                translator.z = -1.0f * speedBuf_UD;
            }
            move = true;
        }
        else if(move_UD < 0.0f)
        {
            //角色试图往下走
            if(direction)
            {
                //这个时候角色朝右，z轴朝上方
                translator.z = -1.0f * speedBuf_UD;
            }
            else
            {
                //这个时候角色朝左，z轴朝下方
                translator.z = speedBuf_UD;
            }
            move = true;
        }

        if(move)
        {
            Animator.SetBool("isRun", true);
            PlayerTransform.Translate(translator);
        }
        else
        {
            Animator.SetBool("isRun", false);
        }

        
	}

    //角色向左转
    void turnLeft()
    {
        Quaternion rotator = Quaternion.Euler(0, 180, 0);
        PlayerTransform.rotation = rotator;
        direction = false;
    }

    //角色向右转
    void turnRight()
    {
        Quaternion rotator = Quaternion.Euler(0, 0, 0);
        PlayerTransform.rotation = rotator;
        direction = true;
    }
}
