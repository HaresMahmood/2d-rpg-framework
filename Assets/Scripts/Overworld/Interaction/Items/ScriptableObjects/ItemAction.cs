using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Action")]
public class ItemAction : ScriptableObject
{
    [System.Serializable]
    public class ActionData
    {
        public string actionOption;
        public UnityEvent actionEvent;
    }

    public List<ActionData> actionData = new List<ActionData>();
}
