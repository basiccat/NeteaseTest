using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class sound : MonoBehaviour {
    public Slider s;
    public AudioSource a;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        a.volume = s.value;	}
}
