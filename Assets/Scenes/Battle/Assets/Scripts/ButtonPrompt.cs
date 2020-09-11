using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPrompt : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private List<Prompt> prompts = new List<Prompt>();

    private TextMeshProUGUI text;
    private Image icon;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(int i)
    {
        text.SetAutoTextWidth(prompts[i].text);

        icon.sprite = prompts[i].icon;
        icon.gameObject.SetActive(prompts[i].icon != null);
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        text = transform.Find("Button/Binding").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Button/Icon").GetComponent<Image>();
    }

    private void Start()
    {
        SetInformation(0);
    }

    #endregion

    #region Nested Class

    [System.Serializable]
    internal class Prompt
    {
        #region Fields

        [SerializeField] internal string text;
        [SerializeField] internal Sprite icon;

        #endregion
    }

    #endregion
}
