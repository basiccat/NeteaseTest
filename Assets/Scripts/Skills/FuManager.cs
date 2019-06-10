using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuManager : MonoBehaviour {
    public static FuManager _instance;
    public GameObject[] skills;
    public float left;
    //private float speed = 1;

    public int currentSkillsNum = 0;   
    public int skillsNumToSave;
    //private GameObject[] currentSkills;
    //public float right;
    private GameObject activeSkill;
    private void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start () {
        //new WaitForSeconds(10);
        InvokeRepeating("activateSkills", 3.0f, 3.0f);
        //activateSkills();
    }
    private void activateSkills()
    {
        if (!GameManager._instance.isPaused)
        {
            int index = Random.Range(0, skills.Length);
            activeSkill = skills[index];
            //activeSkill.SetActive(true);
            GameObject newSkills = GameObject.Instantiate(activeSkill, transform.position, Quaternion.identity, transform);
            //若是碰撞体即继续设置enable
            newSkills.SetActive(true);
            currentSkillsNum++;
            //Debug.Log(currentSkillsNum);
            if (currentSkillsNum == skillsNumToSave)
            {
                CancelInvoke();
            }
        }
        
        //newSkills.transform.Translate(Vector3.left * Time.deltaTime, Space.World);

    }
    public void resetSkillSpeed(GameObject X)
    {
        foreach (Transform child in transform)
        {
            //child.GetComponent<FuAction>().speed = 100.0f;
            if (child.position.x > X.transform.position.x)
            {
               
                Debug.Log(child.name);
                child.GetComponent<FuAction>().speed = 100.0f;
            }
            //else { };
        }
    }

    //IEnumerator CreateTimer()
    //{

    //    yield return new WaitForSeconds(3.0f);
    //    activateSkills();
    //}
    // Update is called once per frame
    void Update () {
    }
}
