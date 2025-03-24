using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelMap
{
    private Biome biome;
    private static int width, height, halfWidth;
    private List<List<Biome>> backgroundTiles;
    private List<List<TileShape>> tiles;
    private List<Tunnel> tunnels = new List<Tunnel>();
    private List<Cave> caves = new List<Cave>();
    private Vector2Int mainEntrance, mainExit;
    private LevelPrefab prefab;

    public LevelMap(Biome Biome, Vector2Int MainEntrance)
    {
        biome = Biome;
        mainEntrance = MainEntrance;
        GenerateEmptyLevel();
        if(biome == Biome.Cave)
            GenerateLevel();
    }

    public void GenerateLevel()
    {
        //todo: maybe make it into abstract method and make it different for every biome?
        GenerateCaveSystem(WorldManager.Instance.PlayerTileSize);
        SmoothenAllEdges();
    }

    private void GenerateEmptyLevel()
    {
        backgroundTiles = new List<List<Biome>>();
        tiles = new List<List<TileShape>>();
        for (int i = 0; i < height; i++)
        {
            backgroundTiles.Add(new List<Biome>());
            tiles.Add(new List<TileShape>());
            for (int j = 0; j < width; j++)
            {
                backgroundTiles[i].Add(biome);
                tiles[i].Add(TileShape.AllNeighboured);
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
                tiles[y][x] = TileShape.Empty;
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

    public static void SetSize(int Width, int Height)
    {
        width = Width;
        halfWidth = width / 2;
        height = Height;
    }

    public void LoadEdge(Vector2Int direction, Tilemap collisions, Tilemap background)
    {
        Vector2Int startPoint = CalcStartPoint(direction);
        Vector2Int endPoint = CalcEndPoint(direction);
        Vector2Int offset = new Vector2Int(-direction.x * width, -direction.y * height);
        Vector3Int position;
        var tileprefabs = WorldManager.Instance.TileDictionary[biome].tiles;
        var bgTile = WorldManager.Instance.Backgrounds[biome];
        int levelX = startPoint.x + offset.x;
        for (int tileMapX = startPoint.x; tileMapX < endPoint.x; tileMapX++, levelX++)
        {
            int levelY = startPoint.y + offset.y;
            for (int tileMapY = startPoint.y; tileMapY < endPoint.y; tileMapY++, levelY++)
            {
                position = new Vector3Int(tileMapY, width - tileMapX, 0);
                collisions.SetTile(position, tileprefabs[tiles[levelX][levelY]]);
                background.SetTile(position, bgTile);
            }
        }
    }

    public static void LoadStoneEdge(Vector2Int direction, Tilemap collisions, Tilemap background)
    {
        Vector2Int startPoint = CalcStartPoint(direction);
        Vector2Int endPoint = CalcEndPoint(direction);
        Vector3Int position;
        var unbreakable = WorldManager.Instance.TileDictionary[Biome.Cave].tiles[TileShape.Unbreakable];
        var bgTile = WorldManager.Instance.Backgrounds[Biome.Cave];
        for (int x = startPoint.x; x < endPoint.x; x++)
        for (int y = startPoint.y; y < endPoint.y; y++)
        {
            position = new Vector3Int(y, width - x, 0);
            collisions.SetTile(position, unbreakable);
            background.SetTile(position, bgTile);
        }
    }

    private static Vector2Int CalcStartPoint(Vector2Int direction)
    {
        // x: -PY -> 0| 0->W | W -> W + PY
        // y: -PY -> 0| 0->H | H -> H + PY
        return new Vector2Int(direction.x == -1 ? -WorldManager.Instance.PlayerTileSize.y : 0 +
            direction.x > 0 ? width : 0,
            direction.y == -1 ? -WorldManager.Instance.PlayerTileSize.y : 0 +
            direction.y > 0 ? height : 0);
    }

    private static Vector2Int CalcEndPoint(Vector2Int direction)
    {
        int endX = direction.x == 1 ? WorldManager.Instance.PlayerTileSize.y : 0;
        endX += direction.x > -1 ? width : 0;
        int endY = direction.y == 1 ? WorldManager.Instance.PlayerTileSize.y : 0;
        endY += direction.y > -1 ? height : 0;
        return new Vector2Int(endX, endY);
    }

    private void SmoothenAllEdges()
    {
        for(int x=1; x < width-1; x++)
            for(int y=1; y < height-1; y++)
                if (tiles[y][x] != TileShape.Empty)
                    SmoothenTileEdges(x, y);
    }

    private void SmoothenTileEdges(int x, int y)
    {
        if(tiles[y+1][x] == TileShape.Empty)
            if(tiles[y][x+1] == TileShape.Empty)
                if(tiles[y-1][x] == TileShape.Empty)
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.NotNeighboured;
                    else
                        tiles[y][x] = TileShape.LeftNeighboured;
                else
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.TopNeighboured;
                    else
                        tiles[y][x] = TileShape.LeftTopNeighboured;
            else
                if(tiles[y-1][x] == TileShape.Empty)
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.RightNeighboured;
                    else
                        tiles[y][x] = TileShape.HorizontalNeighboured;
                else
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.TopRightNeighboured;
                    else
                        tiles[y][x] = TileShape.RightTopLeftNeighboured;
        else
            if(tiles[y][x+1] == TileShape.Empty)
                if(tiles[y-1][x] == TileShape.Empty)
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.BotNeighboured;
                    else
                        tiles[y][x] = TileShape.BotLeftNeighboured;
                else
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.VerticalNeighboured;
                    else
                        tiles[y][x] = TileShape.TopLeftBotNeighboured;
            else
                if(tiles[y-1][x] == TileShape.Empty)
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.RightBotNeighboured;
                    else
                        tiles[y][x] = TileShape.LeftBotRightNeighboured;
                else
                    if (tiles[y][x - 1] == TileShape.Empty)
                        tiles[y][x] = TileShape.BotRightTopNeighboured;
                    else
                        tiles[y][x] = TileShape.AllNeighboured;
    }

    public void OverrideMainExit(Vector2Int newMainExit)
    {
        mainExit = newMainExit;
    }

    public int Width => prefab == null ? width : prefab.Size.x;
    public int Height => prefab == null ? height : prefab.Size.y;

    public Vector2Int Size => prefab == null ? new Vector2Int(width, height) : prefab.Size;
    public Biome Biome => biome;
    public Vector2Int Entrance => mainEntrance;
    public Vector2Int Exit => mainExit;
    public List<List<TileShape>> Tiles => tiles;
}
