using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//////////////////////////////////////
// TODO: Redo (with interface)
//////////////////////////////////////


/// <summary>
///
/// </summary>
[RequireComponent(typeof(Button))]
public class InventoryHoverButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    #region Unity Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().enabled)
        {
            UIButtonParentHandler buttonHandler = GetComponentInParent(typeof(UIButtonParentHandler)) as UIButtonParentHandler;

            transform.Find("Selector").gameObject.SetActive(true);
            buttonHandler.DeselectComponents(GetComponent<UserInterfaceSubComponent>());
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.Find("Selector").gameObject.SetActive(true);
        GetComponentInParent<InventoryComponent>().SelectComponent(GetComponent<UserInterfaceSubComponent>());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.Find("Selector").gameObject.SetActive(false);
    }

    #endregion
}

