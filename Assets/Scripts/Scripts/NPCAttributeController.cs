using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCAttributeController : MonoBehaviour {
    public static int avgAge=36;
    public static float standardDeviationAge = 14.0f;
    public static float genderRate=0.54f;
    //Rand Root
    public static System.Random ran = new System.Random();
   
    public static float floatGenerateSpeed(bool gender,int age)
    {
        float spd = 0;
        switch (gender){
            case false:
                {
                    if(age<=34)
                    {
                        spd = 13.62f / 3.6f;
                    }
                    else if(age<=39)
                    {
                        spd = 13.26f / 3.6f;
                    }
                    else if (age <= 44)
                    {
                        spd = 12.92f / 3.6f;
                    }
                    else if (age <= 49)
                    {
                        spd = 12.29f / 3.6f;
                    }
                    else if (age <= 54)
                    {
                        spd = 12.00f / 3.6f;
                    }
                    else if (age <= 59)
                    {
                        spd = 11.45f / 3.6f;
                    }
                    else if (age <= 64)
                    {
                        spd = 10.72f / 3.6f;
                    }
                    else if (age <= 69)
                    {
                        spd = 10.08f / 3.6f;
                    }
                    else if (age <=74)
                    {
                        spd = 9.5f / 3.6f;
                    }
                    else if (age <= 79)
                    {
                        spd = 9.0f / 3.6f;
                    }
                    else
                    {
                        spd = 8.54f / 3.6f;
                    }
                    break;
                }
            case true:
                {
                    if (age <= 34)
                    {
                        spd = 11.72f / 3.6f;
                    }
                    else if (age <= 39)
                    {
                        spd = 11.45f / 3.6f;
                    }
                    else if (age <= 44)
                    {
                        spd = 11.02f / 3.6f;
                    }
                    else if (age <= 49)
                    {
                        spd = 10.72f / 3.6f;
                    }
                    else if (age <= 54)
                    {
                        spd = 10.05f / 3.6f;
                    }
                    else if (age <= 59)
                    {
                        spd = 10.08f / 3.6f;
                    }
                    else if (age <= 64)
                    {
                        spd = 9.5f / 3.6f;
                    }
                    else if (age <= 69)
                    {
                        spd = 9.0f / 3.6f;
                    }
                    else if (age <= 74)
                    {
                        spd = 8.54f / 3.6f;
                    }
                    else if (age <= 79)
                    {
                        spd = 8.12f / 3.6f;
                    }
                    else
                    {
                        spd = 7.75f / 3.6f;
                    }
                    break;
                }


        }
        return spd*0.5f;
    }

    // 产生正态分布的随机数
    public static float Normal(float averageValue, float standardDeviation)
    {
        return averageValue + standardDeviation * (float)
            (
                Math.Sqrt(-2.0 * Math.Log(1.0 - NextDouble()))
                * Math.Sin((Math.PI + Math.PI) * NextDouble())
            );
    }

    //generate random number in the form of double
    private static double NextDouble()
    {
        
        return ran.NextDouble();

    }

    //Attribute distribution
    public static int GenerateAge()
    {
        return (int)Normal(avgAge, standardDeviationAge);
    }

    public static bool GenerateGender()
    {
        if (NextDouble() <= genderRate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static float GenerateSimilarity()
    {
        return (float)Normal(1, 0.15f);
    }

    public static float GenerateSensitive()
    {
        return (float)Normal(0.7f, 0.27f);
    }

}
