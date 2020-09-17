using UnityEngine;

/// <summary>
///
/// </summary>
public class UserInterfaceSubComponent : MonoBehaviour, UIHandler
{
    #region Variables

    private GameObject selector;

    #endregion

    #region Miscellaneous Methods

    public virtual void Select(bool isSelected)
    {
        if (selector != null)
        {
            selector.SetActive(isSelected);
        }
    }

    public virtual void SetInformation<T>(T information)
    { }

    public virtual void SetInspectorValues()
    {
        if (transform.Find("Selector") != null)
        {
            selector = transform.Find("Selector").gameObject;
        }
    }

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        SetInspectorValues();
    }

    #endregion
}

