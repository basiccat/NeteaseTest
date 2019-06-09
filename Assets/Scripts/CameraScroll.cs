using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {


	//public GameObject cam;
	//public GameObject PlayerObj;

	//public float distance;//相机到目标的距离
	//public float rotateSpeed = 2;//相机视野旋转系数

	public float minSize = 6f;
	public float maxSize = 12f;
	public float scrollSpeed = 1f;//相机视野缩放系数
	public float resumeSpeed = 2f;//相机视野回复系数


	private bool isScroll = false;
	private float size=6f;
	

	// Use this for initialization
	void Start () {

		gameObject.GetComponent<Camera>().orthographicSize = size;//直视（非透视）摄像机参数，size

	}
	
	// Update is called once per frame
	void Update () {

		//处理视野的拉近拉远效果
		
		if (isScroll)
		{
			ScrollView();
		}
		else
		{
			ResumeView();
		}
			
		
	}

	public void scrollState(bool s)
	{
		isScroll = s;
	}

	void ResumeView()//调用回复相机view大小
	{

		//decrease 0.01 because float value can't decrease to exactly min size
		//prevent calling resume over and over again
		size = Mathf.Lerp(size, (minSize-0.01f), resumeSpeed * Time.deltaTime);
		size = Mathf.Clamp(size, (minSize - 0.01f), 16f);//限定距离最小及最大值
		gameObject.GetComponent<Camera>().orthographicSize = size;
		Debug.Log(size);
	}

	void ScrollView()//调用放大相机view大小
	{
		//gameObject.GetComponent<Camera>().orthographicSize;
		//distance = offsetPosition.magnitude;//得到偏移向量的长度

		//value = Mathf.lerp(value, targetValue, scrollSpeed*Time.deltaTime);
		size = Mathf.Lerp(size, maxSize, scrollSpeed * Time.deltaTime);
		//size += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;//获取鼠标中键*相机视野缩放系数
		size = Mathf.Clamp(size, minSize, maxSize);//限定距离最小及最大值
		gameObject.GetComponent<Camera>().orthographicSize = size;
		Debug.Log(size);
		//offsetPosition = offsetPosition.normalized * distance;//更新位置偏移
	}

	

}
