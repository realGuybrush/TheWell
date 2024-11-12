using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.Tilemaps;

[Serializable]
public class TileDictionary
{
    public SerializableDictionaryBase<TileType, Tile> tiles;
}
