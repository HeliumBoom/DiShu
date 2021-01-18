using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public delegate void Action(params object[] objects);
    private static Manager instance;

    public GameObject DiShu;//地鼠预制体
    public int DiShuCount;//地鼠数量
    public float LeftTime;//剩余时间
    public float SpawnTimer;//地鼠产生计时器
    public float SpawnTime;//产生时间
    public float SpawnTimeMax;//最大产生时间
    public float SpawnTimeMin;//最小
    public float DisappearTime;//地鼠消失时间
    public GUIStyle style;
    public Text text;//UI的text

    public int Score;//分数

    public event Action test;

    private List<GameObject> holes;

    public static Manager Instance { get => instance; set => instance = value; }

    private void Start()
    {
        Instance = this;

        //加载地鼠预制体
        DiShu = Resources.Load<GameObject>("Prefabs/DiShu");
        LeftTime = 60f;//初始时间
        SpawnTimeMax = 5f;
        SpawnTimeMin = 0.5f;
        SpawnTime = SpawnTimeMax;
        SpawnTimer = 0f;
        DiShuCount = 0;//地鼠数量

        GameObject te = GameObject.Find("/Canvas/Text");
        text = te.GetComponent<Text>();
        UpdateScore();

        GameObject temp = GameObject.Find("/Enviroment/hole");
        holes = new List<GameObject>();
        //取地所有子物体
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            holes.Add(temp.transform.GetChild(i).gameObject);
        }

        //Debug.Log(gameObject.name);
        //Debug.Log(LayerMask.NameToLayer("DiShu"));
        test += Debug1;
        test += Debug2;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer += Time.deltaTime;
        LeftTime -= Time.deltaTime;
        SpawnTime = Mathf.Clamp(LeftTime / 10 - 2f, SpawnTimeMin, SpawnTimeMax);

        if (DiShuCount < 9 && SpawnTimer > SpawnTime)
        {
            //产生地鼠
            int id = Random.Range(0, 9);

            while (holes[id].transform.childCount == 1)
            {
                id = Random.Range(0, 9);
            }

            GameObject temp = Instantiate<GameObject>(DiShu, holes[id].transform.position, Quaternion.identity, holes[id].transform);
            temp.GetComponent<DiShu>().lifeTime = SpawnTime + 2f;

            DiShuCount++;
            SpawnTimer = 0;
        }

        //射线检测
        if (Input.GetKeyDown(KeyCode.Mouse0))//按键检测
        {
            test.Invoke(1, 45);

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - Camera.main.transform.position);
            RaycastHit raycastInfo;

            //检测有没有点到地鼠
            if (Physics.Raycast(ray, out raycastInfo, 20, 1 << LayerMask.NameToLayer("DiShu")))
            {
                raycastInfo.collider.gameObject.GetComponent<DiShu>().onAttack();
            }
        }

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Debug.Log("down");
        //}
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    Debug.Log("on");
        //}
        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    Debug.Log("up");
        //}
    }

    public void UpdateScore()
    {
        text.text = "Score:" + Score.ToString();
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(50,50,200,600) , "Score:" + Score.ToString(), style);
    }

    public void HitDiShu()
    {
        Score += 10;
        UpdateScore();
    }

    public void OnDiShuDestroy()
    {
        DiShuCount--;
    }

    public void Debug1(params object[] objects)
    {
        Debug.Log(objects[0]);
    }
    public void Debug2(params object[] objects)
    {
        Debug.Log(objects[1]);
    }
}
