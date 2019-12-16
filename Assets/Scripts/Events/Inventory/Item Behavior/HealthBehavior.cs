using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
public class HealthBehavior : MonoBehaviour
{
    public void AddHealth(Item item)
    {
        if (item.ID == 1)
            Debug.Log("Added " + 50 + "HP.");
        else if (item.ID == 2)
            Debug.Log("Added " + 1000 + "HP.");
        else
            Debug.Log("Added " + 100 + "HP.");

        if (item.amount > 1)
            item.amount--;
        else
            InventoryManager.instance.inventory.items.Remove(item);
    }
}
