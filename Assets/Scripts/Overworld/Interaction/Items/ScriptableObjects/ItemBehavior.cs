using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Action")]
public class ItemBehavior
{
    public string buttonName;
    public UnityEvent behaviorEvent = new UnityEvent();

    public ItemBehavior(string buttonName)
    {
        this.buttonName = buttonName;
    }
}