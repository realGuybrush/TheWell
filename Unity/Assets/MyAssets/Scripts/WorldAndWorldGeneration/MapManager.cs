using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    private Tilemap mapGrid;
    private List<Tile> tilesByBiomes;
    private int width = 10, height = 10;
    private float tileSide = 0.25f;
    private List<List<Biome>> levels;
    private int biomeAmount = Enum.GetValues(typeof(Biome)).Length;
    private LevelMap levelMap = new LevelMap();

    public void InitVisualisationParameters(Tilemap MapGrid, List<Tile> TilesByBiomes, float Side)
    {
        mapGrid = MapGrid;
        tileSide = Side;
        tilesByBiomes = TilesByBiomes;
    }

    public void VisualizeMap()
    {
        if (mapGrid == null || tilesByBiomes.Count < biomeAmount) return;
        mapGrid.gameObject.SetActive(!mapGrid.gameObject.activeSelf);
        mapGrid.size = new Vector3Int(width, height, 1);
        mapGrid.ResizeBounds();
        for(int i=0; i< width; i++)
            for(int j=0; j<height; j++)
                mapGrid.SetTile(new Vector3Int (i,j,1), tilesByBiomes[(int)levels[i][j]]);
        mapGrid.transform.position = new Vector3(-tileSide * width / 2, tileSide * height / 2);
    }

    public void GenerateMap(int Width = 10, int Height = 10, int LevelWidth = 500, int LevelHeight = 500, int TunnelWidth = 5, int AmountOfTurns = 0)
    {
        width = Width;
        height = Height;
        GenerateEmptyMap();
        GenerateBiomes();

        levelMap.GenerateEmptyLevel(Biome.Cave, LevelWidth, LevelHeight);
        levelMap.GenerateCaveSystem();
        //levelMap.DigCave(new Vector2Int(LevelWidth / 2, LevelHeight / 2), 100, 50, 100);
        //levelMap.InsertRandomDirectionTunnel(new Vector2Int(LevelWidth/2, 0), new Vector2Int(LevelWidth/2, LevelHeight), TunnelWidth, AmountOfTurns);
    }

    private void GenerateEmptyMap()
    {
        levels = new List<List<Biome>>();
        levels.Add(new List<Biome>());
        for (int j = 0; j < width; j++)
        {
            levels[0].Add(Biome.None);
        }
        levels[0][width / 2] = Biome.Surface;
        for (int i = 1; i < height; i++)
        {
            levels.Add(new List<Biome>());
            for (int j = 0; j < width; j++)
            {
                levels[i].Add(Biome.Cave);
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
}
