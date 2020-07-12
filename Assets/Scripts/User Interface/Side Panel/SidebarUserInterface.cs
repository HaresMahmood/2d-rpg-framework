using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SidebarUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => Party.Count;

    #endregion

    #region Properties

    public List<PartyMember> Party { get; set; }

    #endregion

    #region Variables

    List<SidebarSlot> slots;

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        slots = GetComponentsInChildren<SidebarSlot>().ToList();   
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

        if (counter < slots.Count - 1)
        {
            for (int i = counter; i < slots.Count; i++)
            {
                slots[i].DeactivateSlot();
            }
        }
    }

    #endregion
}

