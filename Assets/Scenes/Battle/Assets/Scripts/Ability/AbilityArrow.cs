using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class AbilityArrow : MonoBehaviour
{
    #region Unity Methods
    
    private void Awake()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            Convert.ToInt32(GetComponentInParent<HorizontalLayoutGroup>().reverseArrangement) * -180
        );
    }

    #endregion
}

