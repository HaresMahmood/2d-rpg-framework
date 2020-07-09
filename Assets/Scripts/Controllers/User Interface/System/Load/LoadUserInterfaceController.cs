using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class LoadUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static LoadUserInterfaceController instance;

    [SerializeField] private LoadUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static LoadUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadUserInterfaceController>();
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
        if (condition)
        {
            UpdateSelectedObject(selectedValue, 0);
            selectedValue = 0;
            UpdateSelectedObject(selectedValue, 1);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        Flags.isActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        Debug.Log(true);

        base.GetInput(axisName);

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle.");
        }

        if (Input.GetButtonDown("Interact"))
        {
            Flags.isActive = false;
            //userInterface.ActivateCategory(selectedValue);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(SetActive(false));
            StartCoroutine(GetComponent<SystemUserInterfaceController>().SetActive(true, false));
        }
    }

    #endregion
}

