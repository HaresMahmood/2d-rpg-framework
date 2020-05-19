using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PartyItemSlot : PartyInformationSlot
{
    #region Variables

    private Transform heldItem;
    private Transform effect;
    private Transform description;

    private TextMeshProUGUI itemText;
    private Image itemIcon;

    private TextMeshProUGUI effectText;

    private TextMeshProUGUI descriptionText;

    // TODO: Crappy names...
    private GameObject text;
    private GameObject empty;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        if (member.HeldItem != null)
        {
            itemText.SetText(member.HeldItem.Name);
            itemIcon.sprite = member.HeldItem.Sprite;

            effectText.SetText(member.HeldItem.Effect.ToString());

            descriptionText.SetText(member.HeldItem.Description);


            itemIcon.gameObject.SetActive(true);

            text.SetActive(true);
            effectText.gameObject.SetActive(true);

            description.gameObject.SetActive(true);

            empty.SetActive(false);
        }
        else
        {
            itemText.SetText("None");
            itemIcon.gameObject.SetActive(false);

            text.SetActive(false);
            effectText.gameObject.SetActive(false);

            description.gameObject.SetActive(false);

            empty.SetActive(true);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        Transform slot = transform.Find("Information Panel").transform;

        heldItem = slot.Find("Held Item/Values");
        effect = slot.Find("Effect");
        description = slot.Find("Description");

        itemText = heldItem.Find("Value").GetComponent<TextMeshProUGUI>();
        itemIcon = heldItem.Find("Icon").GetComponent<Image>();

        effectText = effect.Find("Value").GetComponent<TextMeshProUGUI>();

        descriptionText = description.Find("Value").GetComponent<TextMeshProUGUI>();

        text = effect.Find("Text").gameObject;
        empty = effect.Find("Empty").gameObject;

        base.Awake();
    }

    #endregion
}
