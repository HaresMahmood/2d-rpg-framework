using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class ItemSlot : MonoBehaviour
{
    #region Variables

    private Transform slot;

    private GameObject favoriteTag;
    private GameObject newTag;

    private Image itemSprite;
    private TextMeshProUGUI amountText;

    #endregion

    #region Miscellaneous Methods

    public void AnimateSlot(float opacity, float duration = -1)
    {
        if (duration > -1)
        {
            StartCoroutine(slot.gameObject.FadeOpacity(opacity, duration));
        }
        else
        {
            slot.GetComponent<CanvasGroup>().alpha = opacity;
            slot.gameObject.SetActive(false);
        }
    }

    public void UpdateInformation(Item item, float duration = -1, bool animate = true) // TODO: Make cleaner (no bool)
    {
        slot.gameObject.SetActive(true);

        itemSprite.sprite = item.sprite;
        amountText.SetText(item.amount.ToString());
        favoriteTag.SetActive(item.isFavorite);
        newTag.SetActive(item.isNew);

        if (animate)
        {
            AnimateSlot(1f, duration);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        slot = transform.Find("Information");

        favoriteTag = slot.Find("Tags/Favorite").gameObject;
        newTag = slot.Find("Tags/New").gameObject;

        itemSprite = slot.Find("Sprite").GetComponent<Image>();
        amountText = slot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>();

        slot.gameObject.SetActive(false);
    }

    #endregion
}
