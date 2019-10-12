//TODO: Merge with ChoiceSelection.cs!

using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    public int slotIndex;

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.instance.slotIndex == slotIndex)
            PauseManager.instance.selectedItem = slotIndex;
    }
}
