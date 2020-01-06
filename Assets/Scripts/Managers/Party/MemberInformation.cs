﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MemberInformation : MonoBehaviour // TODO: Think of better name.
{
    #region Variables

    Transform generalInformation;
    Transform progression;
    Transform ability;
    Transform item;

    TextMeshProUGUI nameText;
    TextMeshProUGUI categoryText;
    TextMeshProUGUI dexText;
    TextMeshProUGUI statusText;

    TextMeshProUGUI levelText;
    TextMeshProUGUI currentExpText;
    TextMeshProUGUI remainingExpText;
    TextMeshProUGUI metLevelText;
    TextMeshProUGUI locationText;

    TextMeshProUGUI abilityText;
    TextMeshProUGUI abilityDescriptionText;
    TextMeshProUGUI natureText;

    TextMeshProUGUI heldItemText;
    Image heldItemSprite;
    TextMeshProUGUI effectText;
    TextMeshProUGUI itemDescriptionText;

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
        categoryText.SetText(pokemon.category.ToUpper()); categoryText.GetComponent<AutoTextWidth>().UpdateWidth(pokemon.category.ToUpper());
        dexText.SetText(pokemon.id.ToString()); dexText.GetComponent<AutoTextWidth>().UpdateWidth(pokemon.id.ToString());
        statusText.SetText(pokemon.status.ToString());

        levelText.SetText(pokemon.level.ToString());
        currentExpText.SetText(pokemon.exp.ToString());
        remainingExpText.SetText((pokemon.totalExp - pokemon.exp).ToString());
        //StartCoroutine(progress.Find("Experience/Experience/Bar/Amount").LerpScale(new Vector2((pokemon.exp / pokemon.totalExp), 1), 0.3f));

        //abilityText.SetText(pokemon.ability);
        //abilityDescriptionText.SetText("Test description");
        //natureText.SetText(pokemon.nature);

        heldItemText.SetText(pokemon.heldItem.Name);
        heldItemSprite.sprite = pokemon.heldItem.sprite;
        //effectText.SetText(pokemon.heldItem.effect);
        itemDescriptionText.SetText(pokemon.heldItem.description);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        generalInformation = transform.Find("General Information");
        progression = transform.Find("Progression");
        ability = transform.Find("Ability");
        item = transform.Find("Item");

        nameText = generalInformation.Find("Name/Value").GetComponent<TextMeshProUGUI>();
        categoryText = generalInformation.Find("Name/Dex Information/Category/Value").GetComponent<TextMeshProUGUI>();
        dexText = generalInformation.Find("Name/Dex Information/Dex Number/Value").GetComponent<TextMeshProUGUI>();
        statusText = generalInformation.Find("Status Ailment/Value").GetComponent<TextMeshProUGUI>();

        levelText = progression.Find("Level/Value").GetComponent<TextMeshProUGUI>();
        currentExpText = progression.Find("Experience/Text/Values/Value").GetComponent<TextMeshProUGUI>();
        remainingExpText = progression.Find("Experience/Text/Values/Remaining/Value").GetComponent<TextMeshProUGUI>();
        metLevelText = progression.Find("Met At/Values/Level/Value").GetComponent<TextMeshProUGUI>();
        locationText = progression.Find("Met At/Values/Level/Value").GetComponent<TextMeshProUGUI>();

        abilityText = ability.Find("Ability/Value").GetComponent<TextMeshProUGUI>();
        abilityDescriptionText = abilityText = ability.Find("Description/Value").GetComponent<TextMeshProUGUI>();
        natureText = abilityText = ability.Find("Nature/Value").GetComponent<TextMeshProUGUI>();

        heldItemText = item.Find("Item Name").GetComponent<TextMeshProUGUI>();
        heldItemSprite = item.Find("Item Sprite").GetComponent<Image>();
        effectText = item.Find("Effect/Value").GetComponent<TextMeshProUGUI>();
        itemDescriptionText = item.Find("Description/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
