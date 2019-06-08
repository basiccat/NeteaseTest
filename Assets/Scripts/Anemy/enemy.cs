using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public float blood;
    public bool alive;
    public GameObject en;
    public static enemy en1;

    void Awake()
    {
        blood = 100;
        alive = true;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (blood <= 0)
        {
            Debug.Log("怪物死亡");
            Destroy(en);
        }
    }

    public void desecrate_blood(float power)
    {
        blood -= power;
        Debug.Log("血量-10");
    }


}
