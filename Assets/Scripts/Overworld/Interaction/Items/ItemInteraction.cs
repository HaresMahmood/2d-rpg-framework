using UnityEngine;

/// <summary>
///
/// </summary>
public class ItemInteraction : InteractableObject
{
    #region Variables

    private RangeHandler rangeHandler;

    [UnityEngine.Header("Settings")]
    public OverworldItem overworldItem;
    [Range(0.15f, 0.75f)] [SerializeField] private float duration = 0.3f;

    #endregion

    #region Unity Methods

    private void Start()
    {
        rangeHandler = gameObject.transform.Find("Range").GetComponent<RangeHandler>();

        if (overworldItem.isPickedUp)
            Destroy(this.gameObject);
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
                AnimateLight(duration);
            }
        }

        if (rangeHandler.playerInRange && PlayerInteraction.contextBox.activeSelf)
            SetContextText("Pick up");
    }

    #endregion

    private void AddItem()
    {
       OverworldItemManager.instance.AddItem(overworldItem);
    }

    private void AnimateLight(float duration)
    {
        OverworldItemController.instance.FadeItem(0f, duration);
        Destroy(this.gameObject, duration);
    }
}
