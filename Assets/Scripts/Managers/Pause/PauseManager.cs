using System;
using UnityEngine;

//TODO: Should not be here!
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region Variables

    public static PauseManager instance;

    /// <summary>
    /// Determines whether the game is paused or not.
    /// </summary>
    public bool isPaused; // Made public for debug purposes.

    [SerializeField] private GameObject pausePanel;

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
        isPaused = false;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        TogglePause();
    }

    #endregion

    public void TogglePause()
    {
        if (Input.GetButtonDown("Start") && !DialogManager.instance.isActive)
            isPaused = !isPaused;

        if (isPaused)
        {

            pausePanel.SetActive(true);

            //TODO: Move this whole block to InventoryManager!
            if (InventoryManager.instance.inventory.items.Count > 0)
            {
                Transform[] grid = pausePanel.transform.Find("Inventory/Item Grid").transform.GetChildren();
                foreach (Transform itemSlot in grid)
                {
                    if (Array.IndexOf(grid, itemSlot) < InventoryManager.instance.inventory.items.Count)
                    {
                        Item item = InventoryManager.instance.inventory.items[Array.IndexOf(grid, itemSlot)];
                        itemSlot.Find("Sprite").GetComponent<Image>().sprite = item.sprite;
                        itemSlot.Find("Sprite").gameObject.SetActive(true);
                        itemSlot.Find("Amount").GetComponentInChildren<TextMeshProUGUI>().SetText(item.amount.ToString());
                    }
                }
            }
            //

            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
