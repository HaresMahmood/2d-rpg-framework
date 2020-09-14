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
public class CategoryHoverButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    #region Properties

    public bool IsSelected { get; set; }

    #endregion

    #region Unity Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().enabled)
        {
            GetComponentInParent<CategoryComponent>().DeselectComponents(this);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponentInParent<CategoryComponent>().SelectComponent(this, true);

    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponentInParent<CategoryComponent>().SelectComponent(this, false);
    }

    #endregion
}

