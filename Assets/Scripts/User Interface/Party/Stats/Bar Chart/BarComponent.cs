using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class BarComponent : MonoBehaviour
{
    #region Variables

    private Slider ev;
    private Slider iv;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(int ev, int iv)
    {
        this.ev.value = ev / 255f;
        this.iv.value = iv / 31f;

        SetValue(this.ev, ev.ToString());
        SetValue(this.iv, iv.ToString());
    }

    private void SetValue(Slider slider, string value)
    {
        slider.transform.Find("Value").GetComponent<TextMeshProUGUI>().SetText(value);
    }

    #endregion
    
    #region Unity Methods
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        ev = transform.Find("EV").GetComponent<Slider>();
        iv = transform.Find("IV").GetComponent<Slider>();
    }

    #endregion
}

