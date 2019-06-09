using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContinueGame : MonoBehaviour {
    CanvasGroup canvasGroup;
    public GameObject Plane;
	// Use this for initialization
	void Start () {
        
        canvasGroup = Plane.GetComponentInChildren<CanvasGroup>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //这里需要欧老师补全
    public void OnClickButton()
    {
        Time.timeScale = 1.0f;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

    }

}
