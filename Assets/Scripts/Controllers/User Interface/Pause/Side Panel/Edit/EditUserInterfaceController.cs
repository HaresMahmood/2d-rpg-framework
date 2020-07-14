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
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interact");
        }

        if (Input.GetButtonDown("Cancel"))
        {
            userInterface.ActivateMenu(false);
            StartCoroutine(SidebarUserInterfaceController.Instance.SetActive(true));
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

