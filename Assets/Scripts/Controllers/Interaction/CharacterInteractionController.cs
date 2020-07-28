using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(CharacterMovement))]
public class CharacterInteractionController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Dialog dialog;

    #endregion

    #region Properties

    public Dialog Dialog;

    #endregion

    #region Miscellaneous Methods

    public void Interact(bool isActive, Vector3 orientation)
    {
        GetComponent<CharacterMovement>().SetOrientation(orientation);
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

