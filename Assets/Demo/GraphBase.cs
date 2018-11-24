using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBase : MonoBehaviour {

    [Header("Visulization")]
    //点集
    public GameObject pointSet;


    [HideInInspector]
    public int pnum;//点数量
    public List<Vector3> pPosition = new List<Vector3>();//位置
    public int[,] connects ;//邻接矩阵
   
    public void Start()
    {
        connects = new int[pnum + 1, pnum + 1];
        for (int i = 0; i <= pnum; i++)
            for (int j = 0; j <= pnum; j++)
                connects[i, j] = 0;
        InitConnect();
        for (int i = 1; i <= pnum; i++)
            for (int j = 1; j <= pnum; j++)
                if (connects[i, j] == 1)
                    connects[j, i] = 1;
        int[,] path=Floyd.floyd(ref pnum,ref connects,ref pPosition);
        int l = 0;
    }

    void InitConnect()
    {
        connects[1, 65] = 1;
        connects[65, 77] = 1;
        connects[77, 21] = 1;
        connects[12, 17] = 1;
        connects[21, 78] = 1;
        connects[78, 22] = 1;
        connects[22, 79] = 1;
        connects[79, 2] = 1;
        connects[2, 18] = 1;
        connects[10, 19] = 1;
        connects[18, 81] = 1;
        connects[81, 20] = 1;
        connects[11, 47] = 1;
        connects[20, 82] = 1;
        connects[3, 46] = 1;
        connects[9, 46] = 1;
        connects[8, 3] = 1;
        connects[7, 23] = 1;
        connects[23, 129] = 1;
        connects[24, 129] = 1;
        connects[24, 28] = 1;
        connects[26, 129] = 1;
        connects[4, 24] = 1;
        connects[5, 25] = 1;
        connects[6, 26] = 1;
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
        connects[35, 94] = 1;
        connects[14, 97] = 1;
        connects[97, 38] = 1;
        connects[38, 96] = 1;
        connects[96, 40] = 1;
        connects[40, 50] = 1;
        connects[50, 49] = 1;
        connects[137, 49] = 1;
        connects[23, 129] = 1;
        connects[15, 16] = 1;
        connects[42, 25] = 1;
        connects[41, 25] = 1;
        connects[43, 25] = 1;
        connects[45, 26] = 1;
        connects[44, 26] = 1;
        connects[19, 71] = 1;
        connects[71, 46] = 1;
        connects[47, 99] = 1;
        connects[99, 19] = 1;
        connects[17, 80] = 1;
        connects[80, 47] = 1;
        connects[98, 48] = 1;
        connects[48, 95] = 1;
        connects[95, 51] = 1;
        connects[51, 15] = 1;
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
        connects[64, 66] = 1;
        connects[66, 67] = 1;
        connects[67, 68] = 1;
        connects[68, 69] = 1;
        connects[69, 70] = 1;
        connects[70, 72] = 1;
        connects[72, 73] = 1;
        connects[73, 75] = 1;
        connects[75, 76] = 1;
        connects[76, 29] = 1;
        connects[14, 100] = 1;
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
        connects[136, 25] = 1;
        connects[136, 26] = 1;
        connects[135, 134] = 1;
        connects[132, 133] = 1;
        connects[134, 4] = 1;
        connects[133, 131] = 1;
        connects[131, 27] = 1;
        connects[116, 117] = 1;
        connects[117, 118] = 1;
        connects[118, 124] = 1;
        connects[124, 121] = 1;
        connects[118, 119] = 1;
        connects[119, 120] = 1;
        connects[120, 122] = 1;
        connects[121, 125] = 1;
        connects[122, 123] = 1;
        connects[125, 126] = 1;
        connects[123, 127] = 1;
        connects[126, 128] = 1;
        connects[128, 130] = 1;
        connects[130, 29] = 1;
        connects[127, 76] = 1;
        connects[77, 116] = 1;
        connects[28, 74] = 1;
        connects[74, 3] = 1;
        connects[82, 74] = 1;
        connects[101, 98] = 1;
        connects[39, 137] = 1;
        connects[39, 16] = 1;
        connects[138, 115] = 1;
        connects[138, 114] = 1;
        connects[92, 103] = 1;
        connects[93, 100] = 1;
        connects[94, 14] = 1;
    }

    public void OnDrawGizmos()
    {
        DrawCurved();
    }

    /// <summary>
    /// Unity-Gizmos中图连接可视化
    /// </summary>
    public void DrawCurved()
    {
        //初始化变量
        pnum = 0;
        pPosition.Clear();
        connects = null;

        //遍历所有point
        foreach(Transform pt in pointSet.transform)
        {
            pPosition.Add(pt.position);
        }

        //connect矩阵维护
        pnum = pPosition.Count;
        if(pnum<2)
        {
            return;
        }
        connects = new int[pnum + 1, pnum + 1];
        for (int i = 0; i <= pnum; i++)
            for (int j = 0; j <= pnum; j++)
                connects[i, j] = 0;
        InitConnect();
        for (int i = 1; i <= pnum; i++)
            for (int j = 1; j <= pnum; j++)
                if (connects[i, j] == 1)
                    connects[j, i] = 1;

        //画线
        Gizmos.color = Color.green;
        for(int i=1;i<=pnum;i++)
        {
            for (int j = 1; j <= i;j++)
            {
                if(connects[i,j]==1)
                    Gizmos.DrawLine(pPosition[i-1],pPosition[j-1]);
            }
        }
    }
}
