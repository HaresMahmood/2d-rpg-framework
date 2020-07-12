using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class SidebarSlot : Slot
{
    #region Variables

    private Animator animator;

    private Image sprite;
    private Image heldItem;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;

    private Slider hpBar;

    #endregion

    #region Miscellaneous Methods

    public void AnimateSlot(bool isActive)
    {
        animator.SetBool("isActive", isActive);
    }

    public void DeactivateSlot()
    {
        //base.Awake();
        StartCoroutine(gameObject.FadeOpacity(0f, 0.1f));
    }

    protected override void SetInformation<T>(T slotObject)
    {
        PartyMember member = (PartyMember)Convert.ChangeType(slotObject, typeof(PartyMember));

        float hp = (float)member.Stats.HP / (float)member.Stats.Stats[Pokemon.Stat.HP];
        string color = hp >= 0.5f ? "#67FF8F" : (hp >= 0.25f ? "#FFB766" : "#FF7766");

        if (GetComponent<CanvasGroup>().alpha == 0)
        {
            StartCoroutine(gameObject.FadeOpacity(1f, 0.1f));
        }

        sprite.sprite = member.Species.Sprites.MenuSprite;

        if (heldItem.sprite != null)
        {
            heldItem.GetComponent<CanvasGroup>().alpha = 1;
            heldItem.sprite = member.HeldItem.Sprite;
        }
        else
        {
            heldItem.GetComponent<CanvasGroup>().alpha = 0;
        }

        nameText.SetText(member.Nickname != "" ? member.Nickname : member.Species.Name);
        levelText.SetText(member.Progression.Level.ToString());

        hpBar.value = hp;
        hpBar.fillRect.GetComponent<Image>().color = color.ToColor();
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        animator = GetComponent<Animator>();

        sprite = transform.Find("Sprite").GetComponent<Image>();
        heldItem = transform.Find("Held Item").GetComponent<Image>();

        nameText = transform.Find("Information/Name").GetComponent<TextMeshProUGUI>();
        levelText = transform.Find("Information/Level/Value").GetComponent<TextMeshProUGUI>();

        hpBar = transform.Find("Health Bar").GetComponent<Slider>();

    }

    #endregion

    /*
    #region Variables

    private Transform slot;
    private GameObject informationContainer;

    private Image menuSprite;
    private Image itemSprite;
    private Slider healthBar;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;

    #endregion

    #region Miscellaneous Methods

    private void ResetSlot()
    {
        informationContainer.SetActive(false);
        menuSprite.gameObject.SetActive(false);
        itemSprite.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);

        UpdateBaseColor(false);
    }

    private void UpdateHealthbarColor()
    {
        float value = healthBar.value;
        string color = value > 0.5f ? "#03fc4e" : (value > 0.25f ? "#FFB600" : "#FF1300"); // Green, yellow/orange and red respectively

        StartCoroutine(healthBar.fillRect.gameObject.FadeColor(color.ToColor(), 0.15f));
    }

    private void UpdateBaseColor(bool isFainted)
    {
        string color = isFainted ? "#FFFFFF" : "#FF1300"; // White and red respectively
        StartCoroutine(slot.gameObject.FadeColor(color.ToColor(), 0.15f));
    }

    public void PopulateSlot(Pokemon pokemon)
    {
        //PopulateSlot(pokemon.menuSprite, pokemon.heldItem.sprite, pokemon.totalHealth, pokemon.stats.health, pokemon.name, pokemon.level);
    }

    private void PopulateSlot(Sprite menuSprite, Sprite itemSprite, float totalHealth, float health, string name, int level)
    {
        this.menuSprite.sprite = menuSprite;
        this.itemSprite.sprite = itemSprite;

        if (health > 0)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.value = (health / totalHealth);
            UpdateHealthbarColor();
        }
        else
        {
            UpdateBaseColor(true);
            healthBar.gameObject.SetActive(false);
        }

        nameText.SetText(name);
        levelText.SetText(level.ToString());

        informationContainer.gameObject.SetActive(true);
        this.menuSprite.gameObject.SetActive(true);
        this.itemSprite.gameObject.SetActive(true);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        slot = transform;

        menuSprite = slot.Find("Pokémon").GetComponent<Image>();
        itemSprite = slot.Find("Held Item").GetComponent<Image>();
        healthBar = slot.Find("Health Bar").GetComponent<Slider>();

        informationContainer = slot.Find("Information").gameObject;

        nameText = informationContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        levelText = informationContainer.transform.Find("Level/Value").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        informationContainer.SetActive(false);
        menuSprite.gameObject.SetActive(false);
        itemSprite.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
    */
}
