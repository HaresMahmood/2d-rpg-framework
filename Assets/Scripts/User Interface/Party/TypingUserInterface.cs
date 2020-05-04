using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class TypingUserInterface : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Sprite> icons = new List<Sprite>();
    [SerializeField] [ReadOnly] private Typing type;
    [SerializeField] [ReadOnly] private Sprite icon;

    #endregion

    #region Variables

    private TextMeshProUGUI typeText;
    private Image typeIcon;

    #endregion

    #region Properties

    public List<Sprite> Icons
    {
        get { return icons; }
        set { icons = value; }
    }

    public Typing Type
    {
        get { return type; }
    }

    public Typing.Type Value
    {
        get { return type.Value; }
        set 
        {
            type.Value = value;

            if (type.Value != Typing.Type.None)
            {
                icon = icons.First(i => i.name.Contains(type.Value.ToString().ToLower()));
                type.color = GetColor(type.Value);
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

    public void UpdateUserInterface(Typing type, Sprite icon)
    {
        if (typeText == null)
        {
            typeText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            typeIcon = transform.Find("Icon").GetComponent<Image>();
        }

        if (type.Value != Typing.Type.None)
        {
            typeText.SetText(type.Value.ToString());
            typeIcon.sprite = icon;

            typeText.color = type.color;
            typeIcon.color = type.color;

            if (!typeText.gameObject.activeSelf)
            {
                typeText.gameObject.SetActive(true);
                typeIcon.gameObject.SetActive(true);
            }
        }
        else
        {
            typeText.gameObject.SetActive(false);
            typeIcon.gameObject.SetActive(false);
        }
    }

    private Color GetColor(Typing.Type type)
    {
        Color color = new Color();

        switch (type)
        {
            default: { break; }
            case (Typing.Type.Bug):
                {
                    color = "#a0cc47".ToColor();
                    break;
                }
            case (Typing.Type.Dark):
                {
                    color = "#73553f".ToColor();
                    break;
                }
            case (Typing.Type.Dragon):
                {
                    color = "#5050e6".ToColor();
                    break;
                }
            case (Typing.Type.Electric):
                {
                    color = "#ffcc33".ToColor();
                    break;
                }
            case (Typing.Type.Fairy):
                {
                    color = "#e67ed4".ToColor();
                    break;
                }
            case (Typing.Type.Fighting):
                {
                    color = "#b34a36".ToColor();
                    break;
                }
            case (Typing.Type.Fire):
                {
                    color = "#f26130".ToColor();
                    break;
                }
            case (Typing.Type.Flying):
                {
                    color = "#7e87e6".ToColor();
                    break;
                }
            case (Typing.Type.Ghost):
                {
                    color = "#6652cc".ToColor();
                    break;
                }
            case (Typing.Type.Grass):
                {
                    color = "#56bf4d".ToColor();
                    break;
                }
            case (Typing.Type.Ground):
                {
                    color = "#bfa156".ToColor();
                    break;
                }
            case (Typing.Type.Ice):
                {
                    color = "#66cccc".ToColor();
                    break;
                }
            case (Typing.Type.Normal):
                {
                    color = "#a69974".ToColor();
                    break;
                }
            case (Typing.Type.Poison):
                {
                    color = "#bf43aa".ToColor();
                    break;
                }
            case (Typing.Type.Psychic):
                {
                    color = "#ff4d79".ToColor();
                    break;
                }
            case (Typing.Type.Rock):
                {
                    color = "#a68542".ToColor();
                    break;
                }
            case (Typing.Type.Steel):
                {
                    color = "#a3aacc".ToColor();
                    break;
                }
            case (Typing.Type.Water):
                {
                    color = "#a3aacc".ToColor();
                    break;
                }
        }

        return color;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        UpdateUserInterface(type, icon);
    }

    #endregion
}
