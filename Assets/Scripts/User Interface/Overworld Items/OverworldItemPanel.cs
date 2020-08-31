using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class OverworldItemPanel : MonoBehaviour
{
    #region Variables

    private Image sprite;

    private TextMeshProUGUI categoryText;
    private TextMeshProUGUI nameText;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(Item item)
    {
        sprite.sprite = item.Sprite;
        categoryText.SetText(item.Categorization.ToString());
        nameText.SetText(item.Name);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        sprite = transform.Find("Base/Information/Icon").GetComponent<Image>();

        categoryText = transform.Find("Base/Information/Text/Category").GetComponent<TextMeshProUGUI>();
        nameText = transform.Find("Base/Information/Text/Name").GetComponent<TextMeshProUGUI>();
    }


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}

