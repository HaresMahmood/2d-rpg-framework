using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class InventoryManager : MonoBehaviour
{
    #region Variables

    public static InventoryManager instance;

    [UnityEngine.Header("Setup")]
    public GameObject itemBox;
    public GameObject itemContainer;

    private TextMeshProUGUI itemName, itemCategory;
    private Image itemSprite;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        itemName = itemContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemCategory = itemContainer.transform.Find("Category").GetComponent<TextMeshProUGUI>();
        itemSprite = itemContainer.transform.Find("Sprite").GetComponent<Image>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion

    public void ShowItem(Item item)
    {
        if (!itemBox.activeSelf)
        {
            itemBox.SetActive(true);

            itemName.SetText(item.name);
            itemCategory.SetText(item.category.ToString().ToUpper());
            itemSprite.sprite = item.sprite;
        }
    }
}
