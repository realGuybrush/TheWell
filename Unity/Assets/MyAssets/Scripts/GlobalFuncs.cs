using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFuncs
{
    public static bool AroundZero(float value)
    {
        return ((value <= 0.01f) && (value >= -0.01f));
    }
    public static int Dif(int a, int b)
    {
        return Mathf.Abs(a - b);
    }
    public static float Dif(int a, float b)
    {
        return Mathf.Abs(a - b);
    }
    public static float Dif(float a, int b)
    {
        return Mathf.Abs(a - b);
    }
    public static float Dif(float a, float b)
    {
        return Mathf.Abs(a - b);
    }
}
