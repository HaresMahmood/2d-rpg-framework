using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//////////////////////////////////////
// TODO: Redo (with interface)
//////////////////////////////////////


/// <summary>
///
/// </summary>
[RequireComponent(typeof(InventorySubComponent))]
public class InventoryHoverButton : HoverButton, IDeselectHandler
{
    #region Miscellaneous Methods

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        GetComponentInParent<InventoryGridComponent>().SelectComponent(GetComponent<UserInterfaceSubComponent>());
    }

    #endregion

    #region Unity Methods

    public void OnDeselect(BaseEventData eventData)
    {
        Select(false);
    }

    #endregion
}

