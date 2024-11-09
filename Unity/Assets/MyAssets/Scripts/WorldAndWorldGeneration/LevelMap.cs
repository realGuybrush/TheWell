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
    public void GenerateLevel(Biome Biome, int Width, int Height)
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

    public void InsertTunnel(Vector2Int start, Vector2Int end, int tunnelWidth, int turns)
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
        float currentStepX, currentStepY;
        float stepX = (end.x-start.x) / distance;
        float stepY = (end.y-start.y) / distance;
        int halfWidth = tunnelWidth / 2;
        for (int i = 0; i < distance; i++)
        {
            for(int digX = -halfWidth; digX < halfWidth; digX++)
                for(int digY = -halfWidth; digY < halfWidth; digY++)
                {
                    currentStepX = i * stepX;
                    indexX = (int)(start.x + currentStepX + digX);
                    currentStepY = i * stepY;
                    indexY = (int)(start.y + currentStepY + digY);
                    if (GlobalFuncs.Distance2D(new Vector2Int(indexX, indexY),
                            new Vector2(start.x + currentStepX, start.y + currentStepY)) < halfWidth)
                        if(indexY >= 0 && indexY < height && indexX >= 0 && indexX < width)
                            tiles[indexY][indexX] = TileType.Empty;
                }
        }
    }

    public int Width => width;
    public int Height => height;
    public List<List<TileType>> Tiles => tiles;
}
