
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region 声明变量
    [Tooltip("相机和物体之间的距离")]
    public float distance = 40;
    public float minDistance = 3;
    public float maxDistance = 44;

    [Tooltip("相机y轴旋转角度限制")]
    public int yMinLimit = 0;
    public int yMaxLimit = 80;

    [Tooltip("中间变量")]
    Vector3 pos;
    Vector3 ro;
    private float x;
    private float y;

    //[Tooltip("限制鼠标控制范围")]
    //public pointenter point;

    [Tooltip("鼠标旋转速度 ")]
    public float xSpeed = 250;
    public float ySpeed = 120.0f;
    [Tooltip("鼠标滑轮速度 ")]
    float scrollSpeed = 3f;
    [Tooltip("鼠标移动速度")]
    float moveSpeed = 10f;

    [HideInInspector]
    public Transform target;
    //[HideInInspector]
    //public bool isgo;

    [Tooltip("是否处于使用状态")]
    bool isstart = false;
    int a = 0;
    [Tooltip("是否处于移动状态")]
    public bool ismove = false;
    [Tooltip("移动标准空物体")]
    public Transform m1;
    [Tooltip("目标模型")]
    public Transform t1;
    private float MinX = -8f, MaxX = 8f, MinY = -8.5f, MaxY = 8f;
    #endregion

    /// <summary>
    /// 重置
    /// </summary>
    public void backst()
    {
        transform.position = pos;
        transform.eulerAngles = ro;
        Vector2 angel = transform.eulerAngles;
        x = angel.y;
        y = angel.x;
        Getstop();
        m1.localPosition = Vector3.zero;
        m1.localEulerAngles = Vector3.zero;
        isgo = false;
        ismove = false;
    }

    #region 是否在使用状态
    public bool ready()
    {
        return isstart;
    }
    private void Getstart()
    {
        a++;
        isstart = true;
    }
    private void Getstop()
    {
        a = 0;
        isstart = false;
    }
    #endregion

    #region 系统函数
    private void OnEnable()
    {
        Vector2 angel = transform.eulerAngles;
        x = angel.y;
        y = angel.x;
    }
    void Start()
    {
        //isgo = false;
        Vector2 angel = transform.eulerAngles;
        x = angel.y;
        y = angel.x;
        pos = transform.position;
        ro = transform.eulerAngles;
        target = t1;
    }
    void Update()
    {
        //if (isgo)
        //{
        //    CameraMove(0, 0);
        //   isgo = false;
        //}
        distance = Vector3.Distance(transform.position, target.position);
        //当鼠标在有效位置并且相机不是移动状态
        if (!ismove)//&&point.isok 
        {
            //旋转
            if (Input.GetMouseButton(1))
            {
                if (a == 0)
                    Getstart();
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            m1.eulerAngles = new Vector3(0, -(90 - transform.eulerAngles.y), 0);
            float _mouseX = Input.GetAxis("Mouse X");
            float _mouseY = Input.GetAxis("Mouse Y");
            //移动
            if (Input.GetMouseButton(0))
                CameraMove(_mouseX, _mouseY);
            //缩放
            CameraNear_Far(Input.GetAxis("Mouse ScrollWheel"), scrollSpeed);
        }
        //点击事件
        //DrawRay();
    }
    #endregion 

    #region 操作方法
    private void LateUpdate()
    {
        //旋转角度值赋予
        if (isstart || ismove)
        {
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
            transform.rotation = rotation;
            transform.position = position;
        }
    }
    /// <summary>
    /// 中键控制拖动
    /// </summary>
    /// <param name="_mouseX"></param>
    /// <param name="_mouseY"></param>
    public void CameraMove(float _mouseX, float _mouseY)
    {
        if (a == 0)
            Getstart();
        m1.localPosition = new Vector3(Mathf.Clamp(m1.localPosition.x, MinX, MaxX), m1.localPosition.y, Mathf.Clamp(m1.localPosition.z, MinY, MaxY));
        target = m1;
        target.transform.Translate(Vector3.forward * moveSpeed * _mouseX * Time.deltaTime, Space.Self);
        target.transform.Translate(Vector3.right * moveSpeed * -_mouseY * Time.deltaTime, Space.Self);
    }
    /// <summary>
    /// 滚轮控制缩放
    /// </summary>
    /// <param name="mouseScrollWheel"></param>
    /// <param name="speed"></param>
    private void CameraNear_Far(float mouseScrollWheel, float speed)
    {
        if (a == 0 && Input.GetAxis("Mouse ScrollWheel") != 0)
            Getstart();
        if (mouseScrollWheel > 0)
        {
            distance -= speed;
            if (distance < minDistance)
                distance = minDistance;
        }
        else if (mouseScrollWheel < 0)
        {
            distance += speed;
            if (distance > maxDistance)
                distance = maxDistance;
        }
    }
    /// <summary>
    /// 限制数值大小
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
    /// <summary>
    /// /从当前物体发射线到点击位置，判断是否弹窗
    /// </summary>
    //void DrawRay()
    //{
    //    if (Input.GetMouseButtonDown(0))//按鼠标
    //    {
    //        //生成一条射线,从相机出发，到你鼠标点击方向
    //        Ray ray = transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    //        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);

    //        RaycastHit hit;
    //        //射线检测 mask layer
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            //当这个物体是弹窗
    //            if (hit.transform.GetComponent<tanchuang>() != null)
    //            {
    //                hit.transform.GetComponent<tanchuang>().OnDownBg(int.Parse(hit.transform.parent.name));
    //                //print(hit.transform.GetComponent<tanchuang>().OnDownBg());
    //            }
    //            //Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
    //        }
    //    }
    //}
    #endregion
}


