using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class CharacterSpriteController : MonoBehaviour
{
    #region Variables

    private Animator animator;


    #endregion

    #region Miscellaneous Methods

    public void SetAnimation(string activeMenu, string inactiveMenu)
    {
        animator.SetBool(activeMenu, true);
        animator.SetBool(inactiveMenu, false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
