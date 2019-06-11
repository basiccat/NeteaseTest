using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjManager : MonoBehaviour
{

    Ray mouseRay;//从鼠标发射的射线
    RaycastHit planePoint;//射线与地面的交点
    LayerMask mask = 1 << 8;//标记地面所处的图层

    public GameObject cam;//camera
    private static SelectObjManager _instance;

    public float height;
    
    public static SelectObjManager Instance
    {
        get { return _instance; }
    }

    //物体z轴距摄像机的长度
    public float _zDistance = 50f;
    //对象的缩放系数
    public float _scaleFactor = 1.2f;
    //地面层级
    public LayerMask _groundLayerMask;
    int touchID;
    bool isDragging = false;
    bool isTouchInput = false;
    //是否是有效的放置（如果放置在地面上返回True,否则为False）
    bool isPlaceSuccess = false;
    //当前要被放置的对象
    public GameObject currentPlaceObj = null;
    //坐标在Y轴上的偏移量
    public float _YOffset = 0.5F;

    void Awake()
    {
        _instance = this;
    }
    void Update()
    {
        if (currentPlaceObj == null) return;

        if (Input.GetMouseButton(0))
        {

            MoveCurrentPlaceObj();
            //点击技能时，放大正交相机视口（见CameraScroll类
            cam.GetComponent<CameraScroll>().scrollState(true);
        }
        else if (isDragging)
        {
            CheckIfPlaceSuccess();
            ////视口放大后的复原
            //if (cam.GetComponent<CameraScroll>().size > cam.GetComponent<CameraScroll>().minSize)
            //{
            //    cam.GetComponent<CameraScroll>().ResumeView();
            //}
            cam.GetComponent<CameraScroll>().scrollState(false);
        }
       
    }

    /// <summary>
    ///让当前对象跟随鼠标移动
    /// </summary>
    void MoveCurrentPlaceObj()
    {
        isDragging = true;
        Vector3 point;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _groundLayerMask))
        {
            point = hitInfo.point;
            isPlaceSuccess = true;
        }
        else
        {
            point = ray.GetPoint(_zDistance);
            isPlaceSuccess = false;
        }

        Vector3 target = new Vector3(planePoint.point.x, this.transform.position.y -height , planePoint.point.z);
        currentPlaceObj.transform.position = point + target;
        currentPlaceObj.transform.localEulerAngles = new Vector3(0, 60, 0);
    }
    /// <summary>
    ///在指定位置化一个对象
    /// </summary>
    void CreatePlaceObj()
    {
        GameObject obj = Instantiate(currentPlaceObj) as GameObject;
        
        obj.transform.position = currentPlaceObj.transform.position;
        obj.transform.localEulerAngles = currentPlaceObj.transform.localEulerAngles;
        obj.transform.localScale *= _scaleFactor;


        Debug.Log("set");

        obj.GetComponent<CapsuleCollider>().enabled = true;
        //Debug.Log("技能激活");
        obj.SendMessage("setActive");
        Debug.Log("1");
        
    }
    /// <summary>
    ///检测是否放置成功
    /// </summary>
    void CheckIfPlaceSuccess()
    {
        if (isPlaceSuccess)
        {
            CreatePlaceObj();
        }
        isDragging = false;
        Destroy(currentPlaceObj.gameObject);
        currentPlaceObj = null;
    }


    /// 将要创建的对象传递给当前对象管理器
    public void AttachNewObject(GameObject newObject,float h)
    {
        if (currentPlaceObj)
        {
            currentPlaceObj.SetActive(false);
        }
        currentPlaceObj = newObject;
        height = h;
    }
}
