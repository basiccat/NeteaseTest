using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarTexture : RawImage {
    private Slider slider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        //获取血条组件
        if (slider = this.transform.parent.parent.GetComponent<Slider>())
        {
            //该函数会自动刷新调用
            //改变血条的长度
            uvRect = new Rect(0, 0, slider.value, 20);
        }


    }


}
