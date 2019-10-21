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

    public void CancelMenu()
    {
        Debug.Log("Destroying menu.");
    }
}
