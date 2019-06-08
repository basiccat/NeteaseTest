using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FuAction : MonoBehaviour, IPointerDownHandler
{
    //写成public只为了让FuManager获取
    public float speed = 1;
    //需要被实例化的预制
    public GameObject inistatePrefab;
    //实例化后的对象
    private GameObject inistateObj;
    // Use this for initialization
    void Start () {
        if (inistatePrefab == null)
            return;
        //实例化预制
        inistateObj = Instantiate(inistatePrefab) as GameObject;
        inistateObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager._instance.isPaused)
        {
            if (transform.position.x > FuManager._instance.left)
            {
                //Debug.Log(transform.position.x);
                transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
            }
            else
            {
                speed = 0;
            }
        }

        //Debug.Log(speed);
	}


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameManager._instance.isPaused)
        {
            //Debug.Log(gameObject.name);
            inistateObj.SetActive(true);
            SelectObjManager.Instance.AttachNewObject(inistateObj);
            FuManager._instance.resetSkillSpeed(gameObject);
            Destroy(gameObject);
            FuManager._instance.currentSkillsNum -= 1;
            //Debug.Log(speed);
            float fuck = FuManager._instance.skillsNumToSave - FuManager._instance.currentSkillsNum;
            //Debug.Log(speed);
            FuManager._instance.Invoke("activateSkills", fuck * 3.0f);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.GetComponent<FuAction>().speed);
        if (collision.collider.tag == "Skills" && collision.collider.GetComponent<FuAction>().speed==0) {
            speed = 0;
            //Debug.Log(collision.collider.GetComponent<FuAction>().speed);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Skills" && collision.collider.GetComponent<FuAction>().speed == 0)
        {
            speed = 0;
            //Debug.Log(collision.collider.GetComponent<FuAction>().speed);
        }
    }
}
