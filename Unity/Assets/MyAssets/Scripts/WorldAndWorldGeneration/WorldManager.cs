using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
using RotaryHeart.Lib.SerializableDictionary;
using Debug = UnityEngine.Debug;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    private Vector2Int playerTileSize = new Vector2Int(2, 6);
    private Vector2Int halfPlayerTileSize = new Vector2Int(1, 3);
    private Vector2 playerActualSize = new Vector2(0.5f, 1.5f);

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
        playerActualSize = newTileSide * (Vector2)playerTileSize;
    }

    public Item GetItemByIndex(int hash)
    {
        if (hash == -1) return null;
        return Instantiate(GiantItemList[hash]);
    }

    public ControlKeys AllControlKeys => controlKeys;

    public SerializableDictionaryBase<Biome, TileDictionary> TileDictionary => tileDictionary;

    public SerializableDictionaryBase<Biome, Tile> Backgrounds => backgrounds;

    public Vector2Int PlayerTileSize => playerTileSize;
    public Vector2Int HalfPlayerTileSize => halfPlayerTileSize;
    public Vector2 PlayerActualSize => playerActualSize;
}
