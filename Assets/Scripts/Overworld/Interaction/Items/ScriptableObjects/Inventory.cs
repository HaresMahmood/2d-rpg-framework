using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
/// 
[CreateAssetMenu(fileName = "New inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items = new List<Item>();
}