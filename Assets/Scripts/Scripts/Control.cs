using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    [Header("Result")]
    public float lasttime;
    public int successnumber;
    public int maxNumber;
    public int RUN0_DOOR1_NUM = 0;
    public float RUN0_DOOR1_TIME = 0.0f;
    public int RUN0_DOOR2_NUM = 0;
    public float RUN0_DOOR2_TIME = 0.0f;
    public int RUN2_DOOR1_NUM = 0;
    public float RUN2_DOOR1_TIME = 0.0f;
    [Header("Attribute")]
    public int totalnumber;
    public float totaltime;
    public float Time360Cnt = 0;
    public float time;
    public bool flg = false;
    bool set = false;
    public int Ba1, Ba2, Ba3, Ba4, Ba5 = 0;
    public int Bs1, Bs2,  Bs3,  Bs4, Bs5 = 0;
    public int RUN1_DOOR1_NUM = 0;
    public float RUN1_DOOR1_TIME = 0.0f;
    public bool userDynamic;

    public int jump = 0;
    public int dec = 0;
    public float rate = 0.2f;
    public float chaosRate = 0f;
    public bool chaosed = false;
    [Header("Assest")]
    public GameObject walk0 = null;
    public GameObject walk1 = null;
    public GameObject walk2 = null;
    public GameObject walk3 = null;
    public GameObject walk4 = null;
    public WalkPath walkPath0 = null;
    public WalkPath1 walkPath1 = null;
    public WalkPath2 walkPath2 = null;
    public WalkPath3 walkPath3 = null;
    public WalkPath4 walkPath4 = null;
    public GameObject fireBreak3;
    public GameObject fireBreak4;



    //[HideInInspector]
    public int cntTest = 0;
    public int cnt = 0;
    public int cnt0 = 0;
    public int cnt1 = 0;
    public int cnt2 = 0;
    public int cnt3 = 0;
    public int cnt4 = 0;


    void Start ()
    {
        totaltime = 0;
        totalnumber = 0;
        lasttime = 0;
        Ba1 = 100; Ba2 = 100; Ba3 = 100; Ba4 = 100; Ba5 = 100;
        Bs1 = 50;   Bs2 = 50;   Bs3 = 50;  Bs4 = 50;   Bs5 = 50;
        time = 0f;
        //Time.timeScale = 5;
        CountPeople();

    }
	void Update () {
        var walk0s = walk0.GetComponent<WalkPath>();
        var walk3s = walk3.GetComponent<WalkPath3>();
        var walk4s = walk4.GetComponent<WalkPath4>();
        Time360Cnt += Time.deltaTime;

        //火焰生成0，1，路径断开
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("您按下了E键");
            walk3s.isis = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("您按下了R键");
            walk0s.isis = true;
        }
        


        // run4&5之间的调度
        //if ((float)(cnt0 + cnt3 + cnt4 - cntTest) / (float)(cnt2 + cntTest) <= 0.75f && walk4s.isis == false && userDynamic)
        //{
        //    walk4s.isis = true;
        //    Debug.Log("楼下调度产生");
        //}

        //统计数字的更新
        if (totalnumber==maxNumber/*&&set==false*/)
        {
            //lasttime = Time.time-time;
            lasttime = Time.time - time;
            //set = true;
            if (flg==false)
            {
                flg = true;
                successnumber = totalnumber;
            }
            chaosRate = (float)jump / (float)dec;
            Time.timeScale = 0;
        }
        if(Time360Cnt-time>=360.0f&&flg==false)
        {
            flg = true;
            successnumber = totalnumber;
        }
	}


    public void run0_door1()
    {
        if(totalnumber<=maxNumber)
        {
            RUN0_DOOR1_NUM++;
            RUN0_DOOR1_TIME = Time.time - time;
        }
        
    }
    public void run0_door2()
    {
        if (totalnumber <= maxNumber)
        {
            RUN0_DOOR2_NUM++;
            RUN0_DOOR2_TIME = Time.time - time;
        }
    }
    public void run1_door1()
    {
        if (totalnumber <= maxNumber)
        {
            RUN1_DOOR1_NUM++;
            RUN1_DOOR1_TIME = Time.time - time;
        }
    }
    public void run2_door1()
    {
        if (totalnumber <= maxNumber)
        {
            RUN2_DOOR1_NUM++;
            RUN2_DOOR1_TIME = Time.time - time;
        }
    }
    public void CountPeople()
    {
        
        GameObject[] obj;
        obj = FindObjectsOfType(typeof(GameObject)) as GameObject[]; //关键代码，获取所有gameobject元素给数组obj
        foreach (GameObject child in obj)    //遍历所有gameobject
        {
            //Debug.Log(child.gameObject.name);  //可以在unity控制台测试一下是否成功获取所有元素
            if (child.tag == "run0" || child.tag == "run1" || child.tag == "run2" || child.tag == "run3" || child.tag == "run4")
            {
                cnt++;
            }
            if (child.tag == "run0")
            {
                cnt0++;
            }
            if (child.tag == "run1")
            {
                cnt1++;
            }
            if (child.tag == "run2")
            {
                cnt2++;
            }
            if (child.tag == "run3")
            {
                cnt3++;
            }
            if (child.tag == "run4")
            {
                cnt4++;
            }
        }
        Debug.Log("人数总量为：" + cnt + " run0:" + cnt0 + " run1:" + cnt1 + " run2:" + cnt2 + " run3:" + cnt3 + " run4:" + cnt4);
    }
}
