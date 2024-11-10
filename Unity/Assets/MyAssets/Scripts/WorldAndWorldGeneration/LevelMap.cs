using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelMap
{
    private Biome biome;
    private int width, height;
    private List<List<Biome>> backgroundTiles;
    private List<List<TileType>> tiles;
    private List<Tunnel> tunnels;
    private List<Cave> caves;

    public void GenerateEmptyLevel(Biome Biome, int Width, int Height)
    {
        biome = Biome;
        width = Width;
        height = Height;
        backgroundTiles = new List<List<Biome>>();
        tiles = new List<List<TileType>>();
        for (int i = 0; i < height; i++)
        {
            backgroundTiles.Add(new List<Biome>());
            tiles.Add(new List<TileType>());
            for (int j = 0; j < width; j++)
            {
                backgroundTiles[i].Add(biome);
                tiles[i].Add(TileType.AllNeighboured);
            }
        }
    }

    public void InsertRandomDirectionTunnel(Vector2Int start, Vector2Int end, int tunnelWidth, int turns)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(start);
        for (int i = 0; i < turns; i++)
        {
            points.Add(new Vector2Int(Random.Range(0, width), Random.Range(0, height)));
            DigTunnelFromAToB(points[i], points[i + 1], tunnelWidth);
        }
        points.Add(end);
        DigTunnelFromAToB(points[points.Count-2], points[points.Count-1], tunnelWidth);
    }

    private void DigTunnelFromAToB(Vector2 start, Vector2 end, int tunnelWidth)
    {
        int distance = (int) GlobalFuncs.Distance2D(start, end);
        if (distance == 0) return;
        int indexX, indexY;
        float stepX = (end.x-start.x) / distance;
        float stepY = (end.y-start.y) / distance;
        float currentStepX = -stepX;
        float currentStepY = -stepY;
        int halfWidth = tunnelWidth / 2;
        for (int i = 0; i < distance; i++)
        {
            currentStepX += stepX;
            currentStepY += stepY;
            for(int digX = -halfWidth; digX < halfWidth; digX++)
                for(int digY = -halfWidth; digY < halfWidth; digY++)
                {
                    indexX = (int)(start.x + currentStepX + digX);
                    indexY = (int)(start.y + currentStepY + digY);
                    if (GlobalFuncs.Distance2D(new Vector2Int(indexX, indexY),
                            new Vector2(start.x + currentStepX, start.y + currentStepY)) < halfWidth)
                        if(indexY >= 0 && indexY < height && indexX >= 0 && indexX < width)
                            tiles[indexY][indexX] = TileType.Empty;
                }
        }
    }

    private void DigCaveFromTunnelExit(Vector2Int start, Vector2Int direction, int ellipseWidthHalf, int ellipseHeightHalf, int centerWidthHalf)
    {
        Vector2Int center = start;
        if(direction == Vector2Int.down)
            center += new Vector2Int (0, -ellipseHeightHalf);
        if (direction == Vector2Int.left || direction == Vector2Int.right)
            center += new Vector2Int((ellipseWidthHalf + centerWidthHalf) * direction.x, 0);
        DigCave(center, ellipseWidthHalf, ellipseHeightHalf, centerWidthHalf);
    }

    public void DigCave(Vector2Int center, int ellipseWidthHalf, int ellipseHeightHalf, int centerWidthHalf)
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


    public int Width => width;
    public int Height => height;
    public List<List<TileType>> Tiles => tiles;
}
