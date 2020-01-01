using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MemberInformation : MonoBehaviour // TODO: Think of better name.
{
    #region Variables

    Transform generalInformation;
    Transform progress;
    Transform item;

    TextMeshProUGUI nameText;
    TextMeshProUGUI dexInformationText;
    TextMeshProUGUI statusText;

    TextMeshProUGUI levelText;
    TextMeshProUGUI currentExpText;
    TextMeshProUGUI remainingExpText;

    TextMeshProUGUI heldItemText;
    Image heldItemSprite;

    #endregion

    #region Miscellaneous Methods

    private Color SetStatusColor(Pokemon.Status status)
    {
        Color color;

        switch (status)
        {
            case Pokemon.Status.Paralyzed:
                {
                    color = "FCFF83".ToColor();
                    break;
                }
            case Pokemon.Status.Burned:
                {
                    color = "FF9D83".ToColor();
                    break;
                }
            case Pokemon.Status.Frozen:
                {
                    color = "A0FCFF".ToColor();
                    break;
                }
            case Pokemon.Status.Poisoned:
                {
                    color = "F281FF".ToColor();
                    break;
                }
            case Pokemon.Status.Asleep:
                {
                    color = "B0B0B0".ToColor();
                    break;
                }
            default:
                {
                    color = Color.white;
                    break;
                }
        }

        return color;
    }

    public void UpdateInformation(Pokemon pokemon)
    {
        nameText.SetText(pokemon.name);
        dexInformationText.SetText(pokemon.category.ToUpper() + " <color=#696969>POKÉMON  -  #</color>00" + pokemon.id);
        statusText.SetText(pokemon.status.ToString());

        levelText.SetText(pokemon.level.ToString());
        currentExpText.SetText(pokemon.exp.ToString());
        remainingExpText.SetText("TO NEXT   " + (pokemon.totalExp - pokemon.exp).ToString());
        //StartCoroutine(progress.Find("Experience/Experience/Bar/Amount").LerpScale(new Vector2((pokemon.exp / pokemon.totalExp), 1), 0.3f));

        heldItemText.SetText(pokemon.heldItem.Name);
        heldItemSprite.sprite = pokemon.heldItem.sprite;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        generalInformation = transform.Find("General Information");
        progress = transform.Find("Progress/Progress");
        item = transform.Find("Held Item/Held Item");

        nameText = generalInformation.Find("Name/Name").GetComponent<TextMeshProUGUI>();
        dexInformationText = generalInformation.Find("Name/Dex Information/Information").GetComponent<TextMeshProUGUI>();
        statusText = generalInformation.Find("Status/Status Ailment").GetComponent<TextMeshProUGUI>();


        levelText = progress.Find("Level/Level").GetComponent<TextMeshProUGUI>();
        currentExpText = progress.Find("Experience/Experience/Exp").GetComponent<TextMeshProUGUI>();
        remainingExpText = progress.Find("Experience/Experience/To Next").GetComponent<TextMeshProUGUI>();

        heldItemText = item.Find("Item Name").GetComponent<TextMeshProUGUI>();
        heldItemSprite = item.Find("Item Sprite").GetComponent<Image>();
    }

    #endregion
}
