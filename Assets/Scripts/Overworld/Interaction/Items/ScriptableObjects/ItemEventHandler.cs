using UnityEngine;
using UnityEngine.Events;

public class ItemEventHandler : MonoBehaviour
{
    [System.Serializable]
    public class ItemEvent : UnityEvent<Item> {}

    public ItemEvent eventHandler;
}