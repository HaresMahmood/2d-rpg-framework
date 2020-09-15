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
public class CategoryHoverButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
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
        if (GetComponent<Button>().enabled)
        {
            GetComponentInParent<CategoryComponent>().DeselectComponents(this);
        }

    }

    #endregion
}

