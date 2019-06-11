using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FuAction : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler
{
    //写成public只为了让FuManager获取
    public float speed = 1;
    //需要被实例化的预制
    public GameObject inistatePrefab;
    //实例化后的对象
    private GameObject inistateObj;

    public bool showMessage;
    public string skillName;
    public GameObject cam;//camera
    // Use this for initialization

    public float height;

    void Start () {
        if (inistatePrefab == null)
            return;
        //实例化预制
        inistateObj = Instantiate(inistatePrefab) as GameObject;
        inistateObj.SetActive(false);
        showMessage = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager._instance.isPaused)
        {
            if (transform.position.x >FuManager._instance.transform.position.x+ FuManager._instance.left)
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
            SelectObjManager.Instance.AttachNewObject(inistateObj,height);
            FuManager._instance.resetSkillSpeed(gameObject);
            Destroy(gameObject);
            FuManager._instance.currentSkillsNum -= 1;
            //Debug.Log(speed);
            float fuck = FuManager._instance.skillsNumToSave - FuManager._instance.currentSkillsNum;
            //Debug.Log(speed);
            FuManager._instance.Invoke("activateSkills", fuck * FuManager._instance.skillGTime);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        showMessage = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showMessage = false;
    }
    void OnGUI()
    {
        if (showMessage)
        {
            GUIStyle style1 = new GUIStyle();
            style1.fontSize = 30;
            style1.normal.textColor = Color.red;
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 400, 50), skillName, style1);

        }
    }
}
