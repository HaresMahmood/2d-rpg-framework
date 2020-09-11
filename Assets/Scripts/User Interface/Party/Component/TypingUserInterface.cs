using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class TypingUserInterface : ComponentUserInterface
{
    #region Fields

    [SerializeField] protected Typing type;
    [SerializeField] protected Sprite sprite;

    #endregion

    #region Properties

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
                SetSprite();
            }
        }
    }

    public Sprite Icon
    {
        get { return sprite; }
        set { sprite = value; }
    }

    #endregion

    #region Miscellaneous Methods

    public virtual void UpdateUserInterface(Typing type, Sprite sprite)
    {
        if (text == null)
        {
            text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            icon = transform.Find("Icon").GetComponent<Image>();
        }

        if (type.Value != Typing.Type.None)
        {
            text.SetText(type.Value.ToString());
            icon.sprite = sprite;

            text.color = type.Color;
            icon.color = type.Color;

            text.GetComponent<AutoTextWidth>().UpdateWidth(type.Value.ToString());

            if (!text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(true);
                icon.gameObject.SetActive(true);
            }
        }
        else
        {
            text.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
        }
    }

    private void SetSprite()
    {
        sprite = icons.First(i => i.name.Contains(type.Value.ToString().ToLower()));
        type.Color = GetColor(type.Value);
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
                    color = "#2c9fda".ToColor();
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
    protected override void Awake()
    {
        UpdateUserInterface(type, sprite);
    }

    #endregion
}
