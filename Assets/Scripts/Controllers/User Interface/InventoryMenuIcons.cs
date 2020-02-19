using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class InventoryMenuIcons : MonoBehaviour
{
    #region Variables

    public static InventoryMenuIcons instance;

    [SerializeField] private List<Sprite> icons = new List<Sprite>();

    public List<Sprite> Icons 
    { 
        get 
        { 
            return icons; 
        } 
        private set 
        {
            icons = value; 
        } 
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion
}
