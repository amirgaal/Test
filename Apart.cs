using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Apart : MonoBehaviour
{
    //之前的位置
    Vector3 lastvector;
    //之后的位置
    Vector3 nowvector;

    //选择的物体是本身
    bool ischose;
    //脚本激活
    bool action;

    void OnEnable()
    {
        action = true;
    }
    void OnDisable()
    {
        action = false;
    }

    void OnMouseEnter()
    {
        ischose = true;
    }
    void OnMouseExit()
    {
        ischose = false;
    }

    /// <summary>
    /// 当鼠标点击 进行移动
    /// </summary>
    /// <returns></returns>
    IEnumerator OnMouseDown()
    {
        //世界转屏幕坐标
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        //鼠标点击的偏移值
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

        while (Input.GetMouseButton(0) && action)
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }
    }
    void Update()
    {
        //旋转
        if (Input.GetMouseButtonDown(1) && ischose)
        {
            lastvector = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            nowvector = Input.mousePosition;
            if (lastvector != nowvector)
            {
                if (Mathf.Abs(lastvector.y - nowvector.y) < 100f)
                    transform.transform.Rotate(0, Input.GetAxis("Mouse X") * 3f, 0);
                if (Mathf.Abs(lastvector.x - nowvector.x) < 30f)
                    transform.Rotate(Input.GetAxis("Mouse Y") * 3f, 0, 0);
            }
        }

        //缩放
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && ischose)
        {
            if (transform.localScale.x < 5f)
                transform.localScale += new Vector3(1, 1, 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && ischose)
        {
            if (transform.localScale.x > 1f)
                transform.localScale += new Vector3(-1, -1, -1);
        }
    }
}