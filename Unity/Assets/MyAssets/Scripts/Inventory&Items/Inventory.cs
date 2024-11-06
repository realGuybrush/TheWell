using System;
using System.Collections.Generic;
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
            return false;
        }
        items[column].listInventorySlots[row].amount = newAmount;
        return true;
    }

    public void RemoveItem()
    {
        items[chosenSlotRow].listInventorySlots[chosenSlotColumn].amount--;
        if (items[chosenSlotRow].listInventorySlots[chosenSlotColumn].amount == 0)
            items[chosenSlotRow].listInventorySlots[chosenSlotColumn].itemHash = -1;
    }
}
