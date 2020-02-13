using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class CategoryPanel : MonoBehaviour
{
    #region Variables

    private Transform[] categoryIcons;

    private Transform categoryNameContainer;

    private Animator leftArrowAnimator;
    private Animator rightArrowAnimator;

    #endregion

    #region Miscellaneous Methods

    /// <summary>
    /// Updates the text and position of the selected category's name.
    /// </summary>
    /// <param name="selectedValue">  </param>
    /// <param name="animationDuration">  </param>
    /// <returns> Co-routine. </returns>
    public IEnumerator UpdateCategoryName(int selectedValue, string value, float animationDuration = 0.1f)
    {
        StartCoroutine(categoryNameContainer.gameObject.FadeOpacity(0f, animationDuration));

        yield return new WaitForSecondsRealtime(animationDuration);

        categoryNameContainer.GetComponentInChildren<TextMeshProUGUI>().SetText(value.Replace('_', ' ')); yield return null;
        categoryNameContainer.position = new Vector2(categoryIcons[selectedValue].position.x, categoryNameContainer.position.y);
        StartCoroutine(categoryNameContainer.gameObject.FadeOpacity(1f, animationDuration));
    }

    public void AnimateCategory(int selectedCategory, int increment)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, categoryIcons.Length, -increment);

        AnimateIconPosition(selectedCategory, previousCategory);
        AnimateIconColor(selectedCategory, previousCategory);
        StartCoroutine(AnimateIcon(selectedCategory, previousCategory));
    }

    private void AnimateIconPosition(int selectedCategory, int previousCategory)
    {
        categoryIcons[selectedCategory].GetComponent<Animator>().SetBool("isSelected", true);
        categoryIcons[previousCategory].GetComponent<Animator>().SetBool("isSelected", false);
    }

    private void AnimateIconColor(int selectedCategory, int previousCategory)
    {
        if (categoryIcons[selectedCategory].Find("Icon").GetChildren().Length == 0)
        {
            StartCoroutine(categoryIcons[selectedCategory].GetComponentInChildren<Image>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        }
        else
        {
            foreach (Transform child in categoryIcons[selectedCategory].Find("Icon").GetChildren())
            {
                StartCoroutine(child.gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
            }
        }

        if (categoryIcons[previousCategory].Find("Icon").GetChildren().Length == 0)
        {
            StartCoroutine(categoryIcons[previousCategory].GetComponentInChildren<Image>().gameObject.FadeColor(Color.white, 0.1f));
        }
        else
        {
            foreach (Transform child in categoryIcons[previousCategory].Find("Icon").GetChildren())
            {
                StartCoroutine(child.gameObject.FadeColor(Color.white, 0.1f));
            }
        }
    }

    private IEnumerator AnimateIcon(int selectedCategory, int previousCategory)
    {
        Animator selectedAnimator = categoryIcons[selectedCategory].Find("Icon").GetComponent<Animator>();
        Animator previousAnimator = categoryIcons[previousCategory].Find("Icon").GetComponent<Animator>();

        if (selectedAnimator != null) // Debug.
        {
            if (previousAnimator != null && previousAnimator.GetBool("isSelected"))
            {
                previousAnimator.SetBool("isSelected", false);
            }

            selectedAnimator.SetBool("isSelected", true);
            yield return null; float animationTime = selectedAnimator.GetAnimationTime();
            yield return new WaitForSecondsRealtime(animationTime);
            selectedAnimator.SetBool("isSelected", false);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        categoryIcons = transform.Find("Category Icons").GetChildren();
        categoryNameContainer = transform.Find("Category Name");
        leftArrowAnimator = transform.Find("Left Navigation/Left Arrow").GetComponent<Animator>();
        rightArrowAnimator = transform.Find("Right Navigation/Right Arrow").GetComponent<Animator>();
    }

    #endregion
}
