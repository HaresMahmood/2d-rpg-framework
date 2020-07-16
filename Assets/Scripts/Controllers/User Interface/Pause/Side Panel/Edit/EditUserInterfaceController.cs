using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class EditUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static EditUserInterfaceController instance;
    [SerializeField] private EditUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static EditUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EditUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    // TODO: bad name
    public bool IsActive { get; set; }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        userInterface.UpdateSelectedObject(selectedValue, isActive ? -1 : 0);

        if (!isActive)
        {
            selectedValue = 0;
        }

        yield return null;

        Flags.isActive = isActive;

        StartCoroutine(base.SetActive(isActive, condition));
    }

    protected override void GetInput(string axisName)
    {
        if (IsActive)
        {
            GetInput();
        }
        else
        {
            base.GetInput(axisName);
        }

        if (Input.GetButtonDown("Interact"))
        {
            IsActive = !IsActive;
        }
        else if (Input.GetButtonDown("Cancel") && IsActive)
        {
            IsActive = false;
        }
        else if (Input.GetButtonDown("Cancel") && !IsActive)
        {
            userInterface.ActivateMenu(false);
            StartCoroutine(SidebarUserInterfaceController.Instance.SetActive(true));
        }

        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))
        {
            userInterface.UpdateSelector(IsActive);
        }
    }

    private void GetInput()
    {
        bool hasInput = RegularInput(UserInterface.MaxObjects, "Horizontal");
        if (hasInput)
        {
            PartyController.Instance.party.playerParty = userInterface.UpdatePosition(selectedValue, (int)Input.GetAxisRaw("Horizontal"));
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Flags.isActive)
        {
            GetInput("Horizontal");
        }
    }

    #endregion
}

