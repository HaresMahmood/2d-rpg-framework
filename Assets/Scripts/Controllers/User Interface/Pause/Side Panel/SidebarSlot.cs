using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class SidebarSlot : MonoBehaviour
{
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
}
