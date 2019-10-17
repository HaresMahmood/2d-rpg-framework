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

    public void AddItem(Item item)
    {
        if (!InventoryManager.instance.inventory.items.Contains(item))
            InventoryManager.instance.inventory.items.Add(item);

        InventoryManager.instance.inventory.items.Find(i => i.id == item.id).amount++;

        DisplayItem(item);
        //item.isPickedUp = true;
    }

    public void DisplayItem(Item item)
    {
        displayingItem = true;
        GameObject containerObj = (GameObject)Instantiate(itemContainerPrefab, Vector3.zero, Quaternion.identity);

        containerObj.name = "Overworld Item Container";
        containerObj.transform.SetParent(itemGrid.transform, false);

        containerObj.transform.Find("Overworld Item Container/Item/Name").GetComponent<TextMeshProUGUI>().text = item.name;
        containerObj.transform.Find("Overworld Item Container/Item/Category").GetComponent<TextMeshProUGUI>().text = item.category.ToString().ToUpper();
        containerObj.transform.Find("Overworld Item Container/Item/Sprite").GetComponent<Image>().sprite = item.sprite;

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
