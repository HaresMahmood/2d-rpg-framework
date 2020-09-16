using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(Button))]
public class HoverButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    #region Properties

    public virtual bool IsSelected 
    {
        get { return transform.Find("Selector").gameObject; }
        protected set { }
    }

    #endregion

    #region Miscellaneous Methods

    public virtual void Select(bool isSelected)
    {
        transform.Find("Selector").gameObject.SetActive(isSelected);
    }

    protected virtual void Hover()
    {
        if (GetComponent<Button>().enabled)
        {
            UIHoverButtonHandler buttonHandler = GetComponentInParent(typeof(UIHoverButtonHandler)) as UIHoverButtonHandler;
            
            buttonHandler.DeselectComponents(GetComponent<UserInterfaceSubComponent>());
        }
    }

    #endregion

    #region Unity Methods

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Hover();
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        Hover();
    }

    #endregion
}

