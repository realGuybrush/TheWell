using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum AllItems { Pickaxe, Gun, Ladder, Lamp };
public partial class PlayerControls : BasicMovement
{
    float pickingDistance = 1f;
    public List<GameObject> pickableItem;
    public List<GameObject> pickableItemCursor;
    public List<bool> items;
    public GameObject Picker;
    public void MovePicker()
    {
        Picker.transform.position = GetMousePosition();
    }
    public void InitInventory()
    {
        items = new List<bool>();
        for (int i = 0; i < 4; i++)
        {
            items.Add(false);
        }
    }

    public void IncludePickable(GameObject newP)
    {
        pickableItem.Add(newP);
    }

    public void ExcludePickable(GameObject ExcP)
    {
        if (pickableItem.Contains(ExcP))
        {
            pickableItem.Remove(ExcP);
        }
    }
    public void IncludePickableCursor(GameObject newP)
    {
        if(GlobalFuncs.Distance(transform.position, newP.transform.position)<=pickingDistance)
        pickableItemCursor.Add(newP);
    }

    public void ExcludePickableCursor(GameObject ExcP)
    {
        if (pickableItemCursor.Contains(ExcP))
        {
            pickableItemCursor.Remove(ExcP);
        }
    }

    public bool PickUp(List<GameObject> item, List<GameObject> item2)
    {
        if (item.Count == 0)
            return false;
        //absolutely needed: without it item[0] will sometimes be null after picking up previous item, and won't be deleted for soe reason
        //if you simply put item.RemoveAt(0) after deletion of item, however, two items will be removed from List! magic
        while (item[0] == null)
        {
            item.RemoveAt(0);
            if (item.Count == 0)
                return false;

        }
        Item I = item[0].GetComponent<Item>();
        if (I == null)
            return false;
        items[I.number] = true;
        if (item2.Count != 0)
            if (item2.Contains(item[0]))
                item2.Remove(item[0]);
        GameObject.Destroy(item[0]);
        return true;
    }
}
