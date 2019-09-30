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
    public GameObject itemGrid;

    [UnityEngine.Header("Settings")]
    [SerializeField] private GameObject itemContainerPrefab;

    private List<Item> items;

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
        items = new List<Item>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion

    public void AddItem(Item item)
    {
        items.Add(item);
        ShowItem(item);
    }

    public void ShowItem(Item item)
    {
        GameObject itemContainerObj = (GameObject)Instantiate(itemContainerPrefab, Vector3.zero, Quaternion.identity); // Instantiates/creates new choice button from prefab in scene.

        itemContainerObj.name = "Item container"; // Gives appropriate name to newly instantiated choice button.
        itemContainerObj.transform.SetParent(itemGrid.transform, false);

        itemContainerObj.transform.Find("Item container/Item/Name").GetComponent<TextMeshProUGUI>().text = item.name;
        itemContainerObj.transform.Find("Item container/Item/Category").GetComponent<TextMeshProUGUI>().text = item.category.ToString().ToUpper();
        itemContainerObj.transform.Find("Item container/Item/Sprite").GetComponent<Image>().sprite = item.sprite;

        Destroy(itemContainerObj, itemContainerObj.transform.GetComponentInChildren<Animator>().GetAnimationTime() * 3); //TODO: Find better solution to destroy Item UI.
    }
}
