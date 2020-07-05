using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SettingsCategoryUserInterfaceController : UserInterfaceController
{
    #region Fields

    private SettingsCategoryUserInterface userInterface;

    #endregion

    #region Properties

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        selectedValue = isActive ? selectedValue : 0;

        if (isActive)
        {
            userInterface.UpdateSelectedObject(selectedValue, 1);
        }

        userInterface.ActivatePanel(isActive ? 1f : 0.3f);

        Flags.isActive = isActive;

        yield break;
    }

    public void ActivatePanel(float opacity, float animationDuration)
    {
        if (opacity == 0.3f)
        {
            userInterface.gameObject.SetActive(true);
        }

        if (userInterface.gameObject.activeSelf)
        {
            StartCoroutine(userInterface.FadePanel(opacity, animationDuration));
        }
    }

    protected override void GetInput(string axisName)
    {
        bool hasInput = RegularInput(UserInterface.MaxObjects, axisName);

        if (hasInput)
        {
            UpdateSelectedObject(selectedValue, (int)Input.GetAxisRaw(axisName));
        }

        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(SetActive(false));
            StartCoroutine(SettingsUserInterfaceController.Instance.SetActive(true));
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        userInterface = GetComponent<SettingsCategoryUserInterface>();
    }

    #endregion
}

