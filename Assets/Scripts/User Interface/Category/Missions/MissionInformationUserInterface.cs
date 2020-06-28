﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MissionInformationUserInterface : InformationUserInterface
{
    #region Constants

    public override int MaxObjects => 3;

    #endregion

    #region Variables

    private List<MenuButton> buttons = new List<MenuButton>();

    #endregion

    #region Behavior Definitions

    public void Cancel(int selectedValue)
    {
        buttons[selectedValue].AnimateButton(false);
        UpdateSelectedObject(0, 1);
        buttons[0].AnimateButton(false);

        MissionsController.Instance.SetActive(true);
        ((MissionsUserInterface)MissionsController.Instance.UserInterface).ActivateSubMenu(0);
        StartCoroutine(MissionInformationController.Instance.SetActive(false));
    }

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        StartCoroutine(UpdateSelector(buttons[selectedValue].transform));
        buttons[selectedValue].AnimateButton(true);
        buttons[previousValue].AnimateButton(false);
    }

    public void ToggleSubMenu(Mission mission, bool isActive)
    {
        UpdateSelectedObject(0, 1);
    }

    public void InvokeBehavior(int selectedValue)
    {
        // Debug
        if (selectedValue == 0)
        {

        }
        else if (selectedValue == 1)
        {

        }
        else
        {
            Cancel(selectedValue);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        selector = transform.Find("Selector").gameObject;

        buttons = transform.GetComponentsInChildren<MenuButton>().ToList();

        base.Awake();
    }

    #endregion
}

