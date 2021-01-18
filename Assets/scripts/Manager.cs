using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private static Manager instance;

    public GameObject DiShu;
    public int DiShuCount;
    public float LeftTime;
    public float SpawnTimer;
    public float SpawnTime;
    public float SpawnTimeMax;
    public float SpawnTimeMin;
    public float DisappearTime;
    public GUIStyle style;
    public Text text;

    public int Score;

    private List<GameObject> holes;

    public static Manager Instance { get => instance; set => instance = value; }

    private void Start()
    {
        Instance = this;

        //加载地鼠预制体
        DiShu = Resources.Load<GameObject>("DiShu");
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
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            holes.Add(temp.transform.GetChild(i).gameObject);
        }
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
            temp.GetComponent<DiShu>().lifeTime = SpawnTime + 2;

            DiShuCount++;
            SpawnTimer = 0;
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastInfo;

            if (Physics.Raycast(ray, out raycastInfo, 20, 1 << LayerMask.NameToLayer("DiShu")))
            {
                raycastInfo.collider.gameObject.GetComponent<DiShu>().onAttack();
            }
        }
    }

    public void UpdateScore()
    {
        text.text = "Score:" + Score.ToString();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(50,50,200,600) , "Score:" + Score.ToString(), style);
    }

    public void HitDiShu()
    {
        Score += 10;
    }

    public void OnDiShuDestroy()
    {
        DiShuCount--;
    }
}
