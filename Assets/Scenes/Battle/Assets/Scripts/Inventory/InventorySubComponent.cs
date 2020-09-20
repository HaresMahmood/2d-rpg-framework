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
    #region Properties

    public Item Item { get { return item; } set { item = value; } }

    #endregion

    #region Variables

    [Header("Values")]
    [SerializeField, ReadOnly] private Item item;

    private Transform information;

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
            information.gameObject.SetActive(isActive);
        }

        information.GetComponent<CanvasGroup>().DOFade(Convert.ToInt32(isActive), animationDuration).OnComplete(() => information.gameObject.SetActive(isActive));
    }

    public override void SetInformation<T>(T information)
    {
        Item item = (Item)Convert.ChangeType(information, typeof(Item));

        this.item = item;

        itemSprite.sprite = item.Sprite;
        //quantityText.SetText($"x{item.Quantity}");
        //favoriteTag.SetActive(item.IsFavorite);
        //newTag.SetActive(item.IsNew);
    }

    public override void SetInspectorValues()
    {
        base.SetInspectorValues();

        information = transform.Find("Information");

        favoriteTag = information.Find("Tags/Favorite").gameObject;
        newTag = information.Find("Tags/New").gameObject;

        itemSprite = information.Find("Sprite").GetComponent<Image>();
        quantityText = information.Find("Quantity").GetComponentInChildren<TextMeshProUGUI>();
    }

    #endregion

    #region Unity Methods

    #endregion
}

