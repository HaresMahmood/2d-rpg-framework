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
        PauseUserInterfaceController.Instance.ActivateMenu(isActive ? 0.5f : 1f);
        userInterface.FadePanel(isActive ? 1f : 0.5f);

        Flags.isActive = isActive;

        yield break;
    }

    public bool ActivateMenu(bool isActive, int selectedValue = -1)
    {
        return userInterface.ActivateMenu(isActive, selectedValue);
    }

    protected override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        UserInterface.UpdateSelectedObject(selectedValue, increment);

        selectedValue = Mathf.Clamp(selectedValue, 0, party.Count - 1);
        PartyController.Instance.UpdateSelectedObject(selectedValue);
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Interact"))
        {
            Flags.isActive = ActivateMenu(true, selectedValue);
        }

        if (Input.GetButtonDown("Cancel") || Input.GetAxisRaw("Horizontal") == 1)
        {
            StartCoroutine(SetActive(false));
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

