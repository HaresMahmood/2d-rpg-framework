using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DefaultBehavior : MonoBehaviour
{
    public void DiscardItem(Item item)
    {
        //InventoryManager.instance.isDiscardingItem = true;
    }

    public void GiveItem(Item item)
    {
        //InventoryManager.instance.GiveItem(item);
    }

    public void MakeFavorite(Item item)
    {
        item.isFavorite = !item.isFavorite;
        //InventoryManager.instance.UpdateInventory();
        //InventoryManager.instance.currentItem = item;
    }

    public void CancelMenu()
    {
        //Debug.Log("Destroying menu.");
    }
}
