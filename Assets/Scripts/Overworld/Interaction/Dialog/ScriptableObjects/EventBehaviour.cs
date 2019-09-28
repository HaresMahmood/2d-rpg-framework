using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Dialog/Event")]
public class EventBehaviour : ScriptableObject
{
    public void TestEvent()
    {
        Debug.Log("Test event succssful");
    }
}
