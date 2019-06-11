using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {
    GameObject GameManagerObject;
    GameManager GameManager;
    GameObject PlayerObject;
    Player Player;

    public int frameBetweenDamage = 50;
    int FramesTowait=0;
    bool damageApplyed = false;
	// Use this for initialization
	void Start () {
        GameManagerObject = GameObject.FindGameObjectsWithTag("GameManager")[0];
        GameManager = GameManagerObject.GetComponent<GameManager>();
        PlayerObject = GameManager.Player;
        Player = PlayerObject.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!damageApplyed)
        {
            damageApplyed = true;
            FramesTowait = frameBetweenDamage;
            //Debug.Log();
            if(other.name == "Player")
            {
                Player player = other.GetComponent<Player>();
                //Debug.Log(player.transform.position);
                player.applyDamage(5.0f);
            }
        }
        else
        {
            FramesTowait--;
            if(FramesTowait<=0)
            {
                //已经度过了伤害间隔
                damageApplyed = false;                
            }
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        damageApplyed = false;


    }

    // Update is called once per frame
    void Update () {
		
	}



}
