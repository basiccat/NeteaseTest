using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Player GamePlayer;
    public float health = 100.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void applyDamage(float damage)
    {
        if (health > damage)
        {
            health -= damage;
            Debug.Log(health);
        }
        else
        {
            health = 0;
            Debug.Log("Game Over!");
        }
    }
}
