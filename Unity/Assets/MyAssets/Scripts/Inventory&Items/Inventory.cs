using System;
using System.Collections.Generic;
using MyAssets.Scripts.Inventory_Items;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private List<ListInventorySlots> items;

    [SerializeField]
    private int amountOfColumns = 4, amountOfRows = 4;

    private int chosenSlotRow;
    private int chosenSlotColumn;
    private int chosenMelee = -1;
    private int chosenRanged = -1;
    private int meleeWeaponTypeIndex = (int) ItemType.meleeWeapon;
    private int rangedWeaponTypeIndex = (int) ItemType.rangedWeapon;
    //important: most of methods might be different for every game, based on how you want your inventory to work

    public void InitInventory()
    {
        items = new List<ListInventorySlots>();
        for (int i = 0; i < amountOfColumns; i++)
        {
            items.Add(new ListInventorySlots());
            for (int j = 0; j < amountOfRows; j++)
            {
                items[i].listInventorySlots.Add(new InventorySlot());
            }
        }
    }

    private void SearchForWeapons(ItemType type)
    {
        if(type == ItemType.meleeWeapon && chosenMelee == -1) SearchForMeleeWeapons();
        if(type == ItemType.rangedWeapon && chosenRanged == -1) SearchForRangedWeapons();
    }

    private void SearchForMeleeWeapons()
    {
        for (int i = 0; i < amountOfRows; i++)
        {
            if (items[meleeWeaponTypeIndex].listInventorySlots[i].itemHash != chosenMelee)
            {
                chosenMelee = i;
                break;
            }
        }
    }

    private void SearchForRangedWeapons()
    {
        for (int i = 0; i < amountOfRows; i++)
        {
            if (items[rangedWeaponTypeIndex].listInventorySlots[i].itemHash != chosenRanged)
            {
                chosenRanged = i;
                break;
            }
        }
    }

    public void SelectItem(int index)
    {
        if (index < amountOfColumns)
        {
            chosenSlotColumn = index;
            chosenSlotRow = (chosenSlotRow + 1) % amountOfRows;
        }
    }

    public Item SelectedItem => WorldManager.Instance.
        GetItemByHash(items[chosenSlotColumn].listInventorySlots[chosenSlotRow].itemHash);

    public Item SelectedMelee => chosenMelee == -1 ? null : WorldManager.Instance.
        GetItemByHash(items[meleeWeaponTypeIndex].listInventorySlots[chosenMelee].itemHash);

    public Item SelectedRanged
    {
        get
        {
            if(chosenRanged == -1)
                return null;
            Item item = WorldManager.Instance.GetItemByHash(items[rangedWeaponTypeIndex]
                    .listInventorySlots[chosenRanged].itemHash);
            item.Amount = items[rangedWeaponTypeIndex].listInventorySlots[chosenRanged].amount;
            return item;
        }
    }

    public bool TryToAddItem(Item item)
    {
        int indexByType = (int) item.itemType;
        int firstEmptySlot = -1;
        for (int i = amountOfRows - 1; i >= 0; i--)
        {
            if (items[indexByType].listInventorySlots[i].itemHash == item.Hash)
            {
                return TryStuffingItemsInSlot(i, indexByType, item);
            }
            if (items[indexByType].listInventorySlots[i].itemHash == -1)
                firstEmptySlot = i;
        }
        if (firstEmptySlot != -1)
        {
            items[indexByType].listInventorySlots[firstEmptySlot].itemHash = item.Hash;
            return TryStuffingItemsInSlot(firstEmptySlot, indexByType, item);
        }
        return false;
    }

    private bool TryStuffingItemsInSlot(int row, int column, Item item)
    {
        int newAmount = items[column].listInventorySlots[row].amount + item.Amount;
        if (newAmount > item.MaxStack)
        {
            item.Amount -= item.MaxStack - items[column].listInventorySlots[row].amount;
            items[column].listInventorySlots[row].amount = item.MaxStack;
            SearchForWeapons(item.itemType);
            return false;
        }
        items[column].listInventorySlots[row].amount = newAmount;
        SearchForWeapons(item.itemType);
        return true;
    }

    public void RemoveOneBullet()
    {
        items[(int)ItemType.rangedWeapon].listInventorySlots[chosenRanged].amount--;
        if(items[(int)ItemType.rangedWeapon].listInventorySlots[chosenRanged].amount <= 0)
            items[(int)ItemType.rangedWeapon].listInventorySlots[chosenRanged].itemHash = -1;
    }
}
