using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[System.Serializable]
public class WalkPath3 : MonoBehaviour {

    public List<Vector3> pathPoint = new List<Vector3>();
    public List<GameObject> pathPointTransform = new List<GameObject>();

    public GameObject[] peoplePrefabs;
    public GameObject par;
    public float moveSpeed = 1;
    public float density;

    //3.26
    public List<MovePathGeneral>[] people_toward;
    static Object locker = new Object();
    public TwoDimensionPoint[] parallerPoint;

    //维护地下run3中路径断掉的情况下人物重新寻路
    public bool isRunBroken = false;
    public int dest = 34;

    public void breakRun3()
    {
        isRunBroken = true;
        dest = 15;
        connects[13, 15] = 1;
        connects[93, 95] = 1;
        connects[95, 32] = 0;
        connects[93, 33] = 0;
        CalPath();
    }

    public float cntDown = 0;
    public bool isis = false;

    [HideInInspector]
    public int[] pointLength = new int[10];
    public int[,] connects = new int[pnum + 1, pnum + 1];
    public enum EnumDir { Forward, Backward, HugLeft, HugRight, WeaveLeft, WeaveRight };
    public enum EnumMove { Walk, Run };
    public Vector3[,] points;
    public bool[] _forward;
    public List<Vector3> pointB = new List<Vector3>();
    static public int pnum = 95, Bcount = 11;
    public int z = 1, label = 0, bx = 0, Bcounts = Bcount;
    public int[,] easyconnect = new int[pnum + 1, 8];
    public int[,] shortpath = new int[pnum + 1, pnum + 1];
    public int[,] path = new int[pnum + 1, pnum + 1];
    public int[,] result = new int[pnum + 1, pnum + 1];
    public Vector3[,] Bpath = new Vector3[pnum + 1, Bcount];
    public bool Split = false;
    public bool[,] Run = new bool[pnum + 1, 2];
    public EnumDir direction;
    public EnumMove _moveType;
    public int numberOfWays;
    public bool loopPath;
    public float lineSpacing;
    public bool StartRun = false;
    public string str = null;
    public Thread gasThread, spreadThread;
    float TempTime = 0f;
    public float TTime = 0f;

    public Vector3 getNextPoint(int w, int index)
    {
        return points[w, index];
    }
    public int getPointsTotal(int w)
    {
        return pointLength[w];
    }

    void Awake()
    {
        DrawCurved(false);
        CalPath();
        Bezier(0);
        EasyConnect();
        for (int i = 0; i <= pnum; i++)
        {
            Run[i, 0] = false;
            Run[i, 1] = false;
        }
        str = "";
    }

    private void Start()
    {
        initialPeopleToward();
    }
    void Update()
    {
        

        if (isis==true&&!isRunBroken)
        {
            breakRun3();
            //parallerPoint[parallerPoint.Length - 1].p1 = -1;
            //parallerPoint[parallerPoint.Length - 1].p2 = -1;
            
        }
        else
        {
            //parallerPoint[parallerPoint.Length - 1].p1 = 33;
            //parallerPoint[parallerPoint.Length - 1].p2 = 34;
        }
        if (str == "Left" && StartRun == false)
        {
            for (int i = 1; i <= pnum; i++)
                for (int j = 1; j <= pnum; j++)
                    connects[i, j] = 0;
            InitConnect();
            for (int i = 1; i <= pnum; i++)
                for (int j = 1; j <= pnum; j++)
                    if (connects[i, j] == 1)
                        connects[j, i] = 1;
            Run[70, 0] = true; Run[70, 1] = true;
            Run[71, 0] = true; Run[71, 1] = true;
            Run[69, 0] = true; Run[69, 1] = true;
            Run[68, 0] = true; Run[68, 1] = true;
            Run[67, 0] = true; Run[67, 1] = true;
            Run[66, 0] = true; Run[66, 1] = true;

            Run[65, 0] = true; Run[65, 1] = true;
            Run[64, 0] = true; Run[64, 1] = true;
            Run[60, 0] = true; Run[60, 1] = true;

            Run[59, 0] = true; Run[59, 1] = true;
            Run[58, 0] = true; Run[58, 1] = true;
            Run[57, 0] = true; Run[57, 1] = true;

            Run[63, 0] = true; Run[63, 1] = true;
            Run[62, 0] = true; Run[62, 1] = true;
            Run[61, 0] = true; Run[61, 1] = true;

            Run[56, 0] = true; Run[56, 1] = true;
            Run[55, 0] = true; Run[55, 1] = true;
            Run[54, 0] = true; Run[54, 1] = true;

            Run[10, 0] = true; Run[10, 1] = true;
            Run[14, 0] = true; Run[14, 1] = true;
            Run[14, 0] = true; Run[14, 1] = true;
            Run[23, 0] = true; Run[23, 1] = true;
            Run[28, 0] = true; Run[28, 1] = true;
            //for (int i = 1; i <= pnum; i++)
            //{
            //    Run[i, 0] = true; Run[i, 1] = true;
            //}
            //Split = false;
            CalPath();
            EasyConnect();
            StartRun = true;
        }
        if (StartRun == true)
        {
            TTime += 0.15f;
        }

        if (StartRun == true && TTime > TempTime)
        {
            this.spreadThread = new Thread(new ThreadStart(SpreadCal));
            spreadThread.Start();
        }
    }
    public void SpreadCal()
    {
        for (int i = 1; i <= pnum; i++)
            if (Run[i, 0] == true && Run[i, 1] == true)
                for (int j = 1; j <= pnum; j++)
                    if (connects[i, j] == 1 && Run[j, 0] == false)
                        Run[j, 0] = true;
        TempTime = TempTime + 3.0f;
        for (int i = 1; i <= pnum; i++)
            if (Run[i, 0] == true && Run[i, 1] == false)
                Run[i, 1] = true;
    }
    public void OnDrawGizmos()
    {
        DrawCurved(true);
    }
    public void SpawnPeople()
    {

        if (par == null)
        {
            par = new GameObject();
            par.transform.parent = gameObject.transform;
            par.name = "people";
        }

        int s;
        s = pointLength[0] - 2;

        for (int b = 0; b < numberOfWays; b++)
        {
            for (int a = 1; a <= s; a++)
            {
                if (a!=34&&a!=33 && a !=21 &&
                    a !=41 && a !=44 && a !=50 &&
                    a !=43 && a !=49 && a !=20 &&
                    a !=48 && a !=6 && a !=47 &&
                    a !=19 && a !=46 && a !=45 &&
                    a !=18 && a !=42 && a !=16 &&
                    a != 7 && a != 40 && a != 11 &&
                    a != 39&& a != 9 &&a!=17 &&a!=32&&a!=80)
                {
                    bool forward_ = false;
                    if (direction.ToString() == "Forward")
                    {
                        forward_ = true;
                    }

                    else if (direction.ToString() == "Backward")
                    {
                        forward_ = false;
                    }

                    else if (direction.ToString() == "HugLeft")
                    {
                        if ((b + 2) % 2 == 0)
                            forward_ = true;
                        else
                            forward_ = false;
                    }

                    else if (direction.ToString() == "HugRight")
                    {
                        if ((b + 2) % 2 == 0)
                            forward_ = false;
                        else
                            forward_ = true;
                    }

                    else if (direction.ToString() == "WeaveLeft")
                    {
                        if (b == 1 || b == 2 || (b - 1) % 4 == 0 || (b - 2) % 4 == 0)
                            forward_ = false;
                        else forward_ = true;
                    }

                    else if (direction.ToString() == "WeaveRight")
                    {
                        if (b == 1 || b == 2 || (b - 1) % 4 == 0 || (b - 2) % 4 == 0)
                            forward_ = true;
                        else forward_ = false;
                    }

                    Vector3 myVector = Vector3.zero;

                    myVector = points[b, a + 1] - points[b, a];

                    float myVectorMagnitude = myVector.magnitude;
                    int peopleCount = (int)((density / 5.0f) * myVectorMagnitude);

                    if (peopleCount < 1) peopleCount = 1;

                    //float step = myVectorMagnitude / peopleCount;
                    //float totalStep = 0;

                    for (int i = 0; i < peopleCount; i++)
                    {
                        float randomPos = 0;
                        randomPos = Random.Range(-4f, 4f);
                        var people = gameObject;
                        bool gender = NPCAttributeController.GenerateGender();
                        if (gender == true)
                        {
                            int prefabNum = UnityEngine.Random.Range(0, 7);
                            if (forward_)
                                people = Instantiate(peoplePrefabs[prefabNum], points[b, a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity) as GameObject;
                            else
                                people = Instantiate(peoplePrefabs[prefabNum], points[b, a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity) as GameObject;
                        }
                        else
                        {
                            int prefabNum = UnityEngine.Random.Range(7, peoplePrefabs.Length);
                            if (forward_)
                                people = Instantiate(peoplePrefabs[prefabNum], points[b, a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity) as GameObject;
                            else
                                people = Instantiate(peoplePrefabs[prefabNum], points[b, a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity) as GameObject;
                        }
                        people.transform.parent = par.transform;
                        people.gameObject.transform.Rotate(new Vector3(0, -180, 0));
                        changeLayerTarget(people.transform);

                        var _movePath = people.AddComponent<MovePath3>();
                        _movePath.walkPath = gameObject;

                        _movePath.MyStart(b, a,"listen", loopPath, forward_,gender );
                    }
                }
            }
        }
    }
    public void DrawCurved(bool withDraw)
    {
        if (numberOfWays < 1) numberOfWays = 1;
        if (lineSpacing < 0.6f) lineSpacing = 0.6f;

        _forward = new bool[numberOfWays];


        for (int w = 0; w < numberOfWays; w++)
        {

            if (direction.ToString() == "Forward")
            {
                _forward[w] = true;
            }

            else if (direction.ToString() == "Backward")
            {
                _forward[w] = false;
            }

            else if (direction.ToString() == "HugLeft")
            {
                if ((w + 2) % 2 == 0)
                    _forward[w] = true;
                else
                    _forward[w] = false;
            }

            else if (direction.ToString() == "HugRight")
            {
                if ((w + 2) % 2 == 0)
                    _forward[w] = false;
                else
                    _forward[w] = true;
            }

            else if (direction.ToString() == "WeaveLeft")
            {
                if (w == 1 || w == 2 || (w - 1) % 4 == 0 || (w - 2) % 4 == 0)
                    _forward[w] = false;
                else _forward[w] = true;
            }

            else if (direction.ToString() == "WeaveRight")
            {
                if (w == 1 || w == 2 || (w - 1) % 4 == 0 || (w - 2) % 4 == 0)
                    _forward[w] = true;
                else _forward[w] = false;
            }

        }

        if (pathPoint.Count < 2) return;

        /// <summary>
        /// 初始化点的连接关系
        /// </summary>
        for (int i = 0; i <= pnum; i++)
            for (int j = 0; j <= pnum; j++)
                connects[i, j] = 0;
        InitConnect();
        for (int i = 1; i <= pnum; i++)
            for (int j = 1; j <= pnum; j++)
                if (connects[i, j] == 1)
                    connects[j, i] = 1;



        points = new Vector3[numberOfWays + 2, pathPoint.Count + 2];

        pointLength[0] = pathPoint.Count + 2;


        for (int i = 0; i < pathPoint.Count; i++)
        {
            points[0, i + 1] = pathPointTransform[i].transform.position;
        }
        //头尾各重复一个点
        points[0, 0] = points[0, 1];
        points[0, pointLength[0] - 1] = points[0, pointLength[0] - 2];



        for (int i = 1; i <= pathPoint.Count; i++)
            if (withDraw)
            {
                //Gizmos.color = (_forward[0] ? Color.green : Color.red);
                Gizmos.color = Color.blue;
                for (int j = 1; j <= pathPoint.Count; j++)
                    if (connects[i, j] == 1)
                        Gizmos.DrawLine(points[0, i], points[0, j]);
            }

        for (int w = 1; w < numberOfWays; w++)
        {
            
            if (numberOfWays > 1)
            {
                if (!loopPath)
                {
                    Vector3 vectorStart = points[0, 2] - points[0, 1];
                    Vector3 pointVectorStart = vectorStart;
                    pointVectorStart = Quaternion.Euler(0, -90, 0) * pointVectorStart;

                    if (w % 2 == 0)
                        pointVectorStart = pointVectorStart.normalized * (float)(w * 0.5f * lineSpacing);
                    else if (w % 2 == 1)
                        pointVectorStart = pointVectorStart.normalized * (float)((w + 1) * 0.5f * lineSpacing);

                    Vector3 pointStart1 = Vector3.zero;
                    if (w % 2 == 1)
                        pointStart1 = (points[0, 1] - pointVectorStart);
                    else if (w % 2 == 0)
                        pointStart1 = (points[0, 1] + pointVectorStart);

                    pointStart1.y = points[0, 1].y;

                    points[w, 0] = pointStart1;
                    points[w, 1] = pointStart1;


                    Vector3 vectorFinish = points[0, pointLength[0] - 3] - points[0, pointLength[0] - 2];
                    Vector3 pointVectorFinish = vectorFinish;
                    pointVectorFinish = Quaternion.Euler(0, 90, 0) * pointVectorFinish;

                    if (w % 2 == 0)
                        pointVectorFinish = pointVectorFinish.normalized * (float)(w * 0.5f * lineSpacing);
                    else if (w % 2 == 1)
                        pointVectorFinish = pointVectorFinish.normalized * (float)((w + 1) * 0.5f * lineSpacing);

                    Vector3 pointFinish1 = Vector3.zero;

                    if (w % 2 == 1)
                        pointFinish1 = points[0, pointLength[0] - 2] - pointVectorFinish;
                    else if (w % 2 == 0)
                        pointFinish1 = points[0, pointLength[0] - 2] + pointVectorFinish;

                    pointFinish1.y = points[0, pointLength[0] - 2].y;

                    points[w, pointLength[0] - 2] = pointFinish1;
                    points[w, pointLength[0] - 1] = pointFinish1;
                }

                for (int i = 1; i <= pathPoint.Count; i++)
                {
                    Vector3 vectorNext = points[0, i] - points[0, i + 1];
                    Vector3 vectorPrev = points[0, i - 1] - points[0, i];

                    Vector3 pointVector1 = vectorPrev;
                    Vector3 pointVector2 = vectorNext;

                    float angle = Mathf.DeltaAngle(Mathf.Atan2(pointVector1.x, pointVector1.z) * Mathf.Rad2Deg,
                            Mathf.Atan2(pointVector2.x, pointVector2.z) * Mathf.Rad2Deg);

                    if (w % 2 == 0)
                        pointVector1 = pointVector1.normalized * (float)(w * 0.5f * lineSpacing);
                    else if (w % 2 == 1)
                        pointVector1 = pointVector1.normalized * (float)((w + 1) * 0.5f * lineSpacing);

                    pointVector1 = Quaternion.Euler(0, 90 + angle / 2, 0) * pointVector1;

                    Vector3 point1 = Vector3.zero;
                    if (w % 2 == 1)
                    {
                        point1 = points[0, i] - pointVector1;
                    }
                    else if (w % 2 == 0)
                        point1 = points[0, i] + pointVector1;

                    point1.y = points[0, i].y;
                    point1.x = point1.x + w / 10;

                    points[w, i] = point1;
                }
                for (int i = 1; i <= pathPoint.Count; i++)
                {
                    if (withDraw)
                    {
                        //Gizmos.color = (_forward[w] ? Color.green : Color.red);
                        Gizmos.color = Color.blue;
                        for (int j = 1; j <= pathPoint.Count; j++)
                            if (connects[i, j] == 1)
                                Gizmos.DrawLine(points[w, j], points[w, i]);
                    }
                }
            }
        }
    }

    public void Floyd()
    {
        for (int i = 1; i <= pnum; i++)
        {
            for (int j = 1; j <= pnum; j++)
            {
                path[i, j] = -1;
                if (connects[i, j] == 1)
                    shortpath[i, j] = connects[i, j];
            }
        }
        for (int k = 1; k <= pnum; k++)
        {
            for (int i = 1; i <= pnum; i++)
            {
                for (int j = 1; j <= pnum; j++)
                {
                    if (shortpath[i, k] != 65536 && shortpath[k, j] != 65536
                    && shortpath[i, k] + shortpath[k, j] < shortpath[i, j])
                    {
                        shortpath[i, j] = shortpath[i, k] + shortpath[k, j];
                        path[i, j] = k;
                    }
                }
            }
        }
    }
    public void FindPath(int i, int j)
    {
        int k = path[i, j];
        if (k == -1)
            return;
        FindPath(i, k);
        result[label, z] = k;
        z = z + 1;
        FindPath(k, j);
    }
    public void FindCheapestPath(int begin, int end)
    {
        result[label, z] = begin;
        z = z + 1;
        FindPath(begin, end);
        result[label, z] = end;
        z = 1;
    }
    void InitBezier(int first, int second, int third, int w, int label)
    {
        pointB = new List<Vector3>();
        for (int i = 0; i < 10; i++)
        {
            //一
            Vector3 pos1 = Vector3.Lerp(getNextPoint(w, first), getNextPoint(w, second), i / 100f * 20f);
            Vector3 pos2 = Vector3.Lerp(getNextPoint(w, second), getNextPoint(w, third), i / 100f * 20f);
            //二
            Vector3 find = Vector3.Lerp(pos1, pos2, i / 100f * 20f);
            Bpath[label, i] = find;
        }
    }
    public void CalPath()
    {
        for (int i = 1; i <= pnum; i++)
            for (int j = 1; j <= pnum; j++)
            {
                shortpath[i, j] = 65536;
                result[i, j] = 0;
            }
        Floyd();
        for (int i = 1; i <= pnum; i++)
        {
            if (Split == false)
            {
                if (i != dest)
                {
                    label = i;
                    FindCheapestPath(i, dest);
                }
                else
                    if (i == dest)
                {
                    label = i;
                    result[label, 1] = label;
                    result[label, 2] = dest;
                }
            }
            /*else if (Split == true)
            {
                if (i == 14 || i == 13 || i == 12 || i == 4 || i == 3
                    || i == 2 || i == 1 || i == 11 || i == 10 || i == 9
                    || i == 8 || i == 7 || i == 5 || i == 58 || i == 59
                    || i == 35 || i == 36 || i == 37 || i == 38 || i == 33
                    || i == 34 || i == 55 || i == 57 || i == 6 || i == 60)
                {
                    if (i != 60)
                    {
                        label = i;
                        FindCheapestPath(i, 60);
                    }
                    else if (i == 60)
                    {
                        label = i;
                        result[label, 1] = label;
                        result[label, 2] = 60;
                    }
                }
                else
                {
                    if (i != 16)
                    {
                        label = i;
                        FindCheapestPath(i, 16);
                    }
                    else
                    if (i == 16)
                    {
                        label = i;
                        result[label, 1] = label;
                        result[label, 2] = 16;
                    }
                }
            }*/
        }
    }
    void Bezier(int wp)
    {
        InitBezier(20, 3, 29, wp, bx); bx++;//bx=0 20,3,29
        InitBezier(20, 3, 28, wp, bx); bx++;//bx=1 20,3,28
        InitBezier(26, 24, 27, wp, bx); bx++;//bx=2 26,24,27
        InitBezier(23, 24, 27, wp, bx); bx++;//bx=3 23, 24, 27
        InitBezier(37, 14, 38, wp, bx); bx++;//bx=4 37, 14, 38
        InitBezier(36, 14, 48, wp, bx); bx++;//bx=5 36, 14, 38
        InitBezier(35, 50, 49, wp, bx); bx++;//bx=6 35, 14, 38
        InitBezier(39, 40, 15, wp, bx); bx++;//bx=7 39, 40, 15

    }
    void InitConnect()
    {
        //My implements
        connects[28,81]= 1;
        connects[81,82]= 1;
        connects[82,83]= 1;
        connects[83,84]= 1;
        connects[84,85]= 1;
        connects[85,86]= 1;
        connects[86,87]= 1;
        connects[87,88]= 1;
        connects[88,89]= 1;
        connects[89,90]= 1;
        connects[90,91]= 1;
        connects[91,93]= 1;
        connects[93,33]= 1;
        connects[42,92]= 1;
        connects[92,33]= 1;
        connects[40, 94] = 1;
        connects[94, 95] = 1;


        connects[14, 15] = 1;
        connects[9, 12] = 1;
        connects[10, 8] = 1;
        connects[13, 8] = 1;
        connects[13, 11] = 1;
        connects[11, 7] = 1;
        //connects[7, 16] = 1;
        connects[16, 18] = 1;
        connects[7, 32] = 1;
        connects[17, 33] = 1;
        connects[16, 17] = 1;
        connects[18, 19] = 1;
        connects[19, 6] = 1;
        connects[6, 20] = 1;
        connects[21, 44] = 1;
        connects[44,43]= 1;
        connects[43,20]= 1;
        connects[5, 3] = 1;
        connects[3, 31] = 1;
        connects[30, 29] = 1;
        connects[4, 29] = 1;
        connects[22, 29] = 1;
        connects[23, 2] = 1;
        connects[30, 21] = 1;
        //connects[35,2 ] = 1;
        connects[1,37] = 1;

        connects[15,12]= 1;
        connects[9,39]= 1;
        connects[39,40]= 1;
        connects[42,45]= 1;
        connects[45,46]= 1;
        connects[46,47]= 1;
        connects[47,48]= 1;
        connects[48,49]= 1;
        connects[49,50]= 1;

        connects[5,41]= 1;
        connects[41,50]= 1;
        connects[24,35 ]= 1;
        //connects[23,24]= 1;
        connects[4,36 ] = 1;
        connects[36,22] = 1;
        connects[27,26] = 1;
        connects[2,31] = 1;
        connects[37,28] = 1;
        //connects[28,3] = 1;
        connects[35,22] = 1;
        connects[27,23] = 1;
        connects[25,1] = 1;
        connects[4,51]= 1;
        connects[4,52]= 1;
        connects[4,53]= 1;
        connects[36,54]= 1;
        connects[36,55]= 1;
        connects[36,56]= 1;
        connects[35,61]= 1;
        connects[35,62]= 1;
        connects[35,63]= 1;
        connects[24,57]= 1;
        connects[24,58]= 1;
        connects[24,59]= 1;
        connects[27,60]= 1;
        connects[27,64]= 1;
        connects[27,65]= 1;
        connects[26,66]= 1;
        connects[26,67]= 1;
        connects[26,68]= 1;
        connects[25,69]= 1;
        connects[25,70]= 1;
        connects[25,71]= 1;
        connects[14,72 ] = 1;
        connects[14,73] = 1;
        connects[14,74] = 1;
        connects[14,75] = 1;
        connects[10,76] = 1;
        connects[10,77] = 1;
        connects[10,78] = 1;
        connects[10,79] = 1;
        connects[26,25]= 1;
        connects[38, 37] = 1;
        connects[33, 80] = 1;
        connects[95, 32] = 1;
        connects[33, 32] = 1;
        connects[32, 34] = 1;
        connects[80, 34] = 1;
    }
    void EasyConnect()
    {
        for (int i = 1; i <= pnum; i++)
            for (int j = 0; j < 6; j++)
                easyconnect[i, j] = -1;
        for (int i = 1; i < pnum; i++)
        {
            int num = 1;
            for (int j = 0; j < pnum; j++)
                if (connects[i, j] == 1)
                {
                    easyconnect[i, num] = j;
                    num++;
                }
        }
    }
    public void changeLayerTarget(Transform f)
    {
        f.gameObject.tag = "run3";
        f.gameObject.layer = LayerMask.NameToLayer("target");
        Transform[] ff = f.GetComponentsInChildren<Transform>();
        foreach (Transform c in ff)
        {
            c.gameObject.layer = LayerMask.NameToLayer("target");
        }
    }
    public void singleSpawn(int a)
    {
        int prefabNum = UnityEngine.Random.Range(0, peoplePrefabs.Length);
        var people = gameObject;
        if (peoplePrefabs[prefabNum] == null)
        {
            Debug.Log("peoplePrefabs[prefabNum]");

        }
        if (points[0, a] == null)
        {
            Debug.Log("points[0, a]");
        }
        people = (GameObject)Instantiate(peoplePrefabs[prefabNum], points[0, a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity) ;
        changeLayerTarget(people.transform);
        people.gameObject.transform.Rotate(new Vector3(0, -90, 0));
        var _movePath = people.AddComponent<MovePath3>();
        people.transform.parent = par.transform;
        _movePath.walkPath = gameObject;

        string animName;
        /*if (isWalk)
            animName = "walk";
        else
            animName = "run";*/
        bool forward_ = false;
        bool gender = NPCAttributeController.GenerateGender();
        _movePath.MyStart(0, a, /*animName*/"listen", loopPath, forward_, gender);
    }
    //3.26
    //初始化各个run点的list
    public void initialPeopleToward()
    {
        people_toward = new List<MovePathGeneral>[pnum + 1];
        for (int i = 0; i <= pnum; i++)
        {
            people_toward[i] = new List<MovePathGeneral>();
        }
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.tag == "run3" & child.gameObject.layer == LayerMask.NameToLayer("target"))
            {
                int cnt = child.gameObject.GetComponent<MovePath3>().targetPoint;
                people_toward[cnt].Insert(people_toward[cnt].Count, child.GetComponent<MovePath3>());
            }
        }
    }
    //3.26
    //变化时调用
    public void modifyPeopleToward(MovePathGeneral me, int from, int to)
    {
        lock (locker)
        {
            people_toward[from].Remove(me);
            people_toward[to].Insert(people_toward[to].Count, me);
            inspect(3);
        }

    }
    //3.26
    public void inspect(int thres)
    {
        for (int i = 0; i < parallerPoint.Length; i++)
        {
            if (parallerPoint[i].p1 <= pnum && parallerPoint[i].p2 <= pnum)
            {
                if (judgeFunction(people_toward[parallerPoint[i].p1].Count, people_toward[parallerPoint[i].p2].Count, thres))
                {
                    parallelNodeOperating(parallerPoint[i].p1, parallerPoint[i].p2, thres);
                }
            }
            else
            {
                Debug.Log("Points out arrayBorder!!!!");
            }

        }

    }
    //3.26
    public bool judgeFunction(int x, int y, float thres)
    {
        if (x < y) { int tmp = x; x = y; y = tmp; }
        if (x <= thres / 2)
        {
            return false;
        }
        if (x < thres)
        {
            return (y - x + thres / 2) < 0;
        }
        else
        {
            return (y - (x + Mathf.Sqrt(Mathf.Pow(x, 2) - Mathf.Pow(thres, 2))) / 2) < 0;
        }
    }
    //3.26
    public void parallelNodeOperating(int from, int to, int thres)
    {
        if (people_toward[to].Count > people_toward[from].Count)
        {
            int tmp = to;
            to = from;
            from = tmp;
        }

        int transCNT = (people_toward[from].Count - people_toward[to].Count) / 2;

        for (int i = 0; i < transCNT; i++)
        {
            int cnt = (int)Random.Range(0, people_toward[from].Count - 0.00001f);
            MovePath3 mvp = (MovePath3)people_toward[from][cnt];
            //如果太靠近from则到to的下一个点
            if (Vector3.Distance(mvp.transform.position, mvp.finishPos) <= 2.0f)
            {
                to = result[to, 2];
            }

            people_toward[from].Remove(mvp);
            people_toward[to].Insert(people_toward[to].Count, mvp);
            mvp.targetPoint = to;
            mvp.finishPos = getNextPoint(mvp.w, mvp.targetPoint);
            mvp.finishPos.y = mvp.hit.point.y;
        }
    }
}
