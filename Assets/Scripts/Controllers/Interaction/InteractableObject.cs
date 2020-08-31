using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    public abstract void Interact(Vector3 orienation);

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        
    }


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}

