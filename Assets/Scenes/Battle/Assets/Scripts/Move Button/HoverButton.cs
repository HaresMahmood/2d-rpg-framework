using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
/// </summary>
public class HoverButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler //, IPointerExitHandler
{
    #region Variables



    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.Find("Text").gameObject.SetActive(true);
        transform.Find("Selector").gameObject.SetActive(true);
        GetComponentInParent<MoveButtonComponent>().DeselectButtons(GetComponent<Button>());
    }

    /*
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.Find("Text").gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }
    */

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        transform.Find("Text").gameObject.SetActive(true);
        transform.Find("Selector").gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.Find("Text").gameObject.SetActive(false);
        transform.Find("Selector").gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    #endregion
}

