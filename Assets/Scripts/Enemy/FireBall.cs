using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考https://blog.csdn.net/qq_35539447/article/details/80486247?tdsourcetag=s_pctim_aiomsg中的贝塞尔曲线算法
public class FireBall : MonoBehaviour {

    /****************外部变量*******************/
    public float FlySpeed=0.01f;
    public float Damage = 5.0f;

    public float life = 5.0f;

    //public float TotoalSeconds = 3f;
    //public float timeNow = 0;

    /**贝塞尔弧线计算函数**/
    //protected Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    //{
    //    Vector3 p0p1 = (1 - t) * p0 + t * p1;
    //    Vector3 p1p2 = (1 - t) * p1 + t * p2;
    //    Vector3 result = (1 - t) * p0p1 + t * p1p2;
    //    return result;
    //}

    /****************贝塞尔弧线式喷发内部变量*******************/
    //private Vector3 StartPosition = new Vector3();
    //private Vector3 AirPosition= new Vector3();
    //private Vector3 GroundPosition= new Vector3();

    //public void setStartPosition(Vector3 value) { StartPosition = value; }
    //public void setAirPosition(Vector3 value) { AirPosition = value; }
    //public void setGroundPosition(Vector3 value) { GroundPosition = value; }
    /****************属性*******************/
    private bool direction = true; //true => left,false => right
    public void setDirection(bool dir) { direction = dir; }

    private float RotationY = 0;
    public void setEuler(float Euler_) { RotationY = Euler_; }

    private float lifeTimeNow = 0.0f;

    void Start () {
		if(!direction)
        {
            transform.Rotate(0f, 180f, 0f);
        }
	}
		
	void Update () {

        /**贝塞尔弧线式喷发**/
        //timeNow += Time.deltaTime;
        //if (timeNow > TotoalSeconds)
        //{
        //    timeNow= 0f;
        //}
        //Vector3 positon = Bezier(StartPosition, AirPosition, GroundPosition, timeNow / TotoalSeconds);
        //transform.position = positon;

        lifeTimeNow += Time.deltaTime;
        if(lifeTimeNow>=life)
        {
            Destroy(gameObject);
        }

        //向左
        transform.Translate(new Vector3(Mathf.Abs(FlySpeed * Mathf.Cos(Mathf.Deg2Rad * RotationY)), 0.0f,-1.0f* FlySpeed * Mathf.Sin(Mathf.Deg2Rad * RotationY)));        
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="Player")
        {
            other.GetComponent<Player>().applyDamage(Damage);
            Destroy(gameObject);
        }
    }
}
