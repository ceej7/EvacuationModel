using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
[System.Serializable]
public class WalkPathP : MonoBehaviour {
    #region Varibles
    Timer timekeeper;
    [SerializeField]
    public GameObject fireStart;
    float TempTime = 0f;
    public float TTime = 0f;
    ServerSocket ssock;
    public string str = null;
    public bool StartRun = true;
    public GameObject smoke;
    private bool IfSmoke = false;
    public int failnum=0;
    public Thread gasThread,spreadThread;
    public bool Split=false;
    private GameObject FireObjectR1, FireObjectR2, FireObjectL1, FireObjectL2;
    public bool ChangeFire = false;
    const int grid = 8;
    public int MaxF = 0;
    public int [,] FireArea=new int [grid,grid];
    public GameObject walk0 = null;
    public GameObject walk3 = null;
    ArrayList list = new ArrayList();

    //控制时间
    public float startTime = 45;
    bool startfire = false;
    float ticks = 0;
    #endregion Varibles
    void UpdateFire(string str)
    {
        for (int i = 0; i < grid; i++)
            for (int j = 0; j < grid; j++)
                FireArea[i,j] = 0;
        int k = 1;
        while (str[k]!='F')
        {
            if (str[k]!='L')
            {                        
                int p, q;
                p = (int)(str[k] - '0');
                q=  (int)(str[k+1] - '0');
                if (str[k + 2] == 'a')
                    FireArea[p, q] = 1;
                else if (str[k + 2] == 'b')
                    FireArea[p, q] = 2;
                else if (str[k + 2] == 'c')
                    FireArea[p, q] = 3;
                k = k + 3;
            }
            else
                if (str[k]=='L')
               {
                MaxF = ((int)(str[k+1] - '0'))*10+ (int)(str[k + 2] - '0') ;
                k = k + 3;
                }
        }
    }
    void Start()
    {
        ssock = new ServerSocket();
        ssock.Init();
        FireObjectL1 = GameObject.Find("Fire/FireSpread");
        FireObjectR1 = GameObject.Find("Fire/FireSpread1");
        for (int i = 0; i < grid; i++)
            for (int j = 0; j < grid; j++)
                FireArea[i, j] = 0;
         walk0 = GameObject.Find("Run0");
          walk3 = GameObject.Find("Run3");
    }
    void Update()
    {
        if(startfire==false)
        {
            ticks += Time.deltaTime;
            if(ticks>=startTime)
            {
                startfire = true;
            }
        }
        else
        {
            //if (ssock.ReturnStr() != null)
            //{
            var walk0s = walk0.GetComponent<WalkPath>();
            var walk3s = walk3.GetComponent<WalkPath3>();
            //Debug.Log(str);
            //str = ssock.ReturnStr();
            str = "S42b52c53b62c63c72b73c74bL08FLeftRight";
            UpdateFire(str);
            //if (StartRun == false)
            //{
            if (str.Contains("Left"))
            {
                StartRun = true;
                FireObjectL1.gameObject.SetActive(true);
                //FireObjectR1.gameObject.SetActive(false);
                walk3s.isis = true;

            }
            if (str.Contains("Right"))
            {
                //Split = false;
                StartRun = true;
                FireObjectR1.gameObject.SetActive(true);
                //FireObjectL1.gameObject.SetActive(false);
                walk0s.isis = true;
            }
            // }
            //}
        }

    }
}

