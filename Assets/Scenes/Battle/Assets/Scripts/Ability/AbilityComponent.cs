using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class AbilityComponent : MonoBehaviour
{
    #region Unity Methods
    
    private void OnEnable()
    {
        GetComponentInChildren<TextMeshProUGUI>().alignment = GetComponent<HorizontalLayoutGroup>().reverseArrangement ? TextAlignmentOptions.Right : TextAlignmentOptions.Left; 

        transform.Find("Arrow").eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            Convert.ToInt32(GetComponent<HorizontalLayoutGroup>().reverseArrangement) * -180
        );
    }

    #endregion
}

