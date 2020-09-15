using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class InventorySubComponent : UserInterfaceSubComponent
{
    #region Variables

    [Header("Values")]
    [SerializeField, ReadOnly] private Item item;

    private Transform slot;

    private GameObject favoriteTag;
    private GameObject newTag;

    private Image itemSprite;
    private TextMeshProUGUI quantityText;

    #endregion

    #region Miscellaneous Methods

    public void Animate(bool isActive, float animationDuration)
    {
        if (isActive)
        {
            slot.gameObject.SetActive(isActive);
        }

        slot.GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(isActive), animationDuration).OnComplete(() => slot.gameObject.SetActive(isActive));
    }

    public void SetInformation(Item information)
    {
       // Item item = (Item)Convert.ChangeType(information, typeof(Item));

        this.item = information;

        itemSprite.sprite = item.Sprite;
        quantityText.SetText($"x{item.Quantity}");
        favoriteTag.SetActive(item.IsFavorite);
        newTag.SetActive(item.IsNew);
    }

    public override void SetInspectorValues()
    {
        slot = transform.Find("Information");

        favoriteTag = slot.Find("Tags/Favorite").gameObject;
        newTag = slot.Find("Tags/New").gameObject;

        itemSprite = slot.Find("Sprite").GetComponent<Image>();
        quantityText = slot.Find("Quantity").GetComponentInChildren<TextMeshProUGUI>();
    }

    #endregion

    #region Unity Methods

    #endregion
}

