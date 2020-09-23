using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class GenderUserInterface : ComponentUserInterface
{
    #region Properties

    private new List<Transform> iconList;

    #endregion

    #region Miscellaneous Methods

    public void UpdateUserInterface(PartyMember.MemberGender.Gender gender)
    {
        if (gender == PartyMember.MemberGender.Gender.None)
        {
            iconList[0].gameObject.SetActive(false);
            iconList[1].gameObject.SetActive(false);
        }

        iconList[(int)gender - 1].gameObject.SetActive(true);
        iconList[ExtensionMethods.IncrementInt((int)gender - 1, 0, 2, 1)].gameObject.SetActive(false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        iconList = transform.GetChildren().ToList();
    }

    #endregion
}

