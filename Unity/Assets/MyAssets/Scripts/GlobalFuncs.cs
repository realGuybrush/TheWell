using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFuncs
{
    private static Camera camera;

    public static void InitCamera()
    {
        //should be called by WorldManager at Awake
        camera = Camera.main;
    }

    ///////MATH STUFF///////
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
        return Mathf.Sqrt((pos1.x - pos2.x) * (pos1.x - pos2.x) +
                          (pos1.y - pos2.y) * (pos1.y - pos2.y) +
                          (pos1.z - pos2.z) * (pos1.z - pos2.z));
    }

    ///////COLOR STUFF///////
    public static void SetTransparency(GameObject obj, float transp)
    {
        //fix: perhaps use SpriteRenderer as an input? Check, where you use it anyway
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, transp);
        }
    }
    public static void SetColor(GameObject obj, float r, float g, float b)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(r, g, b, c.a);
        }
    }
    public static void SetColor(GameObject obj, Color c)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float a = spriteRenderer.color.a;
            spriteRenderer.color = new Color(c.r, c.g, c.b, a);
        }
    }

    ///////MOUSE STUFF///////

    public static Vector2 GetMousePosition()
    {
        if (camera == null)
            return new Vector2();
        Vector2 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        return pos;
    }

    public static Vector2 GetMousePositionInRange(float range, Vector2 center)
    {
        if (camera == null)
            return new Vector2();
        Vector2 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Distance(center, pos);
        if (distance < range)
            return pos;
        Vector2 newPos = new Vector2(center.x - (center.x - pos.x) / (distance / range), center.y - (center.y - pos.y) / (distance / range));
        return newPos;
    }

    public static float GetMouseAngleRespToCenter(Vector2 center)
    {
        if (camera == null)
            return 0;
        Vector2 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Distance(center, pos);
        float lowerPart = (pos.y - center.y) < 0.0f?1.0f:0.0f;
        float arccos = Mathf.Rad2Deg* Mathf.Acos((pos.x - center.x) / distance);
        return (pos.y - center.y) < 0.0f ? 180.0f + (180.0f - arccos):arccos;
    }

    public static float GetMouseAngleWithFlip(Vector2 center)
    {
        float returnAngle = GetMouseAngleRespToCenter(center);
        //if (!facingRight)
        //    returnAngle = 180.0f - returnAngle;
        if (returnAngle < 0)
            returnAngle += 360.0f;
        return returnAngle;
    }
}
