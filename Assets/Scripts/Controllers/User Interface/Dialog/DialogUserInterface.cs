using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class DialogUserInterface : MonoBehaviour
{
    #region Variables

    private Animator animator;

    private TextMeshProUGUI dialogText;
    private TextMeshProUGUI nameText;

    private GameObject indicator;

    #endregion

    #region Miscellaneous Methods

    public void UpdateInformation(string text, string name)
    {
        nameText.SetText(name);
        SetText(text);
    }

    public void SetText(string text)
    {
        // http://digitalnativestudios.com/forum/index.php?topic=1182.0

        dialogText.SetText(text);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();

        dialogText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        dialogText = transform.Find("Name").GetComponent<TextMeshProUGUI>();

        indicator = transform.Find("Selector").gameObject;
        indicator.SetActive(false);
    }


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    #endregion
}

