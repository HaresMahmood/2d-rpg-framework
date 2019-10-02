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
    public List<GameObject> itemContainers;

    public bool displayingItem = false;

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
        itemContainers = new List<GameObject>();
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
        DisplayItem(item);

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
