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
            FadeInventoryUserInterface(opacity);
            ((ItemInformationUserInterface)informationPanel).ToggleSubMenu((Item)activeCategorizables[selectedValue], true);
            StartCoroutine(InventoryController.Instance.SetActive(false, false));
        }
    }

    public void FadeInventoryUserInterface(float opacity)
    {
        FadeUserInterface(middlePanel, opacity);
        FadeCharacterSprite(opacity);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        Transform middle = transform.Find("Middle");
        middlePanel = middle.gameObject;

        selector = middle.Find("Grid/Selector").gameObject;

        scrollbar = middle.Find("Grid/Scrollbar").GetComponent<Scrollbar>();

        emptyGrid = middle.Find("Grid/Display Panels/Empty Panel").gameObject;

        categorizableSlots = middle.Find("Grid/Display Panels/Item Grid").GetComponentsInChildren<CategorizableSlot>().ToList();

        informationPanel = transform.Find("Item Information").GetComponent<ItemInformationUserInterface>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();
    }

    #endregion    
}
