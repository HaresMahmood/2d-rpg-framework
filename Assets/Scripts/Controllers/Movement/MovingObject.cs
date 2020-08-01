using UnityEngine;

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

    #endregion

    #region Properties

    public Vector3 Orienation { get { return orientation; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    #endregion

    #region Miscellaneous Methods

    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f) //  && !IsTappingButton()
        {
            if (!IsTappingButton())
            {
                GetInput();
            }
        }
        else
        {
            animator.SetBool(animatedMovement, true);
        }
    }

    protected abstract void GetInput();

    protected virtual bool GetInput(Vector3 orientation)
    {
        if (NoCollision(orientation))
        {
            movePoint.position += orientation;

            ChangeOrienation(orientation.x, orientation.y);

            return true;
        }
        else
        {
            DisableMovement();
        }

        return false;
    }

    protected virtual bool NoCollision(Vector3 orientation) // TODO: Bad name
    {
        return !Physics2D.OverlapCircle(movePoint.position + orientation, radius, collisionLayer);
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
        animator.SetFloat("moveX", horizontal);
        animator.SetFloat("moveY", vertical);

        orientation = new Vector3(horizontal, vertical);
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
            Move();
        }
    }

    #endregion
}