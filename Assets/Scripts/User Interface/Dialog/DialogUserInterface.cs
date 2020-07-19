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

    private TextFade dialogText;
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
        dialogText.FadeTo(text);
    }

    /*
    /// <summary>
    /// Method to animate vertex colors of a TMP Text object.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateText()
    {
        // Need to force the text object to be generated so we have valid data to work with right from the start.
        dialogText.ForceMeshUpdate();


        TMP_TextInfo textInfo = dialogText.textInfo;
        Color32[] newVertexColors;

        int currentCharacter = 0;
        int startingCharacterRange = currentCharacter;
        bool isRangeMax = false;

        while (!isRangeMax)
        {
            int characterCount = textInfo.characterCount;

            // Spread should not exceed the number of characters.
            byte fadeSteps = (byte)Mathf.Max(1, 255 / 5); // TODO Make serializable (5)


            for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
            {
                // Skip characters that are not visible
                //if (textInfo.characterInfo[i].isVisible) continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Get the current character's alpha value.
                byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);

                // Set new alpha values.
                newVertexColors[vertexIndex + 0].a = alpha;
                newVertexColors[vertexIndex + 1].a = alpha;
                newVertexColors[vertexIndex + 2].a = alpha;
                newVertexColors[vertexIndex + 3].a = alpha;

                // Tint vertex colors
                // Note: Vertex colors are Color32 so we need to cast to Color to multiply with tint which is Color.
                newVertexColors[vertexIndex + 0] = (Color)newVertexColors[vertexIndex + 0] * ColorTint;
                newVertexColors[vertexIndex + 1] = (Color)newVertexColors[vertexIndex + 1] * ColorTint;
                newVertexColors[vertexIndex + 2] = (Color)newVertexColors[vertexIndex + 2] * ColorTint;
                newVertexColors[vertexIndex + 3] = (Color)newVertexColors[vertexIndex + 3] * ColorTint;

                if (alpha == 255)
                {
                    startingCharacterRange += 1;

                    if (startingCharacterRange == characterCount)
                    {
                        // Update mesh vertex data one last time.
                        dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                        yield return new WaitForSeconds(1.0f);

                        // Reset the text object back to original state.
                        //dialogText.ForceMeshUpdate();

                        yield return new WaitForSeconds(1.0f);

                        // Reset our counters.
                        currentCharacter = 0;
                        startingCharacterRange = 0;
                        isRangeMax = true; // Would end the coroutine.
                    }
                }
            }

            // Upload the changed vertex colors to the Mesh.
            dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            if (currentCharacter + 1 < characterCount) currentCharacter += 1;

            yield return new WaitForSeconds(0.25f - 0.01f * 0.01f); // TODO: Make serializable (0.1f)
        }
    }
    */

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        animator = GetComponent<Animator>();

        dialogText = transform.Find("Text").GetComponent<TextFade>();
        nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();

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

