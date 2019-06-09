using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class pause : MonoBehaviour {
    public Plane menu;
    CanvasGroup canvasgroup;
	// Use this for initialization
	void Start () {
        canvasgroup = GetComponentInChildren<CanvasGroup>();
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0;
            canvasgroup.alpha = 1;
            canvasgroup.interactable = true;
            canvasgroup.blocksRaycasts = true;
        }

	}
}
