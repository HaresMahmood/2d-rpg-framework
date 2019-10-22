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
        InventoryManager.instance.inventory.items.Remove(item);
        InventoryManager.instance.UpdateInventory();
    }

    public void GiveItem(Item item)
    {
        InventoryManager.instance.GiveItem(item);
    }

    public void MakeFavorite(Item item)
    {
        item.isFavorite = !item.isFavorite;
        //InventoryManager.instance.selectedItem = 0; InventoryManager.instance.itemIndex = 0;
        InventoryManager.instance.currentItem = item;
        InventoryManager.instance.UpdateInventory();
    }

    public void CancelMenu()
    {
        Debug.Log("Destroying menu.");
    }
}
