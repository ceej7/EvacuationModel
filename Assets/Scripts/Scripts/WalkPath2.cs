    using UnityEngine;
using System.Collections.Generic;
using System.Threading;

[System.Serializable]
public class WalkPath2 : MonoBehaviour
{
    public GameObject[] peoplePrefabs;
    public GameObject par;
    public float moveSpeed = 1;
    public float density;

    //3.26
    public List<MovePathGeneral>[] people_toward;
    static Object locker = new Object();
    public TwoDimensionPoint[] parallerPoint;

    public List<Vector3> pathPoint = new List<Vector3>();
    public List<GameObject> pathPointTransform = new List<GameObject>();
    [HideInInspector]
    public List<Vector3> pointB = new List<Vector3>();
    public int z = 1, label = 0, bx = 0, Bcounts = Bcount;
    public int[,] easyconnect = new int[pnum + 1, 10];
    public int[,] shortpath = new int[pnum + 1, pnum + 1];
    public int[,] path = new int[pnum + 1, pnum + 1];
    public int[,] result = new int[pnum + 1, pnum + 1];
    public int[] pointLength = new int[10];
    public int[,] connects= new int[pnum + 1, pnum + 1];
    public Vector3[,] Bpath = new Vector3[pnum + 1, Bcount];
    public bool[,] Run = new bool[pnum + 1, 2];
    float TempTime = 0f;
    public float TTime = 0f;
    public bool Split = false;
    public enum EnumDir { Forward, Backward, HugLeft, HugRight, WeaveLeft, WeaveRight };
    public enum EnumMove { Walk, Run };
    public Vector3[,] points;
    public bool[] _forward;
    static public int pnum = 218, Bcount = 11;
    public EnumDir direction;
    public EnumMove _moveType;
    public int numberOfWays;
    public bool loopPath;
    public float lineSpacing;
    public bool StartRun = false;
    public string str = null;
    public Thread gasThread, spreadThread;
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
            Run[25, 0] = true; Run[25, 1] = true;
            Run[27, 0] = true; Run[27, 1] = true;
            Run[28, 0] = true; Run[28, 1] = true;
            Run[30, 0] = true; Run[30, 1] = true;
            Run[32, 0] = true; Run[32, 1] = true;
            Run[33, 0] = true; Run[33, 1] = true;
            Run[34, 0] = true; Run[34, 1] = true;
            Run[35, 0] = true; Run[35, 1] = true;
            Run[17, 0] = true; Run[17, 1] = true;
            Run[18, 0] = true; Run[18, 1] = true;
            Run[40, 0] = true; Run[40, 1] = true;
            Run[42, 0] = true; Run[42, 1] = true;
            Run[43, 0] = true; Run[43, 1] = true;
            Run[46, 0] = true; Run[46, 1] = true;
            Run[45, 0] = true; Run[45, 1] = true;
            Run[206, 0] = true;Run[206, 1] = true;
            Run[200, 0] = true; Run[200, 1] = true;
            for (int i = 1; i <= pnum; i++)
            {
                Run[i, 0] = true; Run[i, 1] = true;
            }
            Split = false;
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
                if (a != 47&&a!=48&&a!=92&&
                    a!=93&&a!=94&&a!=25&&a!=27)
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
                        changeLayerTarget(people.transform);
                        if (a ==24|| a == 49|| a == 50||
                            a == 28 || a == 52|| a == 23 ||
                            a == 51 || a == 30 || a == 54 ||
                            a == 53 || a == 22 || a == 32 ||
                            a == 56 || a == 55 || a == 21 ||
                            a == 33 || a == 58 || a == 57||
                            a == 20 || a == 34 || a == 63||
                            a==19||a==60||a==62||
                            a == 61 || a == 59 || a == 35 ||
                            a == 18 || a == 17 || a == 40 ||
                            a == 65 || a == 64 || a == 16 ||
                            a == 42 || a == 67 || a == 66 ||
                            a == 15 || a == 43 || a == 69 ||
                            a == 68 || a == 14 || a == 46 ||
                            a == 70 || a == 13 || a == 71)
                        {
                            people.gameObject.transform.Rotate(new Vector3(0, -90, 0));
                        }
                        else
                        {
                            people.gameObject.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        people.transform.parent = par.transform;
                        var _movePath = people.AddComponent<MovePath2>();
                        _movePath.walkPath = gameObject;
                        _movePath.MyStart(b, a, /*animName*/"listen", loopPath, forward_,gender);
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
        for (int i = 1; i <= pnum; i++)
        {
            if (Split == false)
            {
                if (i != 94)
                {
                    label = i;
                    FindCheapestPath(i, 94);
                }
                else
                    if (i == 94)
                {
                    label = i;
                    result[label, 1] = label;
                    result[label, 2] = 94;
                }
            }      
        }
    }
    void Bezier(int wp)
    {
       /* InitBezier(82, 3, 29, wp, bx); bx++;//bx=0 82,3,29
        InitBezier(26, 24, 27, wp, bx); bx++;//bx=1 26,24,27
        InitBezier(23, 24, 27, wp, bx); bx++;//bx=2 23, 24, 27
        InitBezier(92, 14, 98, wp, bx); bx++;//bx=3 92, 14, 98
        InitBezier(93, 14, 97, wp, bx); bx++;//bx=4 36, 14, 38
        InitBezier(94, 50, 49, wp, bx); bx++;//bx=5 35, 14, 38
        InitBezier(39, 40, 15, wp, bx); bx++;//bx=6 39, 40, 15*/

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
        connects[215, 25] = 1;
        connects[12, 45] = 1;
        connects[13, 46] = 1;
        connects[11, 44] = 1;
        connects[14, 43] = 1;
        connects[15, 42] = 1;
        connects[45, 44] = 1;
        connects[46, 214] = 1;
        connects[42, 40] = 1;
        connects[16, 40] = 1;
        connects[10, 41] = 1;
        connects[44, 41] = 1;
        connects[9, 39] = 1;
        connects[41, 39] = 1;
        connects[39, 8] = 1;
        connects[186, 8] = 1;
        connects[185, 17] = 1;
        connects[8, 184] = 1;
        connects[184, 188] = 1;
        connects[185, 187] = 1;
        connects[8, 7] = 1;
        connects[17, 18] = 1;
        connects[7, 36] = 1;
        connects[18, 35] = 1;
        connects[36, 37] = 1;
        connects[35, 34] = 1;
        connects[6, 37] = 1;
        connects[19, 34] = 1;
        connects[5, 38] = 1;
        connects[20, 33] = 1;
        connects[4, 31] = 1;
        connects[21, 32] = 1;
        connects[37, 38] = 1;
        connects[38, 31] = 1;
        connects[34, 33] = 1;
        connects[33, 32] = 1;
        connects[3, 29] = 1;
        connects[22, 30] = 1;
        connects[2, 26] = 1;
        connects[23, 28] = 1;
        connects[1, 27] = 1;
        connects[24, 25] = 1;
        connects[31, 29] = 1;
        connects[32, 30] = 1;
        connects[29, 26] = 1;
        connects[30, 28] = 1;
        connects[26, 27] = 1;
        connects[28, 25] = 1;
        connects[27, 47] = 1;
        connects[25, 48] = 1;
        connects[25, 50] = 1;
        connects[25, 49] = 1;
        connects[28, 52] = 1;
        connects[28, 51] = 1;
        connects[30, 54] = 1;
        connects[30, 53] = 1;
        connects[32, 55] = 1;
        connects[32, 56] = 1;
        connects[33, 58] = 1;
        connects[33, 57] = 1;
        connects[34, 62] = 1;
        connects[34, 61] = 1;
        connects[40, 65] = 1;
        connects[40, 64] = 1;
        connects[42, 67] = 1;
        connects[42, 66] = 1;
        connects[43, 69] = 1;
        connects[43, 68] = 1;
        connects[46, 70] = 1;
        connects[46, 71] = 1;
        connects[45, 73] = 1;
        connects[45, 72] = 1;
        connects[44, 75] = 1;
        connects[44, 74] = 1;
        connects[41, 77] = 1;
        connects[41, 76] = 1;
        connects[39, 79] = 1;
        connects[39, 78] = 1;
        connects[37, 81] = 1;
        connects[37, 80] = 1;
        connects[38, 82] = 1;
        connects[38, 83] = 1;
        connects[31, 84] = 1;
        connects[31, 85] = 1;
        connects[29, 87] = 1;
        connects[29, 86] = 1;
        connects[26, 88] = 1;
        connects[26, 89] = 1;
        connects[27, 91] = 1;
        connects[27, 90] = 1;
        connects[47, 92] = 1;
        connects[92, 93] = 1;
        connects[48, 93] = 1;
        connects[93, 94] = 1;
        connects[34, 60] = 1;
        connects[34, 59] = 1;
        connects[34, 63] = 1;
        connects[40, 17] = 1;
        connects[43, 42] = 1;
        connects[214, 213] = 1;
        connects[27, 189] = 1;
        for (int i = 189; i < 199; i++)
            connects[i, i + 1] = 1;
        connects[25, 200] = 1;
        for (int i = 200; i < 206; i++)
            connects[i, i + 1] = 1;
        connects[186, 207] = 1;
        for (int i = 207; i < 210; i++)
            connects[i, i + 1] = 1;
        for (int i = 0; i <= 52; i += 13)
        {
            connects[102 + i, 99 + i] = 1;
            connects[99 + i, 98 + i] = 1;
            connects[98 + i, 95 + i] = 1;
            connects[101 + i, 100 + i] = 1;
            connects[100 + i, 97 + i] = 1;
            connects[97 + i, 95 + i] = 1;
            connects[96 + i, 104 + i] = 1;
            connects[96 + i, 103 + i] = 1;
            connects[104 + i, 107 + i] = 1;
            connects[103 + i, 106 + i] = 1;
            connects[105 + i, 103 + i] = 1;
        }
        for (int i = 0; i <= 18; i += 6)
        {
            connects[160 + i, 161 + i] = 1;
            connects[160 + i, 162 + i] = 1;
            connects[160 + i, 163 + i] = 1;
            connects[163 + i, 164 + i] = 1;
            connects[163 + i, 165 + i] = 1;
        }
        connects[185, 211] = 1;
        connects[211, 212] = 1;
        connects[212, 213] = 1;

        connects[206, 216] = 1;
        connects[216, 217] = 1;
        connects[217, 218] = 1;
    }
    void EasyConnect()
    {
        for (int i = 1; i <= pnum; i++)
            for (int j = 0; j < 10; j++)
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
    public void reConnect()
    {
        connects[95, 51] = 1;
        connects[96, 23] = 1;
        connects[108, 22] = 1;
        connects[109, 22] = 1;
        connects[122, 18] = 1;
        connects[121, 18] = 1;
        connects[135, 17] = 1;
        connects[134, 17] = 1;
        connects[148, 13] = 1;
        connects[147, 71] = 1;
        connects[160, 20] = 1;
        connects[166, 19] = 1;
        connects[172, 16] = 1;
        connects[178, 14] = 1;
        
        for (int i = 1; i <= pnum; i++)
            for (int j = 1; j <= pnum; j++)
                if (connects[i, j] == 1)
                    connects[j, i] = 1;

        Run[96, 0] = true; Run[96, 1] = true;
        Run[95, 0] = true; Run[95, 1] = true;
        Run[109, 0] = true; Run[109, 1] = true;
        Run[108, 0] = true; Run[108, 1] = true;
        Run[160, 0] = true; Run[160, 1] = true;
        Run[166, 0] = true; Run[166, 1] = true;
        Run[122, 0] = true; Run[122, 1] = true;
        Run[121, 0] = true; Run[121, 1] = true;
        Run[135, 0] = true; Run[135, 1] = true;
        Run[134, 0] = true; Run[134, 1] = true;
        Run[172, 0] = true; Run[172, 1] = true;
        Run[178, 0] = true; Run[178, 1] = true;
        Run[147, 0] = true; Run[147, 1] = true;
        Run[148, 0] = true; Run[148, 1] = true;
        Split = false;
        CalPath();
        EasyConnect();
    }
    public void changeLayerTarget(Transform f)
    {
        f.gameObject.tag = "run2";
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
        people = (GameObject)Instantiate(peoplePrefabs[prefabNum], points[0, a] /*+ new Vector3(randomPos, 0, randomPos)*/, Quaternion.identity);
        changeLayerTarget(people.transform);
        people.gameObject.transform.Rotate(new Vector3(0, -90, 0));
        var _movePath = people.AddComponent<MovePath2>();
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
            if (child.tag == "run2" & child.gameObject.layer == LayerMask.NameToLayer("target"))
            {
                int cnt = child.gameObject.GetComponent<MovePath2>().targetPoint;
                people_toward[cnt].Insert(people_toward[cnt].Count, child.GetComponent<MovePath2>());
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
            MovePath2 mvp = (MovePath2)people_toward[from][cnt];
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

