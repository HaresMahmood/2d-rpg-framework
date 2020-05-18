using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public abstract class ComponentUserInterface : MonoBehaviour
{
    #region Variables

    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI text;

    #endregion

    #region Fields

    [SerializeField] protected List<Sprite> icons = new List<Sprite>();

    #endregion

    #region Properties

    public List<Sprite> Icons
    {
        get { return icons; }
        set { icons = value; }
    }

    #endregion

    #region Miscellaneous Methods

    public virtual void UpdateUserInterface<T>(T information)
    {
        icon.sprite = icons.First(i => i.name.Contains(text.text));
    }

    #endregion

   #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    { }

    #endregion
}

