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
        //todo: remove after finish, or change to some sort of player's map
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
        GenerateSurfaceRow();
        for (int i = 1; i < height; i++)
        {
            levels.Add(new List<LevelMap>());
            for (int j = 0; j < width; j++)
            {
                levels[i].Add(new LevelMap(Biome.Cave, levelWidth, levelHeight, OppositeOf(levels[i-1][j].Exit)));
            }
        }
    }

    private void GenerateSurfaceRow()
    {
        for (int j = 0; j < width; j++)
            levels[0].Add(new LevelMap(Biome.None, levelWidth, levelHeight, Vector2Int.zero));
        levels[0][width / 2].GenerateLevelFromPrefab(startLevelprefab);
        levels[0][width / 2].OverrideMainExit(new Vector2Int(levelWidth / 2, levelHeight - 1));
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

    public void LoadLevel(int x, int y)
    {
        currentLevelX = x;
        currentLevelY = y;
        levels[y][x].InstantiateLevel(levelCollidersGrid, levelBackGround);
        SetNewPortalParameters();
    }
    public void LoadLevel(Vector2Int direction)
    {
        if (direction.x + currentLevelX > -1 && direction.x + currentLevelX < levelWidth &&
            currentLevelY - direction.y > -1 && currentLevelY - direction.y < levelHeight)
        {
            levels[currentLevelY - direction.y][currentLevelX + direction.x].InstantiateLevel(levelCollidersGrid, levelBackGround);
            SetNewPortalParameters();
            currentLevelY -= direction.y;
            currentLevelX += direction.x;
        }
    }

    private void SetNewPortalParameters()
    {
        SetAllPortalsScaleAndPosition(levelCollidersGrid.localBounds.size, levelCollidersGrid.localBounds.size / 2,
             WorldManager.Instance.PlayerActualSize);
    }

    private void SetAllPortalsScaleAndPosition(Vector3 gridSize, Vector3 halfGridSize, Vector2 playerSize)
    {
        Vector2 verticalPortalsSize = new Vector2(playerSize.x, gridSize.y);
        Vector2 horizontalPortalsSize = new Vector2(gridSize.x, playerSize.y);
        downPortal.SetScaleAndPosition(new Vector2(halfGridSize.x, -playerSize.y), horizontalPortalsSize);
        leftPortal.SetScaleAndPosition(new Vector2(-playerSize.x, halfGridSize.y), verticalPortalsSize);
        topPortal.SetScaleAndPosition(new Vector2(halfGridSize.x, gridSize.y + playerSize.y), horizontalPortalsSize);
        rightPortal.SetScaleAndPosition(new Vector2(gridSize.x + playerSize.x, halfGridSize.y), verticalPortalsSize);
    }

    public void TransferToOtherLevelEntrance(Vector2 direction, GameObject someObject)
    {
        if (direction.x + currentLevelX > -1 && direction.x + currentLevelX < levelWidth &&
            currentLevelY - direction.y > -1 && currentLevelY - direction.y < levelHeight)
        {
            Vector2 playerTileSize = WorldManager.Instance.PlayerTileSize;
            Vector2 entrance = levels[currentLevelY - (int) direction.y][currentLevelX + (int) direction.x].Entrance;
            Vector3 size = levelCollidersGrid.cellSize;
            someObject.transform.position = new Vector3((entrance.x + direction.x * playerTileSize.x) * size.x,
                                                        (levelHeight - entrance.y + direction.y * playerTileSize.y) * size.y, 0);
            var body = someObject.GetComponent<Rigidbody2D>();
            if(body != null)
                body.velocity = Vector2.zero;
            //todo: if not player, delete object and add it in list of objects in other level;
        }
    }
}
