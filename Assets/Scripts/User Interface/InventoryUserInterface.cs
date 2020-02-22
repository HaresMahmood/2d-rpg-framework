﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public sealed class InventoryUserInterface : CategoryUserInterface
{
    #region Variables

    #endregion

    #region Miscellaneous Methods

    public void ActiveSubMenu(int selectedValue)
    {
        if (activeCategorizables.Count > 0)
        {
            selector.SetActive(false);
            ((ItemInformationUserInterface)informationPanel).ToggleSubMenu(activeCategorizables[selectedValue], true);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        selector = transform.Find("Indicator").gameObject;

        emptyGrid = transform.Find("Middle/Grid/Empty Grid").gameObject;

        categorizableSlots = transform.Find("Middle/Grid/Item Grid").GetComponentsInChildren<CategorizableSlot>().ToList();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        informationPanel = transform.Find("Item Information").GetComponent<ItemInformationUserInterface>();

        base.Awake();
    }

    #endregion    
}
