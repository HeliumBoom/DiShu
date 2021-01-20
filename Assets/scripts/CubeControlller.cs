using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeControlller : MonoBehaviour
{
    public float moveTimer;
    public UnityEvent OnThisDone;
    float downTimer = 0;

    // Update is called once per frame
    void Update()
    {
        //按键检测
        MonitorKey();

        //自己下落
        moveTimer += Time.deltaTime;
        if (moveTimer > 0.8f)
        {
            Move();
            moveTimer = 0;
        }
    }



    /// <summary>
    /// 自己移动
    /// </summary>
    void Move()
    {
        transform.position += new Vector3(0, -1, 0);

        bool isCrrsh;
        bool isIn = IsIn(out isCrrsh);

        if (isCrrsh || !isIn)
        {
            //向下运动发生出界或者碰撞
            transform.position += new Vector3(0, 1, 0);

            OnThisDone.Invoke();

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    void MonitorKey()
    {
        //左移
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }

        //右移
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

        //旋转
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Rotate(new Vector3(0, 0, 90));

            int deviation;
            bool isCrash;
            bool isIn = IsIn(out deviation, out isCrash);

            if (isCrash)
            {
                transform.Rotate(new Vector3(0, 0, -90));
            }
            else
            {
                if (!isIn)
                {
                    //先偏移
                    transform.position -= new Vector3(deviation, 0, 0);

                    bool afterCrash = false;
                    //偏移之后可能也有挡住的
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        Transform temp = transform.GetChild(i);

                        int x = Mathf.RoundToInt(temp.position.x);
                        int y = Mathf.RoundToInt(temp.position.y);

                        if (y < 19 && x > 0 && x < RCManager.Instance.Cubes.GetLength(0) && RCManager.Instance.Cubes[x, y] != null)
                        {
                            afterCrash = true;
                        }
                    }

                    //偏移之后发生碰撞
                    if (afterCrash)
                    {
                        //移回去
                        transform.position += new Vector3(deviation, 0, 0);
                        //转回去
                        transform.Rotate(new Vector3(0, 0, -90));
                    }
                }
            }
        }

        //快速向下
        if (Input.GetKey(KeyCode.S))
        {
            downTimer += Time.deltaTime;

            if (downTimer > 0.1f)
            {
                Move();
                downTimer = 0;
            }
        }
        else
        {
            downTimer = 0;
        }
    }

    public void MoveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);
        bool isCrash;

        //是否出界
        if (!IsIn(out isCrash) || isCrash)
        {
            transform.position += new Vector3(1, 0, 0);
        }
    }
    public void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        bool isCrash;

        //是否出界
        if (!IsIn(out isCrash) || isCrash)
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    /// <summary>
    /// 左右下出界判断
    /// </summary>
    /// <param name="deviation"></param>
    /// <param name="isCrash"></param>
    /// <returns></returns>
    bool IsIn(out int deviation, out bool isCrash)
    {
        bool isIn = true;
        deviation = 0;
        isCrash = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);

            int x = Mathf.RoundToInt(temp.position.x);
            int y = Mathf.RoundToInt(temp.position.y);

            temp.position = new Vector3(x, y, 0);

            if (y < 19 && x > 0 && x < RCManager.Instance.Cubes.GetLength(0) && RCManager.Instance.Cubes[x, y] != null)
            {
                isCrash = true;
            }

            //Debug.Log("Pos:" + pos.x + "  " + pos.y + "  " + pos.z);
            //Debug.Log("temp:" + temp.position.x + "  " + temp.position.y + "  " + temp.position.z);

            if (x < 1)
            {
                deviation = (Mathf.Abs(x - 1) > deviation) ? (int)x - 1 : deviation;

                isIn = false;
            }
            else if(x > 10)
            {
                deviation = (Mathf.Abs(x - 1) > deviation) ? (int)x - 10 : deviation;

                isIn = false;
            }
            else if (y < 1)//算碰撞
            {
                isCrash = true;
                isIn = false;
            }
        }

        return isIn;
    }

    /// <summary>
    /// 左右下出界判断
    /// </summary>
    /// <returns>true表示没出界 false表示出界</returns>
    bool IsIn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);

            Vector3 pos = new Vector3(Mathf.RoundToInt(temp.position.x), Mathf.RoundToInt(temp.position.y), 0);
            temp.position = pos;
            //Debug.Log("Pos:" + pos.x + "  " + pos.y + "  " + pos.z);
            //Debug.Log("temp:" + temp.position.x + "  " + temp.position.y + "  " + temp.position.z);

            if (pos.x < 1 || pos.x > 10 || pos.y < 1)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isCrash"></param>
    /// <returns></returns>
    bool IsIn(out bool isCrash)
    {
        bool isIn = true;
        isCrash = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);

            int x = Mathf.RoundToInt(temp.position.x);
            int y = Mathf.RoundToInt(temp.position.y);

            temp.position = new Vector3(x, y, 0);

            if (y < 19 && x > 0 && x < RCManager.Instance.Cubes.GetLength(0) && RCManager.Instance.Cubes[x, y] != null)
            {
                isCrash = true;
            }

            //Debug.Log("Pos:" + pos.x + "  " + pos.y + "  " + pos.z);
            //Debug.Log("temp:" + temp.position.x + "  " + temp.position.y + "  " + temp.position.z);

            if (x < 1)
            {
                isIn = false;
            }
            else if (x > 10)
            {
                isIn = false;
            }
            else if (y < 1)
            {
                isCrash = true;
                isIn = false;
            }
        }

        return isIn;
    }
}
