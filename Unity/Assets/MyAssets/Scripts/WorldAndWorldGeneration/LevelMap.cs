using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelMap
{
    private Biome biome;
    private int width, height;
    private List<List<Biome>> backgroundTiles;
    private List<List<TileType>> tiles;
    private List<Tunnel> tunnels = new List<Tunnel>();
    private List<Cave> caves = new List<Cave>();

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

    private void DigTunnelFromAToB(Vector2 start, Vector2 end, int tunnelWidth, bool getNarrower = false)
    {
        tunnels.Add(new Tunnel(start, end, tunnelWidth, tiles, getNarrower));
    }

    public void GenerateCaveSystem()
    {
        GenerateCaves();
        foreach (var cave in caves)
            GenerateRadProtrusions(cave);
    }

    private void GenerateCaves()
    {
        //todo: maybe include player's size somewhere? player.size.x == 2, player.size.y == 4
        for(int i = 2; i < width - 2; i++)
            for (int j = 4; j < height; j++)
            {
                if (Random.Range(0, 8000) == 0)
                {
                    GenerateCave(new Vector2Int(i, j));
                }
            }
    }

    private void GenerateCave(Vector2Int center)
    {
        int halfWidth = width / 2;
        int caveWidth = Random.Range(2, center.x < halfWidth ? center.x : width - center.x);
        int fifthOfCaveWidth = caveWidth / 5;
        int caveHeight = Random.Range(center.y < fifthOfCaveWidth ? center.y : fifthOfCaveWidth,
            center.y < caveWidth ? center.y : caveWidth);
        if (NoCavesAround(center, caveWidth))
            caves.Add(new Cave(new Vector2Int(center.x, center.y), caveWidth, caveHeight, 0, tiles));
    }

    private bool NoCavesAround(Vector2Int center, int caveWidth)
    {
        foreach (var cave in caves)
        {
            float distance = GlobalFuncs.Distance2D(center, cave.center);
            if (distance < caveWidth || distance < cave.ellipseWidthHalf)
                return false;
        }
        return true;
    }

    private void GenerateRadProtrusions(Cave cave)
    {
        int amountOfProtrusions = Random.Range(0, cave.ellipseWidthHalf / 5);
        for (int j = 0; j < amountOfProtrusions; j++)
        {
            GenerateOneProtrusion(cave.ellipseWidthHalf + cave.centerWidth / 2,
                cave.center, cave.ellipseHeightHalf);
        }
    }

    private void GenerateOneProtrusion(int actualCaveWidth, Vector2 center, int ellipseHeightHalf)
    {
        int tunnelWidth = actualCaveWidth / 10;
        Vector2 start = new Vector2(Random.Range(center.x - actualCaveWidth, center.x + actualCaveWidth), center.y);
        Vector2 end = new Vector2(start.x + Random.Range(0, tunnelWidth),
            center.y + Random.Range(-ellipseHeightHalf, ellipseHeightHalf));
        if (tunnelWidth == 0) tunnelWidth = Random.Range(1, 4);
        DigTunnelFromAToB(start, end, tunnelWidth, true);
    }


    public int Width => width;
    public int Height => height;
    public List<List<TileType>> Tiles => tiles;
}
