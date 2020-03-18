using System;
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

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        if ((this.selectedValue - selectedValue == 7) || (selectedValue - this.selectedValue == 7))
        {
            UpdateScrollbar(MaxObjects, selectedValue);
        }

        base.UpdateSelectedObject(selectedValue, increment);
    }

    protected override void UpdateScrollbar(int maxObjects = -1, int selectedValue = -1, float animationDuration = 0.08f)
    {
        if (maxObjects > -1)
        {
            if (scrollbar.GetComponent<CanvasGroup>().alpha != 1f)
            {
                StartCoroutine(scrollbar.gameObject.FadeOpacity(1f, animationDuration));
            }

            if (selectedValue > -1)
            {
                float totalSlots = maxObjects;
                float targetValue = (float)Math.Round(1.0f - (selectedValue / (totalSlots - 1)), 1);

                targetValue = (selectedValue < 7) ? 1 : targetValue;

                StartCoroutine(scrollbar.LerpScrollbar(targetValue, animationDuration));
            }
            else
            {
                scrollbar.value = 1;
            }
        }
        else
        {
            if (scrollbar.GetComponent<CanvasGroup>().alpha != 0f)
            {
                StartCoroutine(scrollbar.gameObject.FadeOpacity(0f, animationDuration));
            }
        }
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

        categorizablePanel = middle.Find("Grid/Display Panels/Item Grid").gameObject;

        emptyPanel = middle.Find("Grid/Display Panels/Empty Panel").gameObject;

        categorizableSlots = categorizablePanel.GetComponentsInChildren<CategorizableSlot>().ToList();

        informationPanel = transform.Find("Item Information").GetComponent<ItemInformationUserInterface>();

        //StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(InventoryController.instance.buttons));

        base.Awake();
    }

    #endregion    
}
