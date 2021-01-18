using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCManager : MonoBehaviour
{
    public float lifeTime;
    public float lifeTimer;

    public List<GameObject> cubePrefabs;

    // Start is called before the first frame update
    void Start()
    {
        cubePrefabs = new List<GameObject>();

        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/L1"));//0
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/L2"));
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/line"));
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/Square"));
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/T"));
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/Z1"));
        cubePrefabs.Add(Resources.Load<GameObject>("Prefabs/Z2"));

        CreatCube();
    }

    public void CreatCube()
    {
        int index = Random.Range(0, cubePrefabs.Count);

        if (index == 2)
        {
            Instantiate<GameObject>(cubePrefabs[index], new Vector3(5.5f, 21, 0), Quaternion.identity);
        }
        else if (index == 3)
        {
            Instantiate<GameObject>(cubePrefabs[index], new Vector3(5.5f, 21.5f, 0), Quaternion.identity);
        }
        else if (index == 4 || index == 5 || index == 6)
        {
            Instantiate<GameObject>(cubePrefabs[index], new Vector3(5, 21.5f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate<GameObject>(cubePrefabs[index], new Vector3(5, 21f, 0), Quaternion.identity);
        }
    }


}
