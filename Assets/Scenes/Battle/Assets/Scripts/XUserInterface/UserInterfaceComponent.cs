using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class UserInterfaceComponent : MonoBehaviour, UIHandler, UIHoverButtonHandler
{
    #region Variables

    protected List<UserInterfaceSubComponent> components;

    #endregion

    #region Miscellaneous Methods

    public virtual void DeselectComponents(UserInterfaceSubComponent selectedComponent)
    {
        List<UserInterfaceSubComponent> components = this.components.Where(c => c != selectedComponent && c.GetComponent<HoverButton>().IsSelected).ToList();

        selectedComponent.GetComponent<HoverButton>().Select(true);

        foreach (UserInterfaceSubComponent component in components)
        {
            component.GetComponent<HoverButton>().Select(false);
        }
    }

    public virtual void SetInformation<T>(T information)
    { }

    public virtual void SetInspectorValues()
    {
        components = GetComponentsInChildren<UserInterfaceSubComponent>().ToList();
    }

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        SetInspectorValues();
    }

    protected virtual void Start()
    {
        DeselectComponents(components[0]);
    }

    #endregion
}

