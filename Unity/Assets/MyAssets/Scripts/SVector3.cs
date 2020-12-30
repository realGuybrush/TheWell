using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SVector3
{
    public float x;
    public float y;
    public float z;
    public SVector3()
    {
        x = 0.0f;
        y = 0.0f;
        z = 0.0f;
    }
    public SVector3(float X, float Y, float Z)
    {
        x = X;
        y = Y;
        z = Z;
    }
    public SVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public Vector3 ToV3()
    {
        return new Vector3(x, y, z);
    }
}
