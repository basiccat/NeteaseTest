using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Text;

public class ReadXml : MonoBehaviour {
	public Text Speaker;
	public Text Content;
	public string XmlPath;
	public GameObject roleA;//角色图像
	public GameObject roleB;
	public GameObject Characters;
	

	private List<string> dialogues_list;//存放dialogues的list
	private int level_index=0;//为了区分关卡的对话做出的index
	private int dialogue_index = 0;//对话索引
	private int dialogue_count = 0;//对话数量
	private string role;//当前在说话的角色
	private string role_detail;//当前在说话的内容。
							   // Use this for initialization
	private XmlDocument xmlDocument;//XML文档，用于从字符串-->类
	private XmlNodeList dialogues;

	void Awake()
	{

		//Time.timeScale = 0;
		//Characters.SetActive(false);
		
		//	xmlDocument = new XmlDocument();//新建一个XML“编辑器”  
		//	string data = Resources.Load(@XmlPath).ToString();
		//	xmlDocument.LoadXml(data);//载入这个xml  
		//	dialogues = xmlDocument.SelectSingleNode("dialogues").ChildNodes;//选择<dialogues>为根结点并得到旗下所有子节点  
		
		//Debug.Log("Total level num: "+dialogues.Count);
		//XmlNodeList level = dialogues[level_index].ChildNodes;//取出相应关卡（level_index）的节点<level>旗下的全部子节点
		//Debug.Log("dialogues num: " + level.Count);
		//dialogues_list = new List<string>();//初始化存放dialogues的list
		//foreach (XmlNode dialogue in level)//遍历<dialogues>下的所有节点<level>压入List
		//{
		//	XmlElement xmlElement = (XmlElement)dialogue;//对于任何一个元素，其实就是每一个<level>  
		//	dialogues_list.Add(xmlElement.ChildNodes.Item(0).InnerText + "," + xmlElement.ChildNodes.Item(1).InnerText);
		//	将角色名和对话内容存入这个list，中间存个逗号一会儿容易分割
		//}

		//dialogue_count = dialogues_list.Count;//获取到底有多少条对话
		//Dialogues_handle(0);//载入第一条对话的场景
		//level_index++;
	}
	private void OnEnable()
	{
		Time.timeScale = 0;
		Characters.SetActive(false);
		if (level_index==0)
		{
			//Debug.Log("Initialte conversation.");
			xmlDocument = new XmlDocument();//新建一个XML“编辑器”  
			string data = Resources.Load(@XmlPath).ToString();
			xmlDocument.LoadXml(data);//载入这个xml  
			dialogues = xmlDocument.SelectSingleNode("dialogues").ChildNodes;//选择<dialogues>为根结点并得到旗下所有子节点  
		}

		if (level_index<dialogues.Count){
			//Debug.Log("level index: " + level_index);
			XmlNodeList level = dialogues[level_index].ChildNodes;//取出相应关卡（level_index）的节点<level>旗下的全部子节点
			//Debug.Log("dialogues num: " + level.Count);
			dialogues_list = new List<string>();//初始化存放dialogues的list
			foreach (XmlNode dialogue in level)//遍历<dialogues>下的所有节点<level>压入List
			{
				XmlElement xmlElement = (XmlElement)dialogue;//对于任何一个元素，其实就是每一个<level>  
				dialogues_list.Add(xmlElement.ChildNodes.Item(0).InnerText + "," + xmlElement.ChildNodes.Item(1).InnerText);
				//将角色名和对话内容存入这个list，中间存个逗号一会儿容易分割
			}

			dialogue_count = dialogues_list.Count;//获取到底有多少条对话
			Dialogues_handle(0);//载入第一条对话的场景
			level_index++;
		}
		
	}
	void Start () {
		

		//Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update()
	{
        
		if (Input.GetMouseButtonDown(0))//如果点击了鼠标左键
		{
			dialogue_index++;//对话跳到一下个
			if (dialogue_index < dialogue_count)//如果对话还没有完
			{
				Dialogues_handle(dialogue_index);//那就载入下一条对话
			}
			else
			{ //对话完了
			  //进入下一游戏场景之类的
				gameObject.SetActive(false);//隐藏对话框
				Characters.SetActive(true);
                if (!GameManager._instance.bossCome)
                {
                    GameManager._instance.LoadMonster();
                }
                Time.timeScale = 1;
                GameManager._instance.isPaused = false;
                dialogue_index = 0;

            }
		}
	}
	//string s;
	//float speed = 0;
	//// Update is called once per frame
	//void Update()
	//{
	//	if (dialogue_index == 0)
	//	{
	//		string[] role_detail_array = dialogues_list[dialogue_index].Split(',');//list中每一个对话格式就是“角色名,对话”
	//		role = role_detail_array[0];
	//		role_detail = role_detail_array[1];

	//		switch (role)//根据角色名
	//		{   //显示当前说话的角色
	//			case "阿伟：":
	//				roleA.SetActive(true);
	//				roleB.SetActive(false);
	//				break;
	//			case "Monster：":
	//				roleB.SetActive(true);
	//				roleA.SetActive(false);
	//				break;
	//		}
	//		speed += Time.deltaTime;

	//		Content.text = s.Substring(0, (int)speed + 1);
	//		Speaker.text = role;
	//		//Content.text = role_detail;//并加载当前的对话
	//	}
	//	if (Input.GetMouseButtonDown(0))//如果点击了鼠标左键
	//	{
	//		dialogue_index++;//对话跳到一下个
	//		if (dialogue_index < dialogue_count)//如果对话还没有完
	//		{
	//			string[] role_detail_array = dialogues_list[dialogue_index].Split(',');//list中每一个对话格式就是“角色名,对话”
	//			role = role_detail_array[0];
	//			role_detail = role_detail_array[1];

	//			switch (role)//根据角色名
	//			{   //显示当前说话的角色
	//				case "阿伟：":
	//					roleA.SetActive(true);
	//					roleB.SetActive(false);
	//					break;
	//				case "Monster：":
	//					roleB.SetActive(true);
	//					roleA.SetActive(false);
	//					break;
	//			}
	//			Speaker.text = role;
	//			Content.text = role_detail;//并加载当前的对话
	//		}
	//		else
	//		{ //对话完了
	//		  //进入下一游戏场景之类的
	//			gameObject.SetActive(false);//隐藏对话框
	//			Characters.SetActive(true);
	//			manager.GetComponent<GameManager>().LoadMonster();
	//		}
	//	}
	//}

	/*处理每一条对话的函数，就是将dialogues_list每一条对话弄到场景*/
	private void Dialogues_handle(int dialogue_index)
	{
		//切割数组
		string[] role_detail_array = dialogues_list[dialogue_index].Split(',');//list中每一个对话格式就是“角色名,对话”
		role = role_detail_array[0];
		role_detail = role_detail_array[1];

		switch (role)//根据角色名
		{   //显示当前说话的角色
			case "阿伟：":
				roleA.SetActive(true);
				roleB.SetActive(false);
				break;
			case "？？？：":
				roleB.SetActive(true);
				roleA.SetActive(false);
				break;
			case "杰哥：":
				roleB.SetActive(true);
				roleA.SetActive(false);
				break;
		}
		Speaker.text = role;
		Content.text = role_detail;//并加载当前的对话
		
	}
}
