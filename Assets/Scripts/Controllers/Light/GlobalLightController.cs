using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class GlobalLightController : MonoBehaviour
{
    #region Variables



    #endregion

    #region Miscellaneous Methods

    public void FadeLight(Color color, float duration, float intensity = -1)
    {
        StopAllCoroutines();
        StartCoroutine(gameObject.FadeColor(color, duration));
        if (intensity > -1 && intensity != GetComponent<Light>().intensity)
        {
            StartCoroutine(GetComponent<Light>().FadeLight(intensity, duration));
        }
    }

    public IEnumerator FlashLight(float intensity, float duration, int repetitions)
    {
        float startIntensity = GetComponent<Light>().intensity;
        int counter = 0;
        while (counter < repetitions)
        {
            FadeLight(GetComponent<Light>().color, (duration / 2), intensity);
            yield return new WaitForSeconds((duration / 2));
            FadeLight(GetComponent<Light>().color, (duration), startIntensity);
            yield return new WaitForSeconds((duration));

            counter++;
        }
    }

    #endregion

    #region Unity Methods

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
