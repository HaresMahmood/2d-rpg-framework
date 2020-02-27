﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class ItemInformationController : UserInterfaceController
{
    #region Fields

    private static ItemInformationController instance;

    [SerializeField] private ItemInformationUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static ItemInformationController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemInformationController>();
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

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        selectedValue = 0;

        yield return null;

        Flags.isActive = isActive;
    }

    protected override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        UserInterface.UpdateSelectedObject(selectedValue, increment);
    }

    protected override void InteractInput(int selectedValue)
    {
        //StartCoroutine(UserInterface.AnimateSelector());

        ((ItemInformationUserInterface)UserInterface).InvokeItemBehavior(selectedValue);
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
