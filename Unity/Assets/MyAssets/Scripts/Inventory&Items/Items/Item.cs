using System;
using MyAssets.Scripts.Inventory_Items;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    private BasicMovement player;

    [SerializeField]
    private ItemType type;

    [SerializeField]
    private int itemIndex; //must be equal to index in WorldManager.GiantItemList

    [SerializeField]
    private int maxStack;

    [SerializeField]
    private int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<BasicMovement>();
        if (player != null)
        {
            player.IncludePickable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<BasicMovement>();
        if (player != null)
        {
            player.ExcludePickable(this);
        }
    }

    public virtual void Act()
    {
        //todo: item must have an Action method, which controls how it is used

    }

    public int Index => itemIndex;
    public ItemType itemType => type;
    public int MaxStack => maxStack;

    public int Amount
    {
        get => amount;
        set => amount = value;
    }
}
