using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class ItemInteractionController : InteractableObject
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private Item item;

    #endregion

    #region Miscellaneous Methods

    public override void Interact(Vector3 orienation)
    {
        Debug.Log(true);
    }

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

