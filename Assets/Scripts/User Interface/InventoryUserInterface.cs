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

    private GameObject middlePanel;

    #endregion

    #region Miscellaneous Methods

    public void ActiveSubMenu(int selectedValue, float opacity = 0.5f)
    {
        if (activeCategorizables.Count > 0)
        {
            FadeUserInterface(middlePanel, opacity);
            FadeCharacterSprite(opacity);
            ((ItemInformationUserInterface)informationPanel).ToggleSubMenu((Item)activeCategorizables[selectedValue], true);
            StartCoroutine(InventoryController.Instance.SetActive(false, false));
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

        middlePanel = transform.Find("Middle").gameObject;

        emptyGrid = transform.Find("Middle/Grid/Empty Grid").gameObject;

        categorizableSlots = transform.Find("Middle/Grid/Item Grid").GetComponentsInChildren<CategorizableSlot>().ToList();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        informationPanel = transform.Find("Item Information").GetComponent<ItemInformationUserInterface>();

        base.Awake();
    }

    #endregion    
}
