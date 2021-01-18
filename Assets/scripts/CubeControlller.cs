using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControlller : MonoBehaviour
{
    float moveTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自己下落
        moveTimer += Time.deltaTime;
        if (moveTimer > 0.8f)
        {
            Move();
            moveTimer = 0;
        }

        //按键检测
        MonitorKey();
    }

    /// <summary>
    /// 自己移动
    /// </summary>
    void Move()
    {
        transform.position += new Vector3(0, -1, 0);
        if(!IsIn())
        {
            transform.position += new Vector3(0, 1, 0);
        }
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    void MonitorKey()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //左移
            transform.position += new Vector3(-1, 0, 0);

            //是否出界
            if (!IsIn())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //右移
            transform.position += new Vector3(1, 0, 0);
            //是否出界
            if (!IsIn())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //旋转
        }
    }

    /// <summary>
    /// 出界判断
    /// </summary>
    /// <returns>true表示没出界 false表示出界</returns>
    bool IsIn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);
            if (temp.position.x < 1 || temp.position.x > 10 || temp.position.y < 1)
            {
                return false;
            }
            
        }

        return true;
    }
}
