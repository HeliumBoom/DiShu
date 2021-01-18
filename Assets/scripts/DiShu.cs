using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiShu : MonoBehaviour
{


    public float lifeTime;
    public float lifeTimer;
    public UnityEvent OnDestroy;

    // Start is called before the first frame update
    void Start()
    {
        OnDestroy.AddListener(Manager.Instance.OnDiShuDestroy);
        //lifeTime = 0.5f + Manager.Instance.LeftTime / 20;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer > lifeTime)
        {
            DisAppear();
        }
    }

    public void onAttack()
    {
        OnDestroy.Invoke();

        //加分
        Manager.Instance.HitDiShu();
        //删除实体对象
        Destroy(gameObject);
    }
    public void DisAppear()
    {
        OnDestroy.Invoke();
        //删除实体对象
        Destroy(gameObject);
    }
}
