using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

/// <summary>
///
/// </summary>
public class InventoryGridComponent : UserInterfaceComponent
{
    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    private int columns;
    private int visibleRows;

    #endregion

    #region Miscellaneous Methods

    // TODO: Make ExtensionMethod?
    public void SelectComponent(UserInterfaceSubComponent component)
    {
        if (GetComponentInChildren<Scrollbar>() != null)
        {
            int row = Mathf.FloorToInt(((components.IndexOf(component) - (components.IndexOf(component) % columns)) / (float)columns));

            StartCoroutine(GetComponentInChildren<Scrollbar>().LerpScrollbar(1f - (float)row / ((components.Count / columns) - 1), 0.1f)); // TODO: Make serializable, remove LerpScrollbar!
        }
    }

    public override void SetInformation<T>(T information)
    {
        List<Item> inventory = ((List<Item>)Convert.ChangeType(information, typeof(List<Item>)));

        for (int i = 0; i < components.Count; i++)
        {
            ((InventorySubComponent)components[i]).Animate(false, animationDuration);
            components[i].GetComponent<Button>().enabled = false;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            ((InventorySubComponent)components[i]).SetInformation(inventory[i]);
            ((InventorySubComponent)components[i]).Animate(true, animationDuration);
            components[i].GetComponent<Button>().enabled = true;
        }

        for (int i = (visibleRows * columns); i < components.Count; i++)
        {
            components[i].gameObject.SetActive(inventory.Count > (visibleRows * columns));
        }

        transform.Find("Empty").GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(inventory.Count == 0), animationDuration);
        transform.Find("Grid").GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(inventory.Count != 0), animationDuration);
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

