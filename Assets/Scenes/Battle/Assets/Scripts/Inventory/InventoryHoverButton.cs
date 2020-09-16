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
public class InventoryHoverButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    #region Unity Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().enabled)
        {
            UIButtonParentHandler buttonHandler = GetComponentInParent(typeof(UIButtonParentHandler)) as UIButtonParentHandler;

            transform.Find("Selector").gameObject.SetActive(true);
            ((InventoryComponent)buttonHandler).SetDescription(GetComponent<InventorySubComponent>().Item);
            buttonHandler.DeselectComponents(GetComponent<UserInterfaceSubComponent>());
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GetComponent<Button>().enabled)
        {
            UIButtonParentHandler buttonHandler = GetComponentInParent(typeof(UIButtonParentHandler)) as UIButtonParentHandler;

            transform.Find("Selector").gameObject.SetActive(true);
            ((InventoryComponent)buttonHandler).SetDescription(GetComponent<InventorySubComponent>().Item);
            buttonHandler.DeselectComponents(GetComponent<UserInterfaceSubComponent>());
        }
    }

    #endregion
}

