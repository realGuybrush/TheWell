using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField]
    private ControlKeys controlKeys = new ControlKeys();

    [SerializeField]
    private List<Item> GiantItemList = new List<Item>();

    [SerializeField]
    private int mapWidth = 10, mapHeight = 10;

    [SerializeField]
    private int playerWidth = 2;

    [SerializeField]
    private int levelToPlayerWidth = 100;

    private int levelWidth = 500, levelHeight = 500;

    [SerializeField]
    private int tunnelWidth = 5;

    [SerializeField]
    private int amountOfTunnelTurns = 0;

    [SerializeField]
    private MapManager mapManager = new MapManager();

    [SerializeField]
    private float tileSide = 0.25f;

    [SerializeField]
    private Tilemap mapGrid;

    [SerializeField]
    private List<Tile> tilesByBiomes;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        GlobalFuncs.InitCamera();
        mapManager.InitVisualisationParameters(mapGrid, tilesByBiomes, tileSide);
        GenerateMap();//todo: move call to NewGame in MainMenu, when it's added.
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            mapManager.VisualizeMap();
        }
        if (Input.GetKey(KeyCode.L))
        {
            mapManager.VisualizeLevelMap();
        }
    }

    public void GenerateMap()
    {
        levelWidth = levelToPlayerWidth * playerWidth;
        levelHeight = levelWidth;
        mapManager.GenerateMap(mapWidth, mapHeight, levelWidth, levelHeight, tunnelWidth, amountOfTunnelTurns);
    }

    public Item GetItemByHash(int hash)
    {
        if (hash == -1) return null;
        return Instantiate(GiantItemList[hash]);
    }

    public ControlKeys AllControlKeys => controlKeys;

    public int MapWidth { set { mapWidth = value; } }
    public int MapHeight { set { mapHeight = value; } }
    public int LevelWidth { set { levelWidth = value; } }
    public int LevelHeight { set { levelHeight = value; } }
}
