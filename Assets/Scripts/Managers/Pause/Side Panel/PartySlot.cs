using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class PartySlot : MonoBehaviour
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

    public void PopulateSlot(Pokemon pokemon)
    {
        PopulateSlot(pokemon.menuSprite, pokemon.heldItem.sprite, pokemon.totalHealth, pokemon.stats.health, pokemon.name, pokemon.level);
    }

    private void PopulateSlot(Sprite menuSprite, Sprite itemSprite, float totalHealth, float health, string name, int level)
    {
        this.menuSprite.sprite = menuSprite;
        this.itemSprite.sprite = itemSprite;

        healthBar.value = (health / totalHealth);

        nameText.SetText(name);
        levelText.SetText(level.ToString());

        informationContainer.gameObject.SetActive(true);
        this.menuSprite.gameObject.SetActive(true);
        this.itemSprite.gameObject.SetActive(true);
        healthBar.gameObject.SetActive(true);
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
