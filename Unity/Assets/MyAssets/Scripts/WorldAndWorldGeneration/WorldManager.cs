using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using RotaryHeart.Lib.SerializableDictionary;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    private readonly int playerTileWidth = 2;
    private readonly int playerTileHeight = 6;
    private readonly int halfPlayerTileWidth = 1;
    private readonly int halfPlayerTileHeight = 3;
    private float playerActualWidth = 0.5f;
    private float playerActualHeight = 1.5f;

    [SerializeField]
    private ControlKeys controlKeys = new ControlKeys();

    [SerializeField]
    private List<Item> GiantItemList = new List<Item>();

    [SerializeField]
    private SerializableDictionaryBase<Biome, TileDictionary> tileDictionary;

    [SerializeField]
    private SerializableDictionaryBase<Biome, Tile> backgrounds;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        GlobalFuncs.InitCamera();//todo: move call to NewGame in MainMenu, when it's added.
    }

    public void SetActualPlayerSize(float newTileSide)
    {
        playerActualWidth = newTileSide * playerTileWidth;
        playerActualHeight = newTileSide * playerTileHeight;
    }

    public Item GetItemByIndex(int hash)
    {
        if (hash == -1) return null;
        return Instantiate(GiantItemList[hash]);
    }

    public ControlKeys AllControlKeys => controlKeys;

    public SerializableDictionaryBase<Biome, TileDictionary> TileDictionary => tileDictionary;

    public SerializableDictionaryBase<Biome, Tile> Backgrounds => backgrounds;

    public int PlayerTileWidth => playerTileWidth;
    public int PlayerTileHeight => playerTileHeight;
    public int HalfPlayerTileWidth => halfPlayerTileWidth;
    public int HalfPlayerTileHeight => halfPlayerTileHeight;
    public float PlayerActualWidth => playerActualWidth;
    public float PlayerActualHeight => playerActualHeight;
}
