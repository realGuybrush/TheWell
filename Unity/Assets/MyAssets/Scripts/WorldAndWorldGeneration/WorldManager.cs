using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using RotaryHeart.Lib.SerializableDictionary;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField]
    private ControlKeys controlKeys = new ControlKeys();

    [SerializeField]
    private List<Item> GiantItemList = new List<Item>();

    [SerializeField]
    private int playerWidth = 2;

    [SerializeField]
    private MapManager mapManager = new MapManager();

    [SerializeField]
    private SerializableDictionaryBase<Biome, TileDictionary> tileDictionary;

    [SerializeField]
    private SerializableDictionaryBase<Biome, Tile> backgrounds;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        GlobalFuncs.InitCamera();
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
        mapManager.GenerateMap(playerWidth);
    }

    public void LoadLevel(Vector2 direction)
    {
        mapManager.LoadLevel(direction);
    }

    public void TransferTo(Vector2 direction, GameObject someObject)
    {
        mapManager.TransferTo(direction, someObject);
    }

    public Item GetItemByHash(int hash)
    {
        if (hash == -1) return null;
        return Instantiate(GiantItemList[hash]);
    }

    public ControlKeys AllControlKeys => controlKeys;

    public SerializableDictionaryBase<Biome, TileDictionary> TileDictionary => tileDictionary;

    public SerializableDictionaryBase<Biome, Tile> Backgrounds => backgrounds;
}
