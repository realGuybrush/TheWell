using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MapManager: MonoBehaviour
{
    public static MapManager Instance;

    [SerializeField]
    private LevelPrefab startLevelprefab;

    [SerializeField]
    private Tilemap mapGrid, levelCollidersGrid, levelBackGround;

    [SerializeField]
    private Portal downPortal, rightPortal, topPortal, leftPortal;

    [SerializeField]
    private List<Tile> tilesByBiomes;

    [SerializeField]
    private float tileSide = 0.25f;

    [SerializeField]
    private int width = 10, height = 10;

    [SerializeField]
    private int levelSide = 500;

    private int currentLevelX, currentLevelY;
    private int levelWidth, levelHeight;
    private List<List<LevelMap>> levels;
    private int biomeAmount = Enum.GetValues(typeof(Biome)).Length;
    private LevelMap levelMap;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        WorldManager.Instance.SetActualPlayerSize(tileSide);
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            VisualizeMap();
        }
        if (Input.GetKey(KeyCode.L))
        {
            VisualizeLevelMap();
        }
    }

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

    public void GenerateMap()
    {
        levelWidth = levelSide;
        levelHeight = levelSide;
        GenerateBiomes();
        GenerateEmptyMap();
        currentLevelX = width / 2;
        currentLevelY = 0;
        levels[currentLevelY][currentLevelX].InstantiateLevel(levelCollidersGrid, levelBackGround);
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
        levels[0][width / 2].OverrideMainExit(new Vector2Int(levelWidth / 2, levelHeight - 1));
        for (int i = 1; i < height; i++)
        {
            levels.Add(new List<LevelMap>());
            for (int j = 0; j < width; j++)
            {
                levels[i].Add(new LevelMap(Biome.Cave, levelWidth, levelHeight, OppositeOf(levels[i-1][j].Exit)));
            }
        }
    }

    private Vector2Int OppositeOf(Vector2Int exit)
    {
        int newX = exit.x;
        if (newX == 0)
            newX = levelWidth - 1;
        else if (newX == levelWidth - 1)
            newX = 0;
        int newY = exit.y;
        if (newY == 0)
            newY = levelHeight - 1;
        else if (newY == levelHeight - 1)
            newY = 0;
        return new Vector2Int(newX, newY);
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
    public void LoadLevel(Vector2Int direction)
    {
        if (direction.x + currentLevelX > -1 &&
            direction.x + currentLevelX < levelWidth &&
            currentLevelY - direction.y > -1 &&
            currentLevelY - direction.y < levelHeight)
        {
            levels[currentLevelY - direction.y][currentLevelX + direction.x].InstantiateLevel(levelCollidersGrid, levelBackGround);
            SetNewPortalParameters();
            currentLevelY -= direction.y;
            currentLevelX += direction.x;
        }
    }

    private void SetNewPortalParameters()
    {
        float gridHeight = levelCollidersGrid.size.y * tileSide;
        float halfGridHeight = gridHeight / 2;
        float gridWidth = levelCollidersGrid.size.x * tileSide;
        float halfGridWidth = gridWidth / 2;
        WorldManager wm = WorldManager.Instance;
        Vector2 verticalPortalsSize = new Vector2(wm.PlayerActualWidth, gridHeight);
        Vector2 horizontalPortalsSize = new Vector2(gridWidth, wm.PlayerActualHeight);
        downPortal.SetScaleAndPosition(new Vector2(halfGridWidth, -wm.PlayerActualHeight), horizontalPortalsSize);
        leftPortal.SetScaleAndPosition(new Vector2(-wm.PlayerActualWidth, halfGridHeight), verticalPortalsSize);
        topPortal.SetScaleAndPosition(new Vector2(halfGridWidth, gridHeight + wm.PlayerActualHeight), horizontalPortalsSize);
        rightPortal.SetScaleAndPosition(new Vector2(gridWidth + wm.PlayerActualWidth, halfGridHeight), verticalPortalsSize);
    }

    public void TransferToOtherLevelEntrance(Vector2 direction, GameObject someObject)
    {
        if (direction.x + currentLevelX > -1 &&
            direction.x + currentLevelX < levelWidth &&
            currentLevelY - direction.y > -1 &&
            currentLevelY - direction.y < levelHeight)
        {
            WorldManager wm = WorldManager.Instance;
            Vector2 entrance = levels[currentLevelY - (int) direction.y][currentLevelX + (int) direction.x].Entrance;
            Vector3 size = levelCollidersGrid.cellSize;
            someObject.transform.position = new Vector3((entrance.x + direction.x * wm.PlayerTileWidth) * size.x,
                                                        (levelHeight - entrance.y + direction.y * wm.PlayerTileHeight) * size.y, 0);
            var body = someObject.GetComponent<Rigidbody2D>();
            if(body != null)
                body.velocity = Vector2.zero;
            //todo: if not player, delete object and add it in list of objects in other level;
        }
    }
}
