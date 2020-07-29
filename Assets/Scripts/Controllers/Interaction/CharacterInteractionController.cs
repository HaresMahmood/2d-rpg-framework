using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(CharacterMovement))]
public class CharacterInteractionController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Dialog dialog;

    #endregion

    #region Properties

    public Dialog Dialog;

    #endregion

    #region Miscellaneous Methods

    public void Interact(Vector3 orienation)
    {
        GetComponent<CharacterMovement>().CanMove = !GetComponent<CharacterMovement>().CanMove;
        GetComponent<CharacterMovement>().SetOrientation(orienation);

        if (!GetComponent<CharacterMovement>().CanMove)
        {
            DialogController.Instance.SetActive(true, dialog.Data[0].LanguageData);
        }
    }

    #endregion
}

