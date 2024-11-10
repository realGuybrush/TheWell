using UnityEngine;

public struct Cave
{
    public Vector2Int center;
    public int ellipseWidth;
    public int ellipseHeight;
    public int centerWidth;

    public Cave(Vector2Int Center, int EllipseWidth, int EllipseHeight, int CenterWidth)
    {
        center = Center;
        ellipseWidth = EllipseWidth;
        ellipseHeight = EllipseHeight;
        centerWidth = CenterWidth;
    }
}
