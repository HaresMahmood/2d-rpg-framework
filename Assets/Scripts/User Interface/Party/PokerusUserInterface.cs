using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PokerusUserInterface : ComponentUserInterface
{
    #region Variables

    private GameObject separator;
    private Image icon;
    private TextMeshProUGUI pokerusText;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateUserInterface<T>(T information)
    {
        PartyMember.MemberPokerus pokerus = (PartyMember.MemberPokerus)Convert.ChangeType(information, typeof(PartyMember.MemberPokerus));

        if (pokerus.Status == PartyMember.MemberPokerus.InfectionStatus.Uninfected)
        {
            separator.SetActive(false);
            icon.gameObject.SetActive(false);
            pokerusText.gameObject.SetActive(false);
        }
        else
        {
            separator.SetActive(true);
            icon.gameObject.SetActive(true);
            pokerusText.gameObject.SetActive(true);

            icon.sprite = icons.First(i => i.name.Contains(pokerus.Status.ToString().ToLower()));
            pokerusText.SetText(pokerus.Status.ToString());
            // TODO: Add strain to UI
        }
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        separator = transform.Find("Separator").gameObject;
        icon = transform.Find("Icon").GetComponent<Image>();
        pokerusText = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}

