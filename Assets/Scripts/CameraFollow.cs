using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public GameObject followingObj;
	public float xAxisBiasValue = 6.5f;
	public float zAxisBiasValue = -24f;
	public float yAxisBiasValue = 6.8f;
	public float smooth = 2f;
	public float minX=-10f;
	public float maxX=33f;
	public float minZ=-23f;
	public float maxZ=0f;



	private Vector3 targetPos;//the position that camera is going to be

	// Use this for initialization
	void Start()
	{
		targetPos = followingObj.GetComponent<Transform>().position +
			Vector3.forward * zAxisBiasValue + Vector3.right * xAxisBiasValue +
			Vector3.up * yAxisBiasValue;
		//targetPos = new Vector3(followingObj.GetComponent<Transform>().position.x+xAxisBiasValue,
		//	yAxisValue, followingObj.GetComponent<Transform>().position.z + zAxisBiasValue);
		transform.position = targetPos;
	}

	// Update is called once per frame
	void Update()
	{
		targetPos = followingObj.GetComponent<Transform>().position +
			Vector3.forward * zAxisBiasValue + Vector3.right * xAxisBiasValue +
			Vector3.up * yAxisBiasValue;
		targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
		targetPos.z = Mathf.Clamp(targetPos.z, minZ, maxZ);
		//targetPos = new Vector3(followingObj.GetComponent<Transform>().position.x + xAxisBiasValue,
		//	yAxisValue, followingObj.GetComponent<Transform>().position.z + zAxisBiasValue);

		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
	}
}
