using UnityEngine;
using System.Collections.Generic;
using System.Threading;

[System.Serializable]
public class WalkPath : MonoBehaviour
{
    public List<Vector3> pathPoint = new List<Vector3>();
    public List<GameObject> pathPointTransform = new List<GameObject>();

    public GameObject[] peoplePrefabs;
    public GameObject par;
    public float moveSpeed = 1;
    public bool isWalk;
    public float density;

    //3.26
    public List<MovePathGeneral>[] people_toward;
    static Object locker = new Object();
    public TwoDimensionPoint[] parallerPoint;

    //run0中的火焰断路
    public bool isis = false;

    [HideInInspector]
    public List<Vector3> pointB = new List<Vector3>();
    static public int pnum = 136,Bcount = 11;
    public int z = 1, label = 0, bx = 0, Bcounts = Bcount;
    public int[,] easyconnect = new int[pnum + 1, 8];
    public int[,] shortpath = new int[pnum + 1, pnum + 1];
    public int[,] path = new int[pnum + 1, pnum + 1];
    public int[,] result = new int[pnum + 1, pnum + 1];
    public int[] pointLength = new int[10];
    public int[,] connects= new int[pnum + 1, pnum + 1];
    public Vector3[,] Bpath = new Vector3[pnum + 1, Bcount];
    public enum EnumDir { Forward, Backward, HugLeft, HugRight, WeaveLeft, WeaveRight };
    public enum EnumMove { Walk, Run };
    public Vector3[,] points;
    public bool[] _forward;
    public bool Split = false;
    public bool[,] Run = new bool[pnum + 1, 2];
    public EnumDir direction;
    public EnumMove _moveType;
    public int numberOfWays;
    public bool loopPath;
    public float lineSpacing;
    public int failnum = 0;
    public bool StartRun = false;
    public string str = null;
    public Thread gasThread, spreadThread;
    float TempTime = 0f;
    public float TTime = 0f;
    public Vector3 getNextPoint(int w, int index)
    {
        return points[w, index];
    }

    public Vector3 getStartPoint(int w)
    {
        return points[w, 1];
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
    }

    private void Start()
    {

        str = "";
        //3.26
        initialPeopleToward();
    }
    void Update()
    {
        if (TempTime > 0f)
        {
            result[100, 2] = 103;
            result[103, 2] = 104;
            result[104, 2] = 107;
            result[107, 2] = 108;
            result[108, 2] = 111;
            result[111, 2] = 112;
            result[112, 2] = 114;
            result[101, 2] = 102;
            result[102, 2] = 105;
            result[105, 2] = 106;
            result[106, 2] = 109;
            result[109, 2] = 110;
            result[110, 2] = 113;
            result[113, 2] = 115;
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
            //for (int i = 1; i <= pnum; i++)
            //{
            //    Run[i, 0] = true; Run[i, 1] = true;
            //}
            Run[12, 0] = true; Run[12, 1] = true;
            Run[11, 0] = true; Run[11, 1] = true;
            Run[ 60 , 0] = true; Run[ 60 , 1] = true;
            Run[10  , 0] = true; Run[10  , 1] = true;
            Run[ 58 , 0] = true; Run[58  , 1] = true;
            Run[ 9 , 0] = true; Run[ 9 , 1] = true;
            Run[ 56 , 0] = true; Run[ 56 , 1] = true;
            Run[ 8 , 0] = true; Run[ 8 , 1] = true;
            Run[53  , 0] = true; Run[ 53 , 1] = true;
            Run[ 7 , 0] = true; Run[ 7 , 1] = true;
            Run[6  , 0] = true; Run[6  , 1] = true;
            Run[ 43 , 0] = true; Run[ 43 , 1] = true;
            Run[ 17 , 0] = true; Run[17, 1] = true;
            Run[ 22 , 0] = true; Run[22, 1] = true;
            Run[ 118 , 0] = true; Run[118, 1] = true;
            Run[ 68 , 0] = true; Run[68, 1] = true;
            Run[28, 0] = true; Run[28, 1] = true;
            Run[14, 0] = true; Run[14, 1] = true;
            Split = false;
            CalPath();
            EasyConnect();
            StartRun = true;
        }
        else if (str == "Right" && StartRun == false)
        {
            for (int i = 1; i <= pnum; i++)
                for (int j = 1; j <= pnum; j++)
                    connects[i, j] = 0;
            InitConnect();
            for (int i = 1; i <= pnum; i++)
                for (int j = 1; j <= pnum; j++)
                    if (connects[i, j] == 1)
                        connects[j, i] = 1;
            //for (int i = 1; i <= pnum; i++)
            //{
            //    Run[i, 0] = true; Run[i, 1] = true;
            //}
            Run[12, 0] = true; Run[12, 1] = true;
            Run[11, 0] = true; Run[11, 1] = true;
            Run[60, 0] = true; Run[60, 1] = true;
            Run[10, 0] = true; Run[10, 1] = true;
            Run[58, 0] = true; Run[58, 1] = true;
            Run[9, 0] = true; Run[9, 1] = true;
            Run[56, 0] = true; Run[56, 1] = true;
            Run[8, 0] = true; Run[8, 1] = true;
            Run[53, 0] = true; Run[53, 1] = true;
            Run[7, 0] = true; Run[7, 1] = true;
            Run[6, 0] = true; Run[6, 1] = true;
            Run[43, 0] = true; Run[43, 1] = true;
            Run[17, 0] = true; Run[17, 1] = true;
            Run[22, 0] = true; Run[22, 1] = true;
            Run[118, 0] = true; Run[118, 1] = true;
            Run[68, 0] = true; Run[68, 1] = true;
            Run[28, 0] = true; Run[28, 1] = true;
            Run[14, 0] = true; Run[14, 1] = true;
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
                if (a != 29 && a != 28 && a != 27 &&
                    a != 85 && a != 84 && a != 83 &&
                    a != 30 && a != 31 && a != 32 &&
                    a != 88 && a != 87 && a != 86 &&
                    a != 13 && a != 34 && a != 33 &&
                    a != 89 && a != 90 && a != 91 &&
                    a != 35 && a != 36 && a != 37 &&
                    a != 92 && a != 93 && a != 94 &&
                    a != 49 && a != 97 && a != 98 &&
                    a != 96 && a != 95 && a != 39 &&
                    a != 14 && a != 50 && a != 51 &&
                    a != 40 && a != 15 && a != 16 &&
                    a != 38 && a != 48 && a != 100 &&
                    a != 101 && a != 102 && a != 103 &&
                    a != 104 && a != 105 && a != 106 &&
                    a != 107 && a != 108 && a != 109 &&
                    a != 110 && a != 111 && a != 112 && a != 113
                    && a != 64 && a != 1 && a != 65)
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
                        randomPos = Random.Range(-4, 4);
                        //new people
                        var people = gameObject;
                        bool gender = NPCAttributeController.GenerateGender();           
                        if(gender==true)
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
                        people.gameObject.transform.Rotate(new Vector3(0, -90, 0));
                        changeLayerTarget(people.transform);
                        //new Script
                        var _movePath = people.AddComponent<MovePath>();
                        _movePath.walkPath = gameObject;

                        _movePath.MyStart(b, a, "listen", loopPath, forward_,gender);
                       
                        

                        
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
                Gizmos.color = (_forward[0] ? Color.green : Color.red);
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
                        Gizmos.color = (_forward[w] ? Color.green : Color.red);
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
        for (int i = 1; i <=pnum; i++)
        {
            if (Split == false)
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
        InitBezier(82, 3, 29, wp, bx); bx++;//bx=0 82,3,29
        InitBezier(26, 24, 27, wp, bx); bx++;//bx=1 26,24,27
        InitBezier(23, 24, 27, wp, bx); bx++;//bx=2 23, 24, 27
        InitBezier(92, 14, 98, wp, bx); bx++;//bx=3 92, 14, 98
        InitBezier(93, 14, 97, wp, bx); bx++;//bx=4 36, 14, 38
        InitBezier(94, 50, 49, wp, bx); bx++;//bx=5 35, 14, 38
        InitBezier(39, 40, 15, wp, bx); bx++;//bx=6 39, 40, 15

    }
    void OnDrawGizmoss()//画线
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < pointB.Count - 1; i++)
        {
            Gizmos.DrawLine(pointB[i], pointB[i + 1]);
        }
    }
    void InitConnect()
    {

        /**
         * My implement
         * */
        connects[1, 65] = 1;
        connects[65, 77] = 1;
        connects[77, 21] = 1;
        connects[12, 17] = 1;
        connects[21, 78] = 1;
        connects[78, 22] = 1;
        connects[22, 79] = 1;
        connects[79, 2] = 1;
        connects[2, 80] = 1;
        connects[80, 18] = 1;
        connects[10, 19] = 1;     
        connects[18, 81] = 1;
        connects[81, 20] = 1;
        connects[11, 47] = 1;
        connects[20, 82] = 1;
        connects[82, 3] = 1;
        connects[3, 46] = 1;
        connects[9, 46] = 1;
        connects[8, 3] = 1;
        connects[7, 23] = 1;
        connects[23, 24] = 1;
        connects[3, 29] = 1;
        //connects[29, 28] = 1;
        connects[24, 27] = 1;
        connects[26, 24] = 1;
        connects[4, 24] = 1;
        connects[5, 25] = 1;
        connects[6, 26] = 1;
        connects[25, 26] = 1;
        connects[27, 83] = 1;
        connects[83, 32] = 1;
        connects[28, 84] = 1;
        connects[84, 31] = 1;
        connects[29, 85] = 1;
        connects[85, 30] = 1;
        connects[32, 86] = 1;
        connects[86, 33] = 1;
        connects[31, 87] = 1;
        connects[87, 34] = 1;
        connects[30, 88] = 1;
        connects[88, 13] = 1;
        connects[33, 91] = 1;
        connects[91, 37] = 1;
        connects[34, 90] = 1;
        connects[90, 36] = 1;
        connects[13, 89] = 1;
        connects[89, 35] = 1;
        connects[37, 92] = 1;
        connects[36, 93] = 1;
        connects[93, 14] = 1;
        connects[92, 14] = 1;
        connects[35, 94] = 1;
        connects[94, 50] = 1;
        connects[50, 49] = 1;
        connects[14, 97] = 1;
        connects[97, 38] = 1;
        connects[38, 96] = 1;
        connects[96, 39] = 1;
        connects[39, 40] = 1;
        connects[40, 15] = 1;
        connects[15, 16] = 1;
        connects[42, 25] = 1;
        connects[41, 25] = 1;
        connects[43, 25] = 1;
        connects[45, 26] = 1;
        connects[44, 26] = 1;
        //connects[27, 28] = 1;
        connects[19, 46] = 1;
        connects[47, 99] = 1;
        connects[99,19] = 1;
        connects[17, 47] = 1;
        connects[14, 98] = 1;
        connects[98, 48] = 1;
        connects[48, 95] = 1;
        connects[95, 51] = 1;
        connects[51, 15] = 1;
        connects[49, 40] = 1;
        connects[50, 49] = 1;
        connects[23, 52] = 1;
        connects[23, 53] = 1;
        connects[3, 54] = 1;
        connects[3, 55] = 1;
        connects[46, 56] = 1;
        connects[46, 57] = 1;
        connects[19, 59] = 1;
        connects[19, 58] = 1;
        connects[47, 61] = 1;
        connects[47, 60] = 1;
        connects[17, 62] = 1;
        connects[17, 63] = 1;
        connects[64, 66] =1;
        connects[66, 67] = 1;
        connects[67, 68] = 1;
        connects[68, 69] = 1;
        connects[69, 70] = 1;
        connects[70, 71] = 1;
        connects[71, 72] = 1;
        connects[72, 73] = 1;
        connects[73, 74] = 1;
        connects[74, 75] = 1;
        connects[75, 76] = 1;
        connects[76, 29] = 1;
        connects[14, 100] = 1;
        connects[14, 101] = 1;
        connects[100, 103] = 1;
        connects[101, 102] = 1;
        connects[103, 104] = 1;
        connects[102, 105] = 1;
        connects[104, 107] = 1;
        connects[102, 105] = 1;
        connects[105, 106] = 1;
        connects[107, 108] = 1;
        connects[106, 109] = 1;
        connects[108, 111] = 1;
        connects[109, 110] = 1;
        connects[111, 112] = 1;
        connects[110, 113] = 1;
        connects[113, 115] = 1;
        connects[112, 114] = 1;
        connects[136, 135] = 1;
        connects[ 135  ,134   ]= 1;
        connects[132,133]= 1;
        connects[134,4]= 1;
        connects[133,131]= 1;
        connects[131,27]= 1;
        connects[65,116]= 1;
        connects[116,117]= 1;
        connects[117,118]= 1;
        connects[118,124]= 1;
        connects[124,121]= 1;
        connects[118,119]= 1;
        connects[119,120]= 1;
        connects[120,122]= 1;
        connects[121,125]= 1;
        connects[122,123]= 1;
        connects[125,126]= 1;
        connects[123,127]= 1;
        connects[126,128]= 1;
        connects[127,129]= 1;
        connects[128,130]= 1;
        connects[130,29]= 1;
        connects[129,76]= 1;
        connects[77, 116] = 1;


    }
    void EasyConnect()
    {
        for (int i = 1; i <= pnum; i++)
            for (int j = 0; j < 8; j++)
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
        f.gameObject.tag = "run0";
        f.gameObject.layer= LayerMask.NameToLayer("target");
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
        people = (GameObject)Instantiate(peoplePrefabs[prefabNum], points[0,a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity);
        changeLayerTarget(people.transform);
        people.gameObject.transform.Rotate(new Vector3(0, -90, 0));
        var _movePath = people.AddComponent<MovePath>();
        people.transform.parent = par.transform;
        _movePath.walkPath = gameObject;

        string animName;
        /*if (isWalk)
            animName = "walk";
        else
            animName = "run";*/
        bool forward_ = false;
        float mSpeed = moveSpeed + UnityEngine.Random.Range(moveSpeed * -0.15f, moveSpeed * 0.15f);
        bool gender = NPCAttributeController.GenerateGender();
        _movePath.MyStart(0, a, /*animName*/"listen", loopPath, forward_,gender);
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
            if (child.tag == "run1" & child.gameObject.layer == LayerMask.NameToLayer("target"))
            {
                int cnt = child.gameObject.GetComponent<MovePath>().targetPoint;
                people_toward[cnt].Insert(people_toward[cnt].Count, child.GetComponent<MovePath>());
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
            MovePath mvp = (MovePath)people_toward[from][cnt];
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

