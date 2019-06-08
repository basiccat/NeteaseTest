using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Xml;

public class ReadJson : MonoBehaviour {

	readonly int level = 0;//using as level
	public Text Speaker;
	public Text Content;
	public string JsonPath;
	//private GameObject roleA;//角色图像
	//private GameObject roleB;

	private List<string> dialogues_list;//存放dialogues的list
	private int dialogue_index = 0;//对话索引
	private int dialogue_count = 0;//对话数量
	private string role;//当前在说话的角色
	private string role_detail;//当前在说话的内容。

	// Use this for initialization
	void Start () {
		//roleA = GameObject.Find("Canvas/roleA");
		//roleB = GameObject.Find("Canvas/roleB");
		//roleName = GameObject.Find("Canvas/Image/roleName");
		//detail = GameObject.Find("Canvas/Image/detail");
		//roleA.SetActive(false);
		//roleB.SetActive(false);


		XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”  
		dialogues_list = new List<string>();//初始化存放dialogues的list
		//string data = System.IO.File.ReadAllText(@"Asset\TextUI.xml");
		string data = Resources.Load(@"Data\TextUI").ToString();
		xmlDocument.LoadXml(data);//载入这个xml  
		XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;//选择<dialogues>为根结点并得到旗下所有子节点  
		foreach (XmlNode xmlNode in xmlNodeList)//遍历<dialogues>下的所有节点<dialogue>压入List
		{
			XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<dialogue>  
			dialogues_list.Add(xmlElement.ChildNodes.Item(0).InnerText + "," + xmlElement.ChildNodes.Item(1).InnerText);
			//将角色名和对话内容存入这个list，中间存个逗号一会儿容易分割
		}
		dialogue_count = dialogues_list.Count;//获取到底有多少条对话
		Dialogues_handle(0);//载入第一条对话的场景
		


		//string contents = System.IO.File.ReadAllText(@JsonPath);
		////string contents = System.IO.File.ReadAllText(@"Assets\Json\TextUI.json");
		//JsonData levels = JsonMapper.ToObject(contents)["level"];
		//for (int i=0; i<levels[level].Count; i++)
		//{
		//	Debug.Log("speaker: "+(string)levels[level]["conversation"][i]["speaker"]);
		//	Debug.Log("content: "+(string)levels[level]["conversation"][i]["content"]);
		//	Speaker.text = (string)levels[level]["conversation"][i]["speaker"];
		//	Content.text = (string)levels[level]["conversation"][i]["content"];
		//}
		
	}
	
	// Update is called once per frame
	void Update () {
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
			}
		}
		
	}

	/*处理每一条对话的函数，就是将dialogues_list每一条对话弄到场景*/
	private void Dialogues_handle(int dialogue_index)
	{
		//切割数组
		string[] role_detail_array = dialogues_list[dialogue_index].Split(',');//list中每一个对话格式就是“角色名,对话”
		role = role_detail_array[0];
		role_detail = role_detail_array[1];

		//switch (role)//根据角色名
		//{   //显示当前说话的角色
		//	case "A":
		//		roleA.SetActive(true);
		//		roleB.SetActive(false);
		//		Speaker.text = "A:";
		//		break;
		//	case "B":
		//		roleB.SetActive(true);
		//		roleA.SetActive(false);
		//		Speaker.text = "B:";
		//		break;
		//}
		Speaker.text = role;
		Content.text = role_detail;//并加载当前的对话
	}

}
