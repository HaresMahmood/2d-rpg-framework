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

        slots[selectedValue].AnimateSlot(0.5f, 0.1f);
        slots[previousValue].AnimateSlot(0.25f, 0.1f);

        StartCoroutine(UpdateSelector(slots[selectedValue].transform));
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
            slots[i].UpdateInformation(party[i]);
            counter = i;
        }

        if (++counter < slots.Count)
        {
            //Debug.Log(true);
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

