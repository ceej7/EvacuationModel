using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floyd
{
    public static int[,] floyd(ref int pnum, ref int[,] connects, ref List<Vector3> pPosition)
    {
        float[,] shortpath = new float[pnum + 1, pnum + 1];
        int[,] path = new int[pnum + 1, pnum + 1];
        for (int i = 1; i <= pnum; i++)
        {
            for (int j = 1; j <= pnum; j++)
            {
                shortpath[i, j] = 65536.0f;
            }
        }
        for (int i = 1; i <= pnum; i++)
        {
            for (int j = 1; j <= pnum; j++)
            {
                path[i, j] = -1;
                if (connects[i, j] == 1)
                    shortpath[i, j] = (pPosition[i-1]-pPosition[j-1]).magnitude;
            }
        }
        for (int k = 1; k <= pnum; k++)
        {
            for (int i = 1; i <= pnum; i++)
            {
                for (int j = 1; j <= pnum; j++)
                {
                    if (shortpath[i, k] != 65536.0f && shortpath[k, j] != 65536.0f
                    && shortpath[i, k] + shortpath[k, j] < shortpath[i, j])
                    {
                        shortpath[i, j] = shortpath[i, k] + shortpath[k, j];
                        path[i, j] = k;
                    }
                }
            }
        }
        return path;
    }
}
