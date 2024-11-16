using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelMap
{
    private Biome biome;
    private int width, height;
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
        height = Height;
        mainEntrance = MainEntrance;
        GenerateEmptyLevel();
        if(biome != Biome.None)
            GenerateLevel();
    }

    public void GenerateLevel()
    {
        //todo: maybe make it into abstract method and make it different for every biome?
        GenerateCaveSystem();
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

    private void GenerateCaveSystem()
    {
        int playerHeight = WorldManager.Instance.PlayerTileHeight;
        GenerateCaves();
        foreach (var cave in caves)
        {
            GenerateRadProtrusions(cave);
            int closestIndex = FindIndexOfNearestCaveBelow(cave.Center);
            if (closestIndex != -1)
            {
                int tunnelWidth = Math.Max((cave.EllipseWidthHalf + cave.CenterWidth) / 5, playerHeight);
                DigTunnelFromAToB(cave.Center, caves[closestIndex].Center - new Vector2Int(0, tunnelWidth/2), tunnelWidth);
                cave.NextCaveIndex = closestIndex;
            }
        }
        DigHole(mainEntrance, playerHeight);
        //todo: add ladders in first and last tunnels
        int indexOfNearestCave = FindIndexOfNearestCaveBelow(mainEntrance);
        if (indexOfNearestCave != -1)
        {
            DigTunnelFromAToB(mainEntrance, caves[indexOfNearestCave].Center, playerHeight);
            Vector2 centerOfLowestCave = FindCenterOfLowestCave(indexOfNearestCave);
            mainExit = new Vector2Int((int)FindCenterOfLowestCave(indexOfNearestCave).x, height - 1);
            DigTunnelFromAToB(centerOfLowestCave, mainExit, playerHeight);
        } else
        {
            mainExit = new Vector2Int(Random.Range(0, width), height - 1);
            DigTunnelFromAToB(mainEntrance, mainExit, playerHeight);
        }
    }

    private void GenerateCaves()
    {
        WorldManager wm = WorldManager.Instance;
        for(int i = wm.PlayerTileWidth; i < width - wm.PlayerTileWidth; i++)
            for (int j = wm.PlayerTileHeight; j < height; j++)
            {
                if (Random.Range(0, 200) == 0)
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
            float distance = GlobalFuncs.Distance2D(center, cave.Center);
            if (distance < caveWidth || distance < cave.EllipseWidthHalf)
                return false;
        }
        return true;
    }

    private void GenerateRadProtrusions(Cave cave)
    {
        int amountOfProtrusions = Random.Range(0, cave.EllipseWidthHalf / 5);
        for (int j = 0; j < amountOfProtrusions; j++)
        {
            GenerateOneProtrusion(cave.EllipseWidthHalf + cave.CenterWidth / 2,
                cave.Center, cave.EllipseHeightHalf);
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

    private void DigTunnelFromAToB(Vector2 start, Vector2 end, int tunnelWidth, bool getNarrower = false)
    {
        tunnels.Add(new Tunnel(start, end, Math.Max(tunnelWidth,3), tiles, getNarrower));
    }

    private int FindIndexOfNearestCaveBelow(Vector2Int center)
    {
        int closestIndex = -1;
        float shortestDistance = width, distanceToOtherCave;
        for (int i = 0; i < caves.Count; i++)
        {
            if (caves[i].Center.y > center.y + caves[i].EllipseHeightHalf)
            {
                distanceToOtherCave = GlobalFuncs.Distance2D(caves[i].Center, center);
                if (distanceToOtherCave < shortestDistance)
                {
                    shortestDistance = distanceToOtherCave;
                    closestIndex = i;
                }
            }
        }
        return closestIndex;
    }

    private Vector2 FindCenterOfLowestCave(int index)
    {
        int nextIndex = index;
        while (caves[nextIndex].NextCaveIndex != -1)
            nextIndex = caves[nextIndex].NextCaveIndex;
        return caves[nextIndex].Center;
    }

    private void DigHole(Vector2Int center, int radius)
    {
        int newX, newY;
        for(int i= -radius; i < radius; i++)
        for (int j = -radius; j < radius; j++)
        {
            newX = i + center.x;
            newY = j + center.y;
            if (newX > -1 && newX < width && newY > -1 && newY < height)
                tiles[newY][newX] = TileType.Empty;
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
