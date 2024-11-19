using System;
using System.Collections.Generic;
using UnityEngine;

public class Cave
{
    private Vector2Int center;
    private int ellipseWidthHalf;
    private int ellipseHeightHalf;
    private int centerWidth;
    private int nextCaveIndex = -1;

    public Cave(Vector2Int Center, int EllipseWidthHalf, int EllipseHeightHalf, int CenterWidth, List<List<TileType>> tiles)
    {
        center = Center;
        ellipseWidthHalf = EllipseWidthHalf;
        ellipseHeightHalf = EllipseHeightHalf;
        centerWidth = CenterWidth;
        DigCave(center, ellipseWidthHalf, ellipseHeightHalf, centerWidth, tiles);
    }

    private void DigCave(Vector2Int center, int ellipseWidthHalf, int ellipseHeightHalf, int centerWidthHalf, List<List<TileType>> tiles)
    {
        int minY = center.y - ellipseHeightHalf;
        Vector2Int pointB = new Vector2Int(center.x - centerWidthHalf, center.y);
        for (int i = pointB.x - ellipseWidthHalf; i <= pointB.x; i++)
        {
            for(int j = center.y; j > minY; j--)
            {
                if(GlobalFuncs.Distance2D(pointB, new Vector2(i, j)) <=
                   CalculateDistanceToEllipseByX(i, pointB, ellipseWidthHalf, ellipseHeightHalf, centerWidthHalf))
                    tiles[j][i] = TileType.Empty;
            }
        }
        pointB = new Vector2Int(center.x + centerWidthHalf, center.y);
        for (int i = center.x - centerWidthHalf; i <= pointB.x; i++)
        {
            for(int j = center.y; j > minY; j--)
            {
                tiles[j][i] = TileType.Empty;
            }
        }
        for (int i = center.x + centerWidthHalf + ellipseWidthHalf; i >= pointB.x; i--)
        {
            for(int j = center.y; j > minY; j--)
            {
                if(GlobalFuncs.Distance2D(pointB, new Vector2(i, j)) <=
                   CalculateDistanceToEllipseByX(i, pointB, ellipseWidthHalf, ellipseHeightHalf, centerWidthHalf))
                    tiles[j][i] = TileType.Empty;
            }
        }
    }

    private float CalculateDistanceToEllipseByX(int X, Vector2Int center, int ellipseWidthHalf, int ellipseHeightHalf, int centerWidthHalf)
    {
        float x = center.x - X;
        int y = (int)Math.Sqrt((1 - (x * x) / (ellipseWidthHalf * ellipseWidthHalf)) * ellipseHeightHalf * ellipseHeightHalf);
        return GlobalFuncs.Distance2D(center, new Vector2(X, center.y + y));
    }

    public Vector2Int Center => center;
    public int EllipseWidthHalf => ellipseWidthHalf;
    public int EllipseHeightHalf => ellipseHeightHalf;
    public int CenterWidth => centerWidth;

    public int NextCaveIndex
    {
        set { nextCaveIndex = value;}
        get => nextCaveIndex;
    }

}
