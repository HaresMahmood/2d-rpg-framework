using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Overworld Item", menuName = "Inventory/Overweorld Item")]
public class OverworldItem : ScriptableObject
{
    public Item item;
    public bool isPickedUp;
}