using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

/// <summary>
/// Defines collision for Tilemaps and Characters and
/// defines grid-based movement for Characters.
/// </summary>
public abstract class MovingObject : MonoBehaviour
{
    #region Variables

    [Header("Settings")]
    [SerializeField] [Range(2f, 15f)] protected float moveSpeed = 3f;
    [SerializeField] [Range(0.1f, 2f)] protected float radius = 0.1f;
    [SerializeField] protected LayerMask collisionLayer;

    /// <summary>
    /// Animator attached to Character.
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// Orientation of Character.
    /// </summary>
    private Vector2 orientation;

    protected Transform movePoint;

    protected string animatedMovement = "isWalking";

    protected bool canMove = true;

    #endregion

    #region Properties



    #endregion

    #region Miscellaneous Methods

    protected virtual bool GetInput(float horizontal, float vertical)
    {
        if (Mathf.Abs(horizontal) == 1f)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(horizontal, 0f, 0f), radius, collisionLayer))
            {
                movePoint.position += new Vector3(horizontal, 0f, 0f);
            }

            animator.SetFloat("moveX", horizontal);
            animator.SetFloat("moveY", vertical);

            return true;
        }
        else if (Mathf.Abs(vertical) == 1f)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, vertical, 0f), radius, collisionLayer))
            {
                movePoint.position += new Vector3(0f, vertical, 0f);
            }

            animator.SetFloat("moveX", horizontal);
            animator.SetFloat("moveY", vertical);

            return true;
        }
        else
        {
            DisableMovement();
        }

        return false;
    }

    protected virtual bool IsTappingButton()
    {
        return false;
    }

    protected virtual void DisableMovement()
    {
        animator.SetBool(animatedMovement, false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        movePoint = transform.Find("Move Point");
        movePoint.parent = null;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f) //  && !IsTappingButton()
            {
                if (!IsTappingButton())
                {
                    GetInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
            }
            else
            {
                animator.SetBool(animatedMovement, true);
            }
        }
    }

    #endregion
}