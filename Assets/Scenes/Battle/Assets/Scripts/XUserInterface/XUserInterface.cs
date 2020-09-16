using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class XUserInterface<T> : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] protected T information;

    protected List<UserInterfaceComponent> components;

    #endregion

    #region Miscellaneous Methods

    protected UserInterfaceComponent FindComponent(string name)
    {
        return components.Find(c => c.name == name);
    }

    /*
    protected T FindComponent<T>()
    {
        return components.Find(c => c is T));
    }
    */

    #endregion
    
    #region Unity Methods
    
    protected virtual void Awake()
    {
        components = GetComponentsInChildren<UserInterfaceComponent>().ToList();
    }

    protected virtual void Start()
    {
        foreach (UserInterfaceComponent component in components)
        {
            component.SetInformation(information);
        }
    }

    #endregion
}

