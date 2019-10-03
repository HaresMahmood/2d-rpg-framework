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
    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Interact") && rangeHandler.playerInRange) //TODO: Make sure Player can't interact when 2 interactable ranges overlap!
        {
            if (CanInteract())
            {
                AddItem();
                Destroy(this.gameObject);
            }
        }

        if (rangeHandler.playerInRange && PlayerInteraction.contextBox.activeSelf)
            SetContextText("Pick up.");
    }

    #endregion

    private void AddItem()
    {
        InventoryManager.instance.AddItem(item);
    }
}
