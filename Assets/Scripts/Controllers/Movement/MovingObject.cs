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
    protected Vector3 orientation;

    protected Transform movePoint;

    protected string animatedMovement = "isWalking";

    protected bool canMove = true;

    protected new BoxCollider2D collider;

    #endregion

    #region Properties

    public Vector3 Orienation { get { return orientation; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }

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

            ChangeOrienation(horizontal, vertical);

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

            ChangeOrienation(horizontal, vertical);

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

    protected virtual void ChangeOrienation(float horizontal, float vertical)
    {
        orientation = new Vector3(horizontal, vertical);
        collider.offset = new Vector2(horizontal, vertical);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        movePoint = transform.Find("Move Point");
        movePoint.parent = null;
    }

    /// <summary>
    /// Update is called once per frame.
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