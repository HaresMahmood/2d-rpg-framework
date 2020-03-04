using System.Linq;
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

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        Transform leftPanel = transform.Find("Left");

        selector = leftPanel.Find("List/Indicator").gameObject;

        scrollbar = leftPanel.Find("List/Scrollbar").GetComponent<Scrollbar>();

        emptyGrid = transform.Find("Middle/Grid/Empty Grid").gameObject;

        categorizableSlots = leftPanel.Find("List/Mission List").GetComponentsInChildren<CategorizableSlot>().ToList();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        informationPanel = transform.Find("Item Information").GetComponent<ItemInformationUserInterface>();

        base.Awake();
    }

    #endregion    
}
