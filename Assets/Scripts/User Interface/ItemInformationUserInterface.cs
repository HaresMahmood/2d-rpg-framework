using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class ItemInformationUserInterface : CategorizableInformationUserInterface
{
    #region Constants

    public override int MaxObjects => selectedItem.Behavior.Count;

    private readonly Color tulipTreeColor = "#EAC03E".ToColor();

    #endregion

    #region Fields

    private static ItemInformationUserInterface instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static ItemInformationUserInterface Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemInformationUserInterface>();
            }

            return instance;
        }
    }

    #endregion

    #region Variables

    private List<MenuButton> buttons = new List<MenuButton>();

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

    #endregion

    #region Static Methods

    public static void Favorite(Item item)
    {
        Instance.ToggleFavorite(item);
    }

    #endregion

    #region Behavior Definitions

    private void ToggleFavorite(Item item, float animationDuration = 0.1f)
    {
        Color color = item.IsFavorite ? "#EAC03E".ToColor() : Color.white;
        int index = item.Behavior.FindIndex(b => b.buttonName == "Favorite");

        item.IsFavorite = !item.IsFavorite;

        StartCoroutine(buttons[index].transform.Find("Big Icon/Icon").gameObject.FadeColor(color, animationDuration));
        StartCoroutine(buttons[index].transform.Find("Small Icon/Icon").gameObject.FadeColor(color, animationDuration));
    }

    #endregion

    #region Miscellaneous Methods

    public void ToggleSubMenu(Item item, bool isActive)
    {
        if (isActive)
        {
            SetObjectDefinitionsFromPanel(horizontalPanel);
            SetValues(item);
            valueText.SetText((item).Quantity.ToString());
            spriteImage.sprite = (item).Sprite;
        }

        selectedItem = item;
        StartCoroutine(ItemInformationController.Instance.SetActive(true));
        StartCoroutine(AnimateSubMenu(item, isActive));
    }

    public void InvokeItemBehavior(int selectedValue)
    {
        selectedItem.Behavior[selectedValue].behaviorEvent.Invoke();;
    }

    public override void SetValues(Categorizable categorizable)
    {
        Color textColor = ((Item)categorizable).Effect.GetQuantity().Equals("") ? "#B0B0B0".ToColor() : GameManager.GetAccentColor();
        bool arrowState = ((Item)categorizable).Effect.GetQuantity().Equals("") ? false : true;

        nameText.SetText(categorizable.Name);
        descriptionText.SetText(categorizable.Description);

        effectTypeText.SetText(((Item)categorizable).Effect.ToString());
        effectQuantityText.SetText(((Item)categorizable).Effect.GetQuantity());

        effectTypeText.GetComponent<AutoTextWidth>().UpdateWidth(((Item)categorizable).Effect.ToString());

        if (!effectTypeText.color.Equals(textColor))
        {
            effectTypeText.color = textColor;
        }

        if (!effectArrow.activeSelf.Equals(arrowState))
        {
            effectArrow.SetActive(arrowState);
        }
    }

    public override void AnimatePanel(Categorizable categorizable, float animationDuration = 0.15f)
    {
        StartCoroutine(AnimatePanel(verticalPanel, categorizable, animationDuration));
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, -increment);

        StartCoroutine(UpdateSelector(buttons[selectedValue].transform));
        buttons[selectedValue].AnimateButton(true);
        buttons[previousValue].AnimateButton(false);
    }

    protected override IEnumerator AnimatePanel(Transform panel, Categorizable categorizable = null, float animationDuration = 0.15F)
    {
        SetObjectDefinitionsFromPanel(panel);

        yield return null;

        StartCoroutine(base.AnimatePanel(panel, categorizable, animationDuration));
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
                if (i == item.Behavior.FindIndex(b => b.buttonName == "Favorite"))
                { 
                    Color color = item.IsFavorite ? tulipTreeColor : Color.white;

                    buttons[i].transform.Find("Big Icon/Icon").GetComponent<Image>().color = color;
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

        informationPanel = verticalPanel.transform;

        valueText = horizontalPanel.transform.Find("Amount/Value").GetComponent<TextMeshProUGUI>();
        spriteImage = horizontalPanel.transform.Find("Name/Icon").GetComponent<Image>();

        buttonPanel = horizontalPanel.transform.Find("Buttons").GetComponent<RectTransform>();
        buttons = buttonPanel.GetComponentsInChildren<MenuButton>().ToList();

        selector = buttonPanel.Find("Indicator").gameObject;

        base.Awake();

        StartCoroutine(UpdateSelector());
    }

    #endregion
}
