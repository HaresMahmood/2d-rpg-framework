using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class OverworldItemManager : MonoBehaviour
{
    #region Variables

    public static OverworldItemManager instance;

    [UnityEngine.Header("Settings")]
    [SerializeField] private GameObject itemGrid;
    [SerializeField] private GameObject itemContainerPrefab;

    private List<GameObject> itemContainers;
    private bool displayingItem = false;

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
        itemContainers = new List<GameObject>();
    }

    #endregion

    public void AddItem(OverworldItem overworldItem)
    {
        Item item = overworldItem.item;
        //if (!//InventoryController.instance.inventory.items.Contains(item))
            //InventoryController.instance.inventory.items.Add(item);

        //InventoryController.instance.inventory.items.Find(i => i.ID == item.ID).amount++;

        DisplayItem(item);
        //overworldItem.isPickedUp = true;
        //item.isNew = true;
    }

    public void DisplayItem(Item item)
    {
        displayingItem = true;
        GameObject containerObj = (GameObject)Instantiate(itemContainerPrefab, Vector3.zero, Quaternion.identity);

        containerObj.name = "Overworld Item Container";
        containerObj.transform.SetParent(itemGrid.transform, false);

        containerObj.transform.Find("Overworld Item Container/Item/Name").GetComponent<TextMeshProUGUI>().text = item.Name;
        //containerObj.transform.Find("Overworld Item Container/Item/Category").GetComponent<TextMeshProUGUI>().text = item.category.ToString().ToUpper();
        //containerObj.transform.Find("Overworld Item Container/Item/Sprite").GetComponent<Image>().sprite = item.sprite;

        itemContainers.Add(containerObj);

        Destroy(containerObj.gameObject, containerObj.transform.GetComponentInChildren<Animator>().GetAnimationTime() * 3);
        displayingItem = false;
    }

    private void DestroyObjects()
    {
        foreach (GameObject container in itemContainers.ToArray())
        {
            Destroy(container.gameObject);
        }

        itemContainers.Clear();
    }
}
