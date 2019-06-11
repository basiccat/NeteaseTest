using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalAttack : MonoBehaviour
{

    public float playerDamage;
    public bool isAttacking = false;
    public float attackCoeffi = 1.0f;

    private int countAttact = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isAttacking = false;
        if (Input.GetMouseButtonDown(1))
        {
            countAttact = 0;
            isAttacking = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            
            isAttacking = false;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster") && isAttacking && countAttact < 1)
        {
            other.gameObject.GetComponent<Monster>().applyDamage(playerDamage);
            countAttact++;
            //print("count  " + countAttact);
            isAttacking = false;
        }
    }

    public void setAttackCoeffi(float dealt)
    {
        attackCoeffi += dealt;
    }

}
