//TODO: Merge with ItemSelection.cs!

using UnityEngine;

public class ChoiceSelection : MonoBehaviour
{
    public int buttonIndex;

    // Update is called once per frame
    void Update()
    {
        if (BranchingDialogManager.instance.buttonIndex == buttonIndex)
            BranchingDialogManager.instance.selectedButton = buttonIndex;
    }
}
