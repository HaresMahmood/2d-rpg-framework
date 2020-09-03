using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MoveButton : MonoBehaviour
{
    #region Variables

    [SerializeField] private PartyMember.MemberMove move;
    [SerializeField] private GameManager manager;

    private TextMeshProUGUI moveName;
    private TextMeshProUGUI pp;

    private TypingIconUserInterface icon;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(PartyMember.MemberMove move)
    {
        moveName.SetText(move.Value.name);
        pp.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(manager.accentColor)}>PP</color> {move.PP}/{move.Value.pp}");

        icon.Value = move.Value.typing.Value;
        icon.UpdateUserInterface(icon.Type, icon.Icon);

        float h, s;

        Color.RGBToHSV(new Color(icon.Type.Color.r, icon.Type.Color.g, icon.Type.Color.b), out h, out s, out _);
        Color color = Color.HSVToRGB(h, s, 0.75f);
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
    }

    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        moveName = transform.Find("Text/Name").GetComponent<TextMeshProUGUI>();
        pp = transform.Find("Text/PP").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<TypingIconUserInterface>();

        SetInformation(move);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #endregion
}

