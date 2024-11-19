using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelMap
{
    private Biome biome;
    private int width, height, halfWidth;
    private List<List<Biome>> backgroundTiles;
    private List<List<TileType>> tiles;
    private List<Tunnel> tunnels = new List<Tunnel>();
    private List<Cave> caves = new List<Cave>();
    private Vector2Int mainEntrance, mainExit;
    private LevelPrefab prefab;

    public LevelMap(Biome Biome, int Width, int Height, Vector2Int MainEntrance)
    {
        biome = Biome;
        width = Width;
        halfWidth = width / 2;
        height = Height;
        mainEntrance = MainEntrance;
        GenerateEmptyLevel();
        if(biome != Biome.None)
            GenerateLevel();
    }

    public void GenerateLevel()
    {
        //todo: maybe make it into abstract method and make it different for every biome?
        GenerateCaveSystem(WorldManager.Instance.PlayerTileSize);
    }

    private void GenerateEmptyLevel()
    {
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

    private void GenerateCaveSystem(Vector2Int playerTileSize)
    {
        GenerateCaves(playerTileSize);
        foreach (var cave in caves)
        {
            GenerateErosionProtrusions(cave);
            ConnectToCaveByIndex(cave, playerTileSize.y, FindIndexOfNearestCaveBelow(cave.Center));
        }
        DigHole(mainEntrance, playerTileSize.y);
        //todo: add ladders in first and last tunnels
        ConnectEntranceToExit(playerTileSize.y);
    }

    private void GenerateCaves(Vector2Int playerTileSize)
    {
        for(int i = playerTileSize.x; i < width - playerTileSize.x; i++)
            for (int j = playerTileSize.y; j < height; j++)
                if (Random.Range(0, 200) == 0)
                    GenerateCave(new Vector2Int(i, j), playerTileSize.x);
    }

    private void GenerateCave(Vector2Int center, int minWidth)
    {
        int caveWidth = Random.Range(minWidth, center.x < halfWidth ? center.x : width - center.x);
        int fifthOfCaveWidth = caveWidth / 5;
        int caveHeight = Random.Range(center.y < fifthOfCaveWidth ? center.y : fifthOfCaveWidth,
            center.y < caveWidth ? center.y : caveWidth);
        if (NoCavesAround(center, caveWidth))
            caves.Add(new Cave(center, caveWidth, caveHeight, 0, tiles));
    }

    private bool NoCavesAround(Vector2Int center, int caveWidth)
    {
        foreach (var cave in caves)
        {
            float distance = GlobalFuncs.Distance2D(center, cave.Center);
            if (distance < caveWidth || distance < cave.EllipseWidthHalf)
                return false;
        }
        return true;
    }

    private void GenerateErosionProtrusions(Cave cave)
    {
        int amountOfProtrusions = Random.Range(0, cave.EllipseWidthHalf / 5);
        for (int j = 0; j < amountOfProtrusions; j++)
            GenerateOneProtrusion(cave.EllipseWidthHalf + cave.CenterWidth / 2, cave.Center, cave.EllipseHeightHalf);
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

    private void ConnectToCaveByIndex(Cave cave, int playerHeight, int otherCaveIndex)
    {
        if (otherCaveIndex == -1) return;
        int tunnelWidth = Math.Max((cave.EllipseWidthHalf + cave.CenterWidth) / 5, playerHeight);
        DigTunnelFromAToB(cave.Center, caves[otherCaveIndex].Center - new Vector2Int(0, tunnelWidth/2), tunnelWidth);
        cave.NextCaveIndex = otherCaveIndex;
    }

    private void DigTunnelFromAToB(Vector2 start, Vector2 end, int tunnelWidth, bool getNarrower = false)
    {
        tunnels.Add(new Tunnel(start, end, Math.Max(tunnelWidth,3), tiles, getNarrower));
    }

    private int FindIndexOfNearestCaveBelow(Vector2Int center)
    {
        int closestIndex = -1;
        float shortestDistance = width;
        float distanceToOtherCave;
        for (int i = 0; i < caves.Count; i++)
            if (caves[i].Center.y > center.y + caves[i].EllipseHeightHalf)
            {
                distanceToOtherCave = GlobalFuncs.Distance2D(caves[i].Center, center);
                if (distanceToOtherCave < shortestDistance)
                {
                    shortestDistance = distanceToOtherCave;
                    closestIndex = i;
                }
            }
        return closestIndex;
    }

    private Vector2Int FindCenterOfLowestCave(int index)
    {
        int nextIndex = index;
        while (caves[nextIndex].NextCaveIndex != -1)
            nextIndex = caves[nextIndex].NextCaveIndex;
        return caves[nextIndex].Center;
    }

    private void DigHole(Vector2Int center, int radius)
    {
        int endX = center.x + radius;
        int endY = center.y + radius;
        for(int x = center.x -radius; x < endX; x++)
        for (int y = center.y -radius; y < endY; y++)
            if (x > -1 && x < width && y > -1 && y < height)
                tiles[y][x] = TileType.Empty;
    }

    private void ConnectEntranceToExit(int playerHeight)
    {
        int indexOfNearestCave = FindIndexOfNearestCaveBelow(mainEntrance);
        if (indexOfNearestCave != -1)
        {
            DigTunnelFromAToB(mainEntrance, caves[indexOfNearestCave].Center, playerHeight);
            Vector2Int centerOfLowestCave = FindCenterOfLowestCave(indexOfNearestCave);
            mainExit = new Vector2Int(centerOfLowestCave.x, height - 1);
            DigTunnelFromAToB(centerOfLowestCave, mainExit, playerHeight);
        } else
        {
            mainExit = new Vector2Int(Random.Range(0, width), height - 1);
            DigTunnelFromAToB(mainEntrance, mainExit, playerHeight);
        }
    }

    public void InstantiateLevel(Tilemap collisions, Tilemap background)
    {
        collisions.ClearAllTiles();
        background.ClearAllTiles();
        Vector3Int position;
        if (prefab != null)
        {
            for(int i=-prefab.Collisions.size.x; i< prefab.Collisions.size.x; i++)
            for( int j=-prefab.Collisions.size.y; j< prefab.Collisions.size.y; j++)
            {
                position = new Vector3Int(i, j, 0);
                collisions.SetTile(position, prefab.Collisions.GetTile(position));
                background.SetTile(position, prefab.Backgrounds.GetTile(position));
            }
        } else
        {
            WorldManager wm = WorldManager.Instance;
            for(int i=0; i< width; i++)
                for( int j=0; j< height; j++)
                {
                    position = new Vector3Int(j, width - i, 0);
                    collisions.SetTile(position, wm.TileDictionary[biome].tiles[tiles[i][j]]);
                    background.SetTile(position, wm.Backgrounds[biome]);
                }
        }
    }

    public void GenerateLevelFromPrefab(LevelPrefab Prefab)
    {
        prefab = Prefab;
        mainEntrance = prefab.Entrance;
        mainExit = prefab.Exit;
    }

    public void OverrideMainExit(Vector2Int newMainExit)
    {
        mainExit = newMainExit;
    }

    public int Width => width;
    public int Height => height;
    public Biome Biome => biome;
    public Vector2Int Entrance => mainEntrance;
    public Vector2Int Exit => mainExit;
    public List<List<TileType>> Tiles => tiles;
}
