using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class DialogUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => 0;

    #endregion

    #region Variables

    private Animator animator;

    private TextMeshProUGUI dialogText;
    private TextMeshProUGUI nameText;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator ActivatePanel(bool isActive)
    {
        if (isActive)
        {
            gameObject.SetActive(true);
        }

        animator.SetBool("isActive", isActive);

        if (!isActive)
        {
            yield return null;
            yield return new WaitForSeconds(animator.GetAnimationTime());

            gameObject.SetActive(false);
            selector.SetActive(false);
        }
    }

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
    protected override void Awake()
    {
        animator = GetComponent<Animator>();

        dialogText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        dialogText = transform.Find("Name").GetComponent<TextMeshProUGUI>();

        selector = transform.Find("Selector").gameObject;

        base.Awake();

        selector.SetActive(false);
    }


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    #endregion
}

