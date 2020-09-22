using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

/// <summary>
///
/// </summary>
public class InventoryGridComponent : UserInterfaceComponent
{
    #region Properties

    public float AnimationDuration { private get; set; }

    #endregion

    #region Variables

    private int columns;
    private int visibleRows;

    #endregion

    #region Events

    public event EventHandler<Item> OnValueChange;

    #endregion

    #region Miscellaneous Methods

    // TODO: Make ExtensionMethod?
    public void SelectComponent(UserInterfaceSubComponent component, bool scroll)
    {
        // bool isSelected = components.Where(c => c.gameObject == EventSystem.current.currentSelectedGameObject).ToArray().Length > 0;         GetComponent<CanvasGroup>().DOFade(isSelected ? 1f : 0.3f, AnimationDuration);

        if (scroll) // TODO: Debug
        {
            if (GetComponentInChildren<Scrollbar>() != null)
            {
                int row = Mathf.FloorToInt(((components.IndexOf(component) - (components.IndexOf(component) % columns)) / (float)columns));

                StartCoroutine(GetComponentInChildren<Scrollbar>().LerpScrollbar(1f - (float)row / ((components.Count / columns) - 1), 0.1f)); // TODO: Make serializable, remove LerpScrollbar!
            }
        }

        OnValueChange?.Invoke(this, ((InventorySubComponent)component).Item);
    }

    public override void SetInformation<T>(T information)
    {
        List<Item> inventory = ((List<Item>)Convert.ChangeType(information, typeof(List<Item>)));

        for (int i = (visibleRows * columns); i < components.Count; i++)
        {
            components[i].gameObject.SetActive(inventory.Count > (visibleRows * columns)); // TODO: Fix Scrollbar
        }

        for (int i = 0; i < components.Count; i++)
        {
            ((InventorySubComponent)components[i]).Item = null;
            ((InventorySubComponent)components[i]).Animate(false, AnimationDuration);
            components[i].GetComponent<Button>().enabled = false;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            components[i].SetInformation(inventory[i]);
            ((InventorySubComponent)components[i]).Animate(true, AnimationDuration);
            components[i].GetComponent<Button>().enabled = true;
        }

        if (EventSystem.current.currentSelectedGameObject != components[0].gameObject)
        {
            EventSystem.current.SetSelectedGameObject(inventory.Count == 0 ? null : components[0].gameObject);
        }

        transform.Find("Empty").GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(inventory.Count == 0), AnimationDuration);
        transform.Find("Grid").GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(inventory.Count != 0), AnimationDuration);

        OnValueChange?.Invoke(this, inventory.Count == 0 ? null : ((InventorySubComponent)components[0]).Item);
    }

    public void SetDescription(Item item)
    {
        transform.Find("Inventory/Content/Details/Information/Basic Information/Name").GetComponent<TextMeshProUGUI>().SetText(item.Name);
        transform.Find("Inventory/Content/Details/Information/Description/Value").GetComponent<TextMeshProUGUI>().SetText(item.Description);
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        columns = GetComponentInChildren<GridLayoutGroup>().constraintCount;
        visibleRows = Mathf.RoundToInt(GetComponent<ScrollRectFaster>().GetComponent<RectTransform>().sizeDelta.y / (GetComponentInChildren<GridLayoutGroup>().spacing.y + GetComponentInChildren<GridLayoutGroup>().cellSize.y));
    }

    protected override void Start()
    {
        base.Start();
    }

    #endregion
}

