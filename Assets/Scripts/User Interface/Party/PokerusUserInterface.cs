using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PokerusUserInterface : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<Sprite> icons = new List<Sprite>();

    private GameObject separator;
    private Image icon;
    private TextMeshProUGUI pokerusText;

    #endregion

    #region Miscellaneous Methods

    public void UpdateUserInterface(PartyMember.MemberPokerus pokerus)
    {
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
    private void Awake()
    {
        separator = transform.Find("Separator").gameObject;
        icon = transform.Find("Icon").GetComponent<Image>();
        pokerusText = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}

