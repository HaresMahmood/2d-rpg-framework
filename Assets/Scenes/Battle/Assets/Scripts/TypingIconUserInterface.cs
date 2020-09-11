using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class TypingIconUserInterface : TypingUserInterface
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    public override void UpdateUserInterface(Typing type, Sprite sprite)
    {
        if (icon == null)
        {
            icon = GetComponent<Image>();
        }

        if (type.Value != Typing.Type.None)
        {
            icon.sprite = sprite;
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #endregion
}

