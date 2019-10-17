using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Action")]
public class ItemBehavior : ScriptableObject
{
    [System.Serializable]
    public class BehaviorData
    {
        public string menuOption;
        public UnityEvent behaviorEvent;
    }

    public List<BehaviorData> behaviorData = new List<BehaviorData>();
}
