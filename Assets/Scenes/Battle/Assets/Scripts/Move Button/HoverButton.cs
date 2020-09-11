using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
/// </summary>
public class HoverButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler //, IPointerExitHandler
{
    #region Unity Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().enabled)
        {
            GetComponent<MoveButtonSubComponent>().SelectButton(true);
            GetComponentInParent<MoveButtonComponent>().DeselectButtons(GetComponent<Button>());
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponent<MoveButtonSubComponent>().SelectButton(true);
        GetComponentInParent<MoveButtonComponent>().DeselectButtons(GetComponent<Button>());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<MoveButtonSubComponent>().SelectButton(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    #endregion
}

