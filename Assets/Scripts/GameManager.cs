using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

struct MonsterAtATime
{
	
}
class JsonObject
{
	
	public string Name { get; set; }
	public string Skill { get; set; }
	public int Age { get; set; }
	public override string ToString()
	{
		return string.Format("JsonObject:name:{0},age={1},skill={2}", Name, Age, Skill);
	}
}
public class GameManager : MonoBehaviour {

    public static GameManager _instance;
    public bool isPaused;
    public GameObject myMenu;
    public GameObject Player;
    public GameObject Door;
	public GameObject[] Monsters;
    public Slider backgroundSlider;
    public Slider voiceSlider;
    public GameObject GameOver;
    public GameObject Win;

	JsonData waves;
	Vector3 pos =new Vector3(9.5f, 0.0f, 15.0f);
	Quaternion rot = Quaternion.Euler(0, 180, 0);
    
	IEnumerator CreateMethod()
	{
		//对于n波怪物
		for (int OnGoingWave = 0; OnGoingWave < waves.Count; OnGoingWave++)
		{
			JsonData monster = waves[OnGoingWave]["monster"];
			float waveTime = (float)waves[OnGoingWave]["time"];
			//Debug.Log(waveTime);//这一波怪物持续的时间

			Vector3 pos = new Vector3(9.5f, 0.0f, 15.0f);
			Quaternion rot = Quaternion.Euler(0, 180, 0);
			for (int m=0; m<monster.Count;m++ )
			{
				pos = new Vector3((float)monster[m]["posx"], (float)monster[m]["posy"], (float)monster[m]["posz"]);
				rot = Quaternion.Euler(0, (int)monster[m]["rot"], 0);
				switch ((int)monster[m]["type"])
				{
					case 0:
						
						GameObject monster0Clone = (GameObject)GameObject.Instantiate(Monsters[0], pos, rot);   
						monster0Clone.SetActive(true);
						yield return new WaitForSeconds(0.5f);
						break;
					case 1:
						GameObject monster1Clone = (GameObject)GameObject.Instantiate(Monsters[1], pos, rot);
						monster1Clone.SetActive(true);
						yield return new WaitForSeconds(0.5f);
						break;
					default:
						break;
				}
				
			}
			yield return new WaitForSeconds(waveTime);
		}
			
		
	}
    private void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	public void LoadMonster()
	{
		string contents = System.IO.File.ReadAllText(@"Assets\Resources\Data\waves.json");//json数据文件的相对路径
																						  //Debug.Log("contents = " + contents);
		waves = JsonMapper.ToObject(contents)["waves"];//得到一个jsondata数组
													   //Debug.Log(waves);

		StartCoroutine(CreateMethod());//生成怪物的协程，独立
	}
    private void Pause()
    {
        Debug.Log("暂停");
        isPaused = true;
        myMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
    }
	// Update is called once per frame
	private void Update () {
        if (SceneManager.GetActiveScene().name == "FirstScene" && GameOver.activeSelf != true && Win.activeSelf!=true) 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
            gameObject.GetComponent<AudioSource>().volume = backgroundSlider.value;
        }

	}
    private void unPause()
    {
        isPaused = false;
        myMenu.SetActive(false);
        //Time.timeScale = 0;
        Cursor.visible = false;
    }
    public void continueGame()
    {
        unPause();
    }

   public void newGame()
    {
        SceneManager.LoadScene("FirstScene"); 
    }
    public void returnMain()
    {
        SceneManager.LoadScene("Menu");
    }
    public void selectLevel()
    {
        SceneManager.LoadScene("choose");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
