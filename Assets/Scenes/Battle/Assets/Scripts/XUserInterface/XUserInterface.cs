using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class XUserInterface<T> : MonoBehaviour
{
    #region Variables

    [SerializeField] protected T information;

    protected List<UserInterfaceComponent> components;

    #endregion

    #region Miscellaneous Methods



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

