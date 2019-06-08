using UnityEngine;
using System.Collections;
using System;

enum state:int{
	idle=0,run=1,hurt=2,clim=3
}
enum colli:int{
	enemy=0,recover=1
}


		
public class PlayerOperation : MonoBehaviour
{
    //public GameObject Player;
	private state playerState=0;
	private Rigidbody PlayerRd;
	private Animator animator;
	private int preDirection;//默认往右走
	private float blood = 100.0f;

	public float MoveSpeed;
	public float JumpHeight;
	public float playerDamage=10;
	public float angel = 45.0f;
	public bool isAttacking = false;
	void Awake()
	{
		PlayerRd = transform.GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		playerState =state.idle;
		preDirection=0;//默认往右走
	}


	void FixedUpdate()
	{
		PlayerRd.velocity = new Vector3(0f, 0, 0f);
		if (blood <= 0.0f) {
			//print ("dead");
		}
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if (!h.Equals (0)) {
			int curDirection = h > 0 ? 0 : 1;
			if(curDirection!=preDirection)
				turnDirection (curDirection);
			int t = h > 0 ? 1 : -1;
			PlayerRd.velocity = new Vector3 (MoveSpeed*t,0,PlayerRd.velocity.z);//速度
			playerState = state.run;
		} else if (!v.Equals (0)) {
			int t = v > 0 ? 1 : -1;
			PlayerRd.velocity = new Vector3 (PlayerRd.velocity.x, 0,MoveSpeed*t);
			playerState = state.run;
		} else if (Input.GetMouseButton (0)) {//right click
			playerState = state.clim;
			isAttacking = true;
		}else if (Input.GetMouseButton (2))
			playerState = state.hurt;
		else if(Input.GetMouseButtonUp(0))
			isAttacking = false;
		else
			playerState =state.idle;
		
		switch (playerState) {
		case state.idle:
			animator.SetInteger ("state", 0);
			break;
		case state.run:
			animator.SetInteger ("state", 1);
			break;
		case state.hurt:
			animator.SetInteger ("state", 2);
			break;
		case state.clim:
			animator.SetInteger ("state", 3);
			break;
		default:
			animator.SetInteger ("state", 0);
			break;
			
		}
			
	}

	private void calculateDamage(float damage)
	{
        if (blood > damage)
        {
            blood -= damage;
            Debug.Log(blood);
        }
        else
        {
            blood = 0;
            Debug.Log("Game Over!");
        }
    }
	private void turnDirection(int curDirection)
	{
		Quaternion rotator;
		if(curDirection==1)
			rotator = Quaternion.Euler (0, 180, 0);
		else
			rotator = Quaternion.Euler(0, 0, 0);
		print (preDirection);
		print (curDirection);
		preDirection = curDirection;
		transform.rotation = rotator;
	}
	public float getBlood()
	{
		return blood;
	}
	public void addBlood(float delta)
	{
		blood += delta;
	}
	public void reduceBlood(float delta)
	{
		blood -= delta;
	}




}
