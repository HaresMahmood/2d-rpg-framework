using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InputController : MonoBehaviour
{
    #region Variables

    private Controls controls;

    #endregion

    #region Miscellaneous Methods



    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        controls = new Controls();


    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        controls.UI.Enable();

    }

    private void OnDisable()
    {
        controls.UI.Disable();
    }

    #endregion
}

