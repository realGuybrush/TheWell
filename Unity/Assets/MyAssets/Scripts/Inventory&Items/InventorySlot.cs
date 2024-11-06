using System;
using System.Collections.Generic;

[Serializable]
public class InventorySlot
{
    public int itemHash = -1;
    public int amount;
}

[Serializable]
public class ListInventorySlots
{
    public List<InventorySlot> listInventorySlots = new List<InventorySlot>();
}
