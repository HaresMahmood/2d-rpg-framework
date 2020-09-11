using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//////////////////////////////////////
// TODO: Redo (with interface)
//////////////////////////////////////


/// <summary>
///
/// </summary>
public class PartyButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler //, IPointerExitHandler
{
    #region Unity Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().enabled)
        {
            GetComponent<PartySubComponent>().AnimateSlot(0.35f);
            transform.Find("Selector").gameObject.SetActive(true);
            GetComponentInParent<PartyComponent>().DeselectComponents(GetComponent<PartySubComponent>());
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponent<PartySubComponent>().AnimateSlot(0.35f);
        transform.Find("Selector").gameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<PartySubComponent>().AnimateSlot(0.2f);
        transform.Find("Selector").gameObject.SetActive(false);
    }

    #endregion
}

