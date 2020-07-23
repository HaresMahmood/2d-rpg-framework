using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>

// TODO: Clean up!
public class ItemInformationUserInterface : InformationUserInterface
{
    #region Constants

    public override int MaxObjects => selectedItem.Behavior.Count;

    private readonly Color tulipTreeColor = "#EAC03E".ToColor();

    #endregion

    #region Variables

    private List<MenuButton> buttons = new List<MenuButton>();

    private QuantitySelectorUserInterface quantitySelector;

    private Transform verticalPanel;
    private Transform horizontalPanel;
    private GameObject effectArrow;
    private RectTransform buttonPanel;
    private Animator informationAnimator;
    private Image spriteImage;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI valueText;
    private TextMeshProUGUI effectTypeText;
    private TextMeshProUGUI effectQuantityText;

    private Item selectedItem;

    private int selectedValue;

    #endregion

    #region Behavior Definitions

    public void Favorite(Item item, float animationDuration = 0.1f)
    {
        item.IsFavorite = !item.IsFavorite;

        Color color = item.IsFavorite ? "#EAC03E".ToColor() : Color.white;
        int index = item.Behavior.FindIndex(b => b.buttonName == "Favorite");

        StartCoroutine(buttons[index].transform.Find("Big Icon/Icon").gameObject.FadeColor(color, animationDuration));
        StartCoroutine(buttons[index].transform.Find("Small Icon/Icon").gameObject.FadeColor(color, animationDuration));
    }

    public void Discard(Item item, float opacity = 0.7f)
    {
        int index = item.Behavior.FindIndex(b => b.buttonName == "Discard");
        float xCoordinate = buttons[index].transform.position.x + (buttons[index].GetComponent<RectTransform>().sizeDelta.x / 2);

        StartCoroutine(AnimateSelector());
        StartCoroutine(ActivateButtons(selectedItem, opacity, MaxObjects));

        QuantitySelectorController.Instance.UserInterface = quantitySelector;

        StartCoroutine(ItemInformationController.Instance.SetActive(false));
        StartCoroutine(QuantitySelectorController.Instance.SetActive(true));

        quantitySelector.ToggleSelector(true, item, xCoordinate);
    }

    public void Cancel()
    {
        StartCoroutine(ResetSubMenu(selectedValue));
        StartCoroutine(InventoryController.Instance.SetActive(true));
        ((InventoryUserInterface)InventoryController.Instance.UserInterface).FadeInventoryUserInterface(1f);
        StartCoroutine(ItemInformationController.Instance.SetActive(false));
    }

    public void Use(Item item)
    {
        
    }

    #endregion

    #region Miscellaneous Methods

    public void ToggleSubMenu(Item item, bool isActive)
    {
        if (item != null)
        {
            selectedItem = item;

            if (isActive)
            {
                SetObjectDefinitionsFromPanel(horizontalPanel);
                SetValues(item);
                valueText.SetText((item).Quantity.ToString());
                spriteImage.sprite = (item).Sprite;
            }
        }

        StartCoroutine(AnimateSubMenu(item, isActive));

        if (!isActive)
        {
            StartCoroutine(InventoryController.Instance.SetActive(true));
            ((InventoryUserInterface)InventoryController.Instance.UserInterface).FadeInventoryUserInterface(1f);
        }

        StartCoroutine(ItemInformationController.Instance.SetActive(isActive));
    }

    public void InvokeItemBehavior(int selectedValue)
    {
        selectedItem.Behavior[selectedValue].behaviorEvent.Invoke();;
    }

    public override void SetValues(ScriptableObject selectedObject)
    {
        Item item = (Item)selectedObject;

        Color textColor = item.Effect.GetQuantity().Equals("") ? "#B0B0B0".ToColor() : GameManager.GetAccentColor();
        bool arrowState = item.Effect.GetQuantity().Equals("") ? false : true;

        nameText.SetText(item.Name);
        nameText.GetComponent<AutoTextWidth>().UpdateWidth(item.Name);
        descriptionText.SetText(item.Description);

        effectTypeText.SetText(item.Effect.ToString());
        effectQuantityText.SetText(item.Effect.GetQuantity());

        effectTypeText.GetComponent<AutoTextWidth>().UpdateWidth(item.Effect.ToString());

        if (!effectTypeText.color.Equals(textColor))
        {
            effectTypeText.color = textColor;
        }

        if (!effectArrow.activeSelf.Equals(arrowState))
        {
            effectArrow.SetActive(arrowState);
        }
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        StartCoroutine(UpdateSelector(buttons[selectedValue].transform));
        buttons[selectedValue].AnimateButton(true);
        buttons[previousValue].AnimateButton(false);

        this.selectedValue = selectedValue;
    }

    public override void AnimatePanel(ScriptableObject selectedObject, float animationDuration = 0.15f)
    {
        StartCoroutine(AnimatePanel(verticalPanel, selectedObject, animationDuration));
    }

    protected override IEnumerator AnimatePanel(Transform panel, ScriptableObject selectedObject = null, float animationDuration = 0.15F)
    {
        SetObjectDefinitionsFromPanel(panel);

        yield return null;

        StartCoroutine(base.AnimatePanel(panel, selectedObject, animationDuration));
    }

    private IEnumerator AnimateSubMenu(Item item, bool isActive, float delay = 0.03f)
    {
        float opacity = isActive ? 1f : 0f;

        if (!isActive)
        {
            StartCoroutine(ActivateButtons(item, opacity, MaxObjects));
            yield return new WaitForSecondsRealtime(delay * buttons.Count);
        }

        informationAnimator.SetBool("isActive", isActive);
        yield return null; yield return new WaitForSecondsRealtime(informationAnimator.GetAnimationTime());

        if (isActive)
        {
            StartCoroutine(ActivateButtons(item, opacity, MaxObjects));
            UpdateSelectedObject(0);
        }
    }

    private IEnumerator ActivateButtons(Item item, float opacity, int maxObjects, float animationDuration = 0.1f, float delay = 0.03f)
    {
        if (opacity == 1)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }

            if (maxObjects != buttons.Count)
            {
                for (int i = maxObjects; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        for (int i = 0; i < maxObjects; i++)
        {
            if (item != null)
            {
                buttons[i].SetValues(item.Behavior[i].buttonName, item.Behavior[i].iconSprite);

                if (opacity == 1)
                {
                    ApplyButtonStyles(item, i);
                }
            }

            buttons[i].FadeButton(opacity, animationDuration);

            yield return new WaitForSecondsRealtime(delay);
        }

        if (opacity == 0)
        {
            for (int i = 0; i < maxObjects; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonPanel);
    }

    private void ApplyButtonStyles(Item item, int index)
    {
        if (index == item.Behavior.FindIndex(b => b.buttonName == "Favorite"))
        {
            Color color = item.IsFavorite ? tulipTreeColor : Color.white;

            buttons[index].transform.Find("Big Icon/Icon").GetComponent<Image>().color = color;
            buttons[index].transform.Find("Small Icon/Icon").GetComponent<Image>().color = color;
        }
        else if (index == item.Behavior.FindIndex(b => b.buttonName == "Discard"))
        {
            float xCoordinate = buttons[index].transform.position.x - 30f;

            quantitySelector.ToggleSelector(false, null, xCoordinate);
        }
    }

    private IEnumerator ResetSubMenu(int selectedValue, float animationDuration = 0.15f)
    {
        StartCoroutine(UpdateSelector());
        buttons[selectedValue].AnimateButton(false);

        yield return new WaitForSecondsRealtime(animationDuration);

        StartCoroutine(AnimateSubMenu(null, false));
    }

    private void SetObjectDefinitionsFromPanel(Transform panel)
    {
        if (nameText != panel.Find("Name/Value").GetComponent<TextMeshProUGUI>())
        {
            nameText = panel.Find("Name/Value").GetComponent<TextMeshProUGUI>();
            descriptionText = panel.Find("Description/Value").GetComponent<TextMeshProUGUI>();

            effectTypeText = panel.Find("Effect/Type").GetComponent<TextMeshProUGUI>();
            effectQuantityText = panel.Find("Effect/Quantity").GetComponent<TextMeshProUGUI>();
            effectArrow = panel.Find("Effect/Arrow").gameObject;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        informationAnimator = transform.GetComponent<Animator>();

        verticalPanel = transform.Find("Information (Vertical)");
        horizontalPanel = transform.Find("Information (Horizontal)");

        InformationPanel = verticalPanel.transform;

        valueText = horizontalPanel.transform.Find("Amount/Value").GetComponent<TextMeshProUGUI>();
        spriteImage = horizontalPanel.transform.Find("Name/Icon").GetComponent<Image>();

        buttonPanel = horizontalPanel.transform.Find("Buttons").GetComponent<RectTransform>();
        buttons = buttonPanel.GetComponentsInChildren<MenuButton>().ToList();

        quantitySelector = horizontalPanel.Find("Quantity Selector").GetComponent<QuantitySelectorUserInterface>();

        selectorI = buttonPanel.Find("Selector").gameObject;

        base.Awake();

        StartCoroutine(UpdateSelector());
    }

    #endregion
}
