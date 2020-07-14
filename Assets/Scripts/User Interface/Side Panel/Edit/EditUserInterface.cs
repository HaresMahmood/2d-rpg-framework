using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class EditUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => Party.Count;

    #endregion

    #region Properties

    public List<PartyMember> Party { get; set; }

    #endregion

    #region Variables

    private List<FullPartySlot> slots;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        ActivateSlot(selectedValue, true);
        ActivateSlot(previousValue, false);
    }

    public void ActivateSlot(int selectedValue, bool isActive, float animationDuration = 0.1f)
    {
        slots[selectedValue].AnimateSlot(isActive ? 0.5f : 0.3f, animationDuration);

        if (isActive)
        {
            StartCoroutine(UpdateSelector(slots[selectedValue].transform));
        }
    }

    public void ActivateMenu(bool isActive)
    {
        SidebarUserInterfaceController.Instance.ActivateMenu(isActive);
    }

    public void UpdateInformation(List<PartyMember> party)
    {
        int counter = 0;

        for (int i = 0; i < party.Count; i++)
        {
            slots[i].AnimateSlot(1f);
            slots[i].UpdateInformation(party[i]);
            counter = i;
        }

        if (++counter < slots.Count)
        {
            for (int i = counter; i < slots.Count; i++)
            {
                slots[i].AnimateSlot(0f);
            }
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        slots = GetComponentsInChildren<FullPartySlot>().ToList();
        selector = transform.Find("List/Selector").gameObject;

        base.Awake();
    }

    #endregion
}

