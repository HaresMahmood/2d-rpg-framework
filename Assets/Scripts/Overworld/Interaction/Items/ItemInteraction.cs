using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class ItemInteraction : InteractableObject
{
    #region Variables

    private RangeHandler rangeHandler;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Item item;

    #endregion

    #region Unity Methods

    private void Start()
    {
        rangeHandler = gameObject.transform.Find("Range").GetComponent<RangeHandler>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Interact") && rangeHandler.playerInRange) //TODO: Make sure Player can't interact when 2 interactable ranges overlap!
        {
            if (CanInteract(transform.position))
            {
                ShowItem();
                Destroy(this.gameObject);
            }
        }
    }

    #endregion

    private void ShowItem()
    {
        InventoryManager.instance.ShowItem(item);
    }
}
