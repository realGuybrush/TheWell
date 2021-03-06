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

    public static float Distance(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt((pos1.x-pos2.x)* (pos1.x - pos2.x)+ (pos1.y - pos2.y) * (pos1.y - pos2.y) + (pos1.z - pos2.z) * (pos1.z - pos2.z));
    }

    public static void SetTransparency(GameObject obj, float transp)
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            Color c = obj.GetComponent<SpriteRenderer>().color;
            obj.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, transp);
        }
    }
    public static void SetColor(GameObject obj, float r, float g, float b)
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            Color c = obj.GetComponent<SpriteRenderer>().color;
            obj.GetComponent<SpriteRenderer>().color = new Color(r, g, b, c.a);
        }
    }
    public static void SetColor(GameObject obj, Color c)
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            float a = obj.GetComponent<SpriteRenderer>().color.a;
            obj.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, a);
        }
    }
}
