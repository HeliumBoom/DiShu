using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RCManager : MonoBehaviour
{
    private static RCManager instance;
    public static RCManager Instance { get => instance; set => instance = value; }

    public float lifeTime;
    public float lifeTimer;

    public List<GameObject> cubePrefabs;

    public GameObject[,] Cubes = new GameObject[11,25];
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
        UnityEvent eventTemp = cube.GetComponent<CubeControlller>().OnThisDone;
        eventTemp.AddListener(() => { AddToCubes(cube); });
        eventTemp.AddListener(CreatCube);
    }

    public void AddToCubes(GameObject parent)
    {
        int count = parent.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = parent.transform.GetChild(parent.transform.childCount - 1);
            child.parent = cubesParent.transform;
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            //Debug.Log(x + " " + y + " " + child.name);

            Cubes[x, y] = child.gameObject;

            if (y > 18)
            {
                IsGameOver =true;
            }
        }
    }
}
