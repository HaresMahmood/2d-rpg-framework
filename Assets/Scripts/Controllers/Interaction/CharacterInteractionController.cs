using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(CharacterMovement))]
public class CharacterInteractionController : InteractableObject
{
    #region Fields

    [Header("Setup")]
    [SerializeField] private Dialog dialog;

    #endregion

    #region Miscellaneous Methods

    public override void Interact(Vector3 orienation)
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

