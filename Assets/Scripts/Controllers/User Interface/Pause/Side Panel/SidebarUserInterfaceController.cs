using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SidebarUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static SidebarUserInterfaceController instance;
    [SerializeField] private SidebarUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static SidebarUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SidebarUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Variables

    List<PartyMember> party;

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        party = PartyController.Instance.party.playerParty;
        userInterface.Party = party;
    }

    #endregion
}

