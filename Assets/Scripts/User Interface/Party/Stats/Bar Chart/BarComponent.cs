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

    private TextMeshProUGUI evText;
    private TextMeshProUGUI ivText;

    #endregion

    #region Miscellaneous Methods

    public void SetInformation(int ev, int iv)
    {
        this.ev.value = ev / 255f;
        this.iv.value = iv / 31f;

        evText.SetText(ev.ToString());
        ivText.SetText(iv.ToString());
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

        evText = ev.transform.Find("Fill Area/Fill/Value").GetComponent<TextMeshProUGUI>();
        ivText = iv.transform.Find("Fill Area/Fill/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}

