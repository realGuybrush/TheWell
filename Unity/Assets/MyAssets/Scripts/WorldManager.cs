using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField]
    private ControlKeys controlKeys = new ControlKeys();

    [SerializeField]
    private List<Item> GiantItemList = new List<Item>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        GlobalFuncs.InitCamera();
    }

    public Item GetItemByHash(int hash)
    {
        if (hash == -1) return null;
        return Instantiate(GiantItemList[hash]);
    }

    public ControlKeys AllControlKeys => controlKeys;
}
