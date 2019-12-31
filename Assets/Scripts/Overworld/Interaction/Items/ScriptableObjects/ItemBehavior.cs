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
    public Sprite iconSprite;
    public UnityEvent behaviorEvent = new UnityEvent();

    public ItemBehavior(string buttonName, Sprite iconSprite)
    {
        this.buttonName = buttonName;
        this.iconSprite = iconSprite;
    }
}