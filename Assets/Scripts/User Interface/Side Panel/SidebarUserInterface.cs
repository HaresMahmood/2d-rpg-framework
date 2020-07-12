using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SidebarUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => (Party.Count); // + 1

    #endregion

    #region Properties

    public List<PartyMember> Party { get; set; }

    #endregion

    #region Variables

    List<SidebarSlot> slots;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        slots[selectedValue].AnimateSlot(true);
        slots[previousValue].AnimateSlot(false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        selector = transform.Find("Selectors").gameObject;
        slots = GetComponentsInChildren<SidebarSlot>().ToList();

        //base.Awake();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        int counter = 0;

        for (int i = 0; i < Party.Count; i++)
        {
            slots[i].UpdateInformation(Party[i]);
            counter = i;
        }

        if (++counter < slots.Count)
        {
            for (int i = counter; i < slots.Count; i++)
            {
                slots[i].DeactivateSlot();
            }
        }
    }

    #endregion
}

