using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

/// <summary>
///
/// </summary>
public class InventoryComponent : UserInterfaceComponent, UIButtonParentHandler
{
    #region Properties

    public Inventory Inventory { get { return inventory; } }

    #endregion

    #region Variables

    [SerializeField] private Inventory inventory;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 5f)] private float animationDuration;

    private int columns;
    private int visibleRows;

    #endregion

    #region Miscellaneous Methods

    // TODO: Make ExtensionMethod?
    public void SelectComponent(UserInterfaceSubComponent component)
    {
        int row = Mathf.FloorToInt(((components.IndexOf(component) - (components.IndexOf(component) % columns)) / (float)columns));

        StartCoroutine(GetComponentInChildren<Scrollbar>().LerpScrollbar(1f - (float)row / ((components.Count / columns) - 1), 0.1f)); // TODO: Make serializable, remove LerpScrollbar!
    }

    public void DeselectComponents(UserInterfaceSubComponent selectedComponent)
    {
        List<UserInterfaceSubComponent> components = this.components.Where(b => b != selectedComponent && b.transform.Find("Selector").gameObject.activeSelf).ToList();

        foreach (UserInterfaceSubComponent component in components)
        {
            component.transform.Find("Selector").gameObject.SetActive(false);
        }
    }



    // TODO: Debug
    public void SetInformation(List<Item> inventory)
    {
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

        
        
        transform.Find("Inventory/Content/Items/Empty").GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(inventory.Count == 0), animationDuration);
        transform.Find("Inventory/Content/Items/Grid").GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(inventory.Count != 0), animationDuration);
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        columns = GetComponentInChildren<GridLayoutGroup>().constraintCount;
        visibleRows = Mathf.RoundToInt(GetComponentInChildren<ScrollRectFaster>().GetComponent<RectTransform>().sizeDelta.y / (GetComponentInChildren<GridLayoutGroup>().spacing.y + GetComponentInChildren<GridLayoutGroup>().cellSize.y));
    }

    private void Start()
    {
        for (int i = 1; i < components.Count; i++)
        {
            components[i].transform.Find("Selector").gameObject.SetActive(false);
        }

        transform.Find("Inventory/Content/Categories").GetComponent<ButtonPromptController>().SetInformation(transform.Find("Inventory/Content/Categories").GetComponent<ButtonList>().PromptGroups);
        SelectComponent(components[0]);
    }

    #endregion
}

