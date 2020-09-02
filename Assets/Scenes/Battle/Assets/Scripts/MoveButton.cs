using System.Collections;
using System.Collections.Generic;
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

    private TextMeshProUGUI moveName;
    private TextMeshProUGUI pp;

    private Image icon;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(PartyMember.MemberMove move)
    {
        moveName.SetText(move.Value.name);
        pp.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(GameManager.GetAccentColor())}>PP</color> {move.PP.ToString()}/{move.Value.pp}");

        //icon.sprite = move.Value.
    }

    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        moveName = transform.Find("Text/Name").GetComponent<TextMeshProUGUI>();
        pp = transform.Find("Text/PP").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<Image>();

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

