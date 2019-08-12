using System.Collections;
using UnityEngine;
using TMPro;

public class RevealText : MonoBehaviour
{
    public TextMeshProUGUI textThing;
    public int totalVisibleCharacters;
    public int counter;
    public int visibleCount;


    IEnumerator Start()
    {
        textThing = gameObject.GetComponent<TextMeshProUGUI>();

        textThing.ForceMeshUpdate();

        totalVisibleCharacters = textThing.textInfo.characterCount;
        counter = 0;

        while (true)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            textThing.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
                yield return new WaitForSeconds(1f);

            counter += 1;
            yield return new WaitForSeconds(0.05f);
        }
    }
}