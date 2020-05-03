using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class TypingUserInterface : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Sprite> icons = new List<Sprite>();
    [SerializeField] [ReadOnly] private Pokemon.Typing typing;
    [SerializeField] [ReadOnly] private Sprite icon;

    #endregion

    #region Properties

    public List<Sprite> Icons
    {
        get { return icons; }
        set { icons = value; }
    }

    public Pokemon.Typing Typing
    {
        get { return typing; }
        set 
        {
            typing = value;

            if (typing != Pokemon.Typing.None)
            {
                icon = icons.First(i => i.name.Contains(typing.ToString().ToLower()));
            }
        }
    }

    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods


    #endregion
}
