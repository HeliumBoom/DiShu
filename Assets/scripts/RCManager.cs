using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RCManager : MonoBehaviour
{
    private static RCManager instance;
    public static RCManager Instance { get => instance; set => instance = value; }

    public float lifeTime;
    public float lifeTimer;

    public List<GameObject> cubePrefabs;

    public GameObject[,] Cubes = new GameObject[11, 22];
    public GameObject cubesParent;

    public bool IsGameOver = false;

    private void Awake()
    {
        instance = this;
        cubePrefabs = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/L1"));//0
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/L2"));//1
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/line"));//2
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/Square"));//3
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/T"));//4
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/Z1"));//5
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/Z2"));//6

        CreatCube();
    }

    public void CreatCube()
    {
        if (IsGameOver)
        {
            return;
        }

        int index = Random.Range(0, cubePrefabs.Count);
        GameObject cube;


        if (index == 3)
        {
            cube = Instantiate<GameObject>(cubePrefabs[index], new Vector3(5.5f, 21.5f, 0), Quaternion.identity);
        }
        else
        {
            cube = Instantiate<GameObject>(cubePrefabs[index], new Vector3(5, 21f, 0), Quaternion.identity);
        }

        cube.name = cubePrefabs[index].name;

        GameObject temp = GameObject.Find("/Canvas/Left");
        Button button = temp.GetComponent<Button>();
        button.onClick.AddListener(cube.GetComponent<CubeControlller>().MoveLeft);

        ///加载cube事件
        UnityEvent eventTemp = cube.GetComponent<CubeControlller>().OnThisDone;
        eventTemp.AddListener(() => { AddToCubes(cube); });
        eventTemp.AddListener(CreatCube);
    }

    public void AddToCubes(GameObject parent)
    {
        int count = parent.transform.childCount;//childcount会变所以事先要存
        List<int> line = new List<int>();//用来储存哪几行变化了

        for (int i = 0; i < count; i++)
        {
            Transform child = parent.transform.GetChild(parent.transform.childCount - 1);
            child.parent = cubesParent.transform;

            int x = Mathf.RoundToInt(child.transform.position.x);//获得cube应该在的xy坐标
            int y = Mathf.RoundToInt(child.transform.position.y);

            if (!line.Contains(y))//不重复的加入到数组中
            {
                line.Add(y);
            }
            //Debug.Log(x + " " + y + " " + child.name);

            Cubes[x, y] = child.gameObject;

            if (y > 18)
            {
                IsGameOver = true;
            }
        }

        //把改变的行传入来销毁
        DetectionToDestroyLine(line.ToArray());
    }

    public void DetectionToDestroyLine(params int[] line)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < line.Length; i++)
        {
            bool canDestroy = true;

            //判断这一行能不能被消除
            for (int j = 1; j < Cubes.GetLength(0); j++)
            {
                if (Cubes[j, line[i]] == null)
                {
                    canDestroy = false;
                    break;
                }
            }

            //如果能
            if (canDestroy)
            {
                //把被消除的行数（index）记录一下
                list.Add(line[i]);

                //消除
                for (int j = 1; j < Cubes.GetLength(0); j++)
                {
                    GameObject temp = Cubes[j, line[i]];
                    Cubes[j, line[i]] = null;
                    Destroy(temp);
                }
            }
        }

        //排序
        list.Sort();

        for (int i = list.Count - 1; i >= 0; i--)
        {
            //上面的移下来
            for (int j = list[i] + 1; j < Cubes.GetLength(1); j++)
            {
                for (int k = 1; k < Cubes.GetLength(0); k++)
                {
                    if (Cubes[k, j] != null)//判空
                    {
                        Debug.Log(Cubes[k, j].name);
                        Cubes[k, j].transform.position += new Vector3(0, -1, 0);
                        Cubes[k, j - 1] = Cubes[k, j];
                        Cubes[k, j] = null;
                    }
                }
            }
        }
        

    }

    public void Test1()
    {
        Debug.Log("Buttom");
    }

}
