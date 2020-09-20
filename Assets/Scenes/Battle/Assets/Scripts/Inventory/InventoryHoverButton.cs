using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///
/// </summary>
public class InventoryHoverButton : HoverButton, IDeselectHandler
{
    #region Unity Methods

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        GetComponentInParent<InventoryGridComponent>().SelectComponent(GetComponent<UserInterfaceSubComponent>(), false);
    }


    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        GetComponentInParent<InventoryGridComponent>().SelectComponent(GetComponent<UserInterfaceSubComponent>(), true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Select(false);
    }

    #endregion
}

