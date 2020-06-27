using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionInformationController : UserInterfaceController
{
    #region Fields

    private static MissionInformationController instance;

    [SerializeField] private MissionInformationUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static MissionInformationController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MissionInformationController>();
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
        selectedValue = 0;

        yield return null;

        Flags.isActive = isActive;
    }

    protected override void InteractInput(int selectedValue)
    {
        //StartCoroutine(UserInterface.AnimateSelector());

        //((ItemInformationUserInterface)UserInterface).InvokeItemBehavior(selectedValue);

        Debug.Log("Enter");
    }

    protected override void CancelInput(int selectedValue)
    {
        userInterface.Cancel();
    }

    protected override void GetInput(string axisName)
    {
        bool hasInput = RegularInput(UserInterface.MaxObjects, axisName);
        if (hasInput)
        {
            UpdateSelectedObject(selectedValue, (int)Input.GetAxisRaw(axisName));
        }

        if (Input.GetButtonDown("Interact"))
        {
            InteractInput(selectedValue);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            CancelInput(selectedValue);
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

