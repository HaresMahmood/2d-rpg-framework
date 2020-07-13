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

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        userInterface.UpdateSelectedObject(selectedValue, isActive ? 1 : 0);

        Flags.isActive = isActive;

        yield break;
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Interact"))
        {
            Flags.isActive = userInterface.ActivateMenu(selectedValue, true);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Cancel");
        }
    }

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

