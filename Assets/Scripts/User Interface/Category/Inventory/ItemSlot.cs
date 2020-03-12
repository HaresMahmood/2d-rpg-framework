using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class ItemSlot : CategorizableSlot
{
    #region Variables

    private GameObject favoriteTag;
    private GameObject newTag;

    private Image itemSprite;
    private TextMeshProUGUI quantityText;

    #endregion

    #region Miscellaneous Methods

    protected override void SetInformation(Categorizable categorizable)
    {
        Item item = (Item)categorizable;

        itemSprite.sprite = item.Sprite;
        quantityText.SetText(item.Quantity.ToString());
        favoriteTag.SetActive(item.IsFavorite);
        newTag.SetActive(item.IsNew);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// 
    /// Derives from CategorizableSlot base class.
    /// </summary>
    protected override void Awake()
    {
        slot = transform.Find("Information");

        favoriteTag = slot.Find("Tags/Favorite").gameObject;
        newTag = slot.Find("Tags/New").gameObject;

        itemSprite = slot.Find("Sprite").GetComponent<Image>();
        quantityText = slot.Find("Quantity").GetComponentInChildren<TextMeshProUGUI>();

        //base.Awake();
    }

    #endregion
}
