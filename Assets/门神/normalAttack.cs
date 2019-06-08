using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalAttack : MonoBehaviour {

	public float playerDamage;
	public bool isAttacking = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		isAttacking = false;
		if (Input.GetMouseButtonDown (1))
			isAttacking = true;
		if(Input.GetMouseButtonUp(1))
			isAttacking = false;
		
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag ("Monster") && isAttacking) {
			other.gameObject.GetComponent<Monster> ().applyDamage(playerDamage);
			isAttacking = false;
		}
	}
	
}
