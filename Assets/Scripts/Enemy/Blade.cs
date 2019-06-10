using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {
    /*********外部参数**********/
    public float damage = 5.0f;

    /*********内部参数**********/
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {        
        if(other.name=="Player")
        {
            //与玩家接触
            other.GetComponent<Player>().applyDamage(damage);
        }
    }
}
