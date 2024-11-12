using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MapManager
{
    [SerializeField]
    private LevelPrefab startLevelprefab;

    [SerializeField]
    private Tilemap mapGrid, levelCollidersGrid, levelBackGround;

    [SerializeField]
    private List<Tile> tilesByBiomes;

    [SerializeField]
    private float tileSide = 0.25f;

    [SerializeField]
    private int width = 10, height = 10;

    [SerializeField]
    private int levelToPlayerWidth = 100;

    private int currentLevelX, currentLevelY;
    private int levelWidth, levelHeight;
    private List<List<LevelMap>> levels;
    private int biomeAmount = Enum.GetValues(typeof(Biome)).Length;
    private LevelMap levelMap;

    public void VisualizeMap()
    {
        if (mapGrid == null || tilesByBiomes.Count < biomeAmount) return;
        mapGrid.gameObject.SetActive(!mapGrid.gameObject.activeSelf);
        mapGrid.size = new Vector3Int(width, height, 1);
        mapGrid.ResizeBounds();
        for(int i=0; i< width; i++)
            for(int j=0; j<height; j++)
                mapGrid.SetTile(new Vector3Int (i,j,1), tilesByBiomes[(int)levels[i][j].Biome]);
        mapGrid.transform.position = new Vector3(-tileSide * width / 2, tileSide * height / 2);
    }

    public void GenerateMap(int playerWidth)
    {
        levelWidth = levelToPlayerWidth * playerWidth;
        levelHeight = levelWidth;
        GenerateBiomes();
        GenerateEmptyMap();
        levels[0][width / 2].InstantiateLevel(levelCollidersGrid, levelBackGround);
        //levelMap = new LevelMap(Biome.Cave, levelWidth, levelHeight, new Vector2Int(levelWidth/2, 0));
        //levelMap.GenerateLevel();
        //levelMap.DigCave(new Vector2Int(LevelWidth / 2, LevelHeight / 2), 100, 50, 100);
        //levelMap.InsertRandomDirectionTunnel(new Vector2Int(LevelWidth/2, 0), new Vector2Int(LevelWidth/2, LevelHeight), TunnelWidth, AmountOfTurns);
    }

    private void GenerateEmptyMap()
    {
        levels = new List<List<LevelMap>>();
        levels.Add(new List<LevelMap>());
        for (int j = 0; j < width; j++)
        {
            levels[0].Add(new LevelMap(Biome.None, levelWidth, levelHeight, new Vector2Int(0, 0)));
        }
        levels[0][width / 2].GenerateLevelFromPrefab(startLevelprefab);
        for (int i = 1; i < height; i++)
        {
            levels.Add(new List<LevelMap>());
            for (int j = 0; j < width; j++)
            {
                levels[i].Add(new LevelMap(Biome.Cave, levelWidth, levelHeight, levels[i-1][j].Exit));
            }
        }
    }

    private void GenerateBiomes()
    {

    }

    public void VisualizeLevelMap()
    {
        if (mapGrid == null || tilesByBiomes.Count < biomeAmount) return;
        mapGrid.gameObject.SetActive(!mapGrid.gameObject.activeSelf);
        mapGrid.size = new Vector3Int(levelMap.Width, levelMap.Height, 1);
        mapGrid.ResizeBounds();
        for(int i=0; i < levelMap.Width; i++)
        for (int j = 0; j < levelMap.Height; j++)
        {
            if(levelMap.Tiles[i][j] != TileType.Empty)
                mapGrid.SetTile(new Vector3Int (i,j,1), tilesByBiomes[(int)Biome.Cave]);
            else
                mapGrid.SetTile(new Vector3Int (i,j,1), tilesByBiomes[(int)Biome.None]);
        }
        mapGrid.transform.position = new Vector3(-tileSide * levelMap.Width / 2, tileSide * levelMap.Height / 2);
    }

    public void LoadLevel(int x, int y)
    {
        currentLevelX = x;
        currentLevelY = y;
        levels[y][x].InstantiateLevel(levelCollidersGrid, levelBackGround);
    }
    public void LoadLevel(Vector2 direction)
    {
        if(direction.x + currentLevelX > -1 &&
           direction.x + currentLevelX < levelWidth &&
           currentLevelY - direction.y > -1 &&
           currentLevelY - direction.y < levelHeight)
        levels[currentLevelY - (int)direction.y][currentLevelX + (int)direction.x].InstantiateLevel(levelCollidersGrid, levelBackGround);
    }

    public void TransferTo(Vector2 direction, GameObject someObject)
    {

    }
}
