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
    [SerializeField] [Range(0.1f, 5f)] protected float walkTime = 1f;

    /// <summary>
    /// Lists that keep track of Tilemaps. These can
    /// be dynamically updated depending on the scene Character is in.
    /// </summary>
    public static List<Tilemap> obstacleTiles = new List<Tilemap>(); // obstacleTiles

    /// <summary>
    /// Determines the state of movement of Character.
    /// </summary>
    private bool canMove = true, isMoving = false, onCoolDown = false, onGround, hasGround, hasObstacle, hasChar;

    /// <summary>
    /// Collider2D attached to Character.
    /// </summary>
    private new Collider2D collider;

    /// <summary>
    /// Animator attached to Character.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Orientation of Character.
    /// </summary>
    private Vector2 orientation;

    #endregion

    #region Properties



    #endregion

    #region Miscellaneous Methods

    /// <summary>
    /// Returns true if the Collider2D of Character collides with that of 
    /// another character.
    /// </summary>
    /// <param name="moveCollider"> Collider of the character that is being checked. </param>
    /// <param name="direction"> Direction Character is facing towards. </param>
    /// <param name="distance"> Maximum distance the character that is being checked can be located. </param>
    /// <returns> Boolean. </returns>
    public bool GetCollider(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[5]; // Array to store all detected colliders withing set distance.
            ContactFilter2D contactFilter = new ContactFilter2D // Filters object that are only on the Obstacles sorting layer...
            {
                useLayerMask = true,
                layerMask = LayerMask.NameToLayer("Obstacles")
            };

            int numHits = moveCollider.Cast(direction, contactFilter, hits, distance); // Casts a RayCast in the direction and for the distance specified.

            for (int i = 0; i < numHits; i++)
            {
                if (!hits[i].collider.isTrigger && hits[i].transform.CompareTag("Collidable")) // If the particular collider is not a Trigger, ...
                {
                    return true; // Hit blocking collision
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if Character can move to target-tile.
    /// </summary>
    /// <param name="xCoordinate"> X-coordinate of the target-direction. </param>
    /// <param name="yCoordinate"> Y-coordinate of the target-direction. </param>
    protected virtual void AttemptMove(int xCoordinate, int yCoordinate)
    {
        Vector2 startPosition = transform.position; // Starting position of Character.
        Vector2 direction = new Vector2(xCoordinate, yCoordinate);
        Vector2 targetPosition = startPosition + direction; // Target-tile Character wants to move to.

        /*
        foreach (Tilemap tile in obstacleTiles)
        {
            if (GetTile(tile, targetPosition) != null) // If target-tile is a obstacle-tile, ...
            {
                Debug.Log("Encountered obstacle");
                return;
            }
        }
        */

        if (GetCollider(collider, direction, 1f))
        {
            return;
        }

        StartCoroutine(Move(targetPosition)); // Moves Character to target-tile.
    }

    /// <summary>
    /// Sets the correct direction in the Animator for Character.
    /// </summary>
    protected virtual void SetMoveAnimations()
    {
        if (isMoving)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    /// <summary>
    /// Moves Character to the center of target-tile.
    /// </summary>
    /// <param name="end"> End-position of Character. </param>
    protected IEnumerator Move(Vector3 end)
    {
        float remainingDistance = (transform.position - end).sqrMagnitude; // Calculates squared magnitude of the remaining distance, since it is much faster.
        float inverseMoveTime = 1 / walkTime; // Calculates the inverse move-time, since it is mathematically "cheaper" to divide than to multiply.

        isMoving = true; // Character is now moving.

        while (remainingDistance > float.Epsilon) // Compares the remaining distance to a very small value.
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime); // Calculate a position between the points specified to get the new position.

            transform.position = newPosition; // Sets current position to new position.
            remainingDistance = (transform.position - end).sqrMagnitude; // Calculates new remaining distance.

            yield return null;
        }

        isMoving = false; // Once player has reached the new position, it is not moving anymore.

        /*
        isMoving = true; // Character is now moving.

        Sequence sequence = DOTween.Sequence();

        transform.DOMove(end, walkTime).SetEase(Ease.Linear);

        sequence.OnComplete(() =>
        {
            isMoving = false;
        });
        */
    }

    /// <summary>
    ///  Sets the walk and run animations for Character and
    ///  sets the appriopriate orientation values of Character.
    /// </summary>
    protected void SetAnimations(int xCoordinate, int yCoordinate)
    {
        animator.SetFloat("moveX", xCoordinate);
        animator.SetFloat("moveY", yCoordinate);

        orientation = new Vector2(xCoordinate, yCoordinate);
    }

    /// <summary>
    /// Returns the Tilemap at the specified position.
    /// </summary>
    /// <param name="tilemap"> Tilemap that needs to be checked. </param>
    /// <param name="pos"> Position of Tilemap. </param>
    /// <returns> TileBase. </returns>
    protected TileBase GetTile(Tilemap tilemap, Vector2 pos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(pos));
    }

    private IEnumerator ChangeOrientation(int horizontal, int vertical, float duration)
    {
        SetAnimations(horizontal, vertical); // Sets direction the player is facing in, based on input.
        isMoving = true;
        SetMoveAnimations();

        yield return new WaitForSeconds(duration);

        isMoving = false;
        SetMoveAnimations();
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponentInChildren<Collider2D>();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Update()
    {
        Debug.Log(isMoving);

        if (!isMoving)
        {
            SetMoveAnimations(); // Turns all move animations off.
        }

        if (!canMove || isMoving) return; // We wait until Player is done moving or while game is paused. || PauseUserInterfaceController.instance.flags.isActive

        // To store the direction in which Player wants to move.
        int horizontal = 0;
        int vertical = 0;

        // To get move directions.
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        // We can't go diagonally, or in both directions at the same time.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) // If there is an input, ...
        {
            if (!orientation.Equals(new Vector2(horizontal, vertical)))
            {
                StartCoroutine(ChangeOrientation(horizontal, vertical, walkTime));
                return;
            }

            if (canMove) // If Player is able to move, ...
            {
                SetAnimations(horizontal, vertical); // Sets direction the player is facing in, based on input.

                AttemptMove(horizontal, vertical); // Moves Player if possible.

                SetMoveAnimations(); // Sets animations based on if Player is walking or running.
            }
        }
        else
        {
            SetMoveAnimations(); // Turns all move animations off.
        }
    }

    // Debug
    /*
    if (PauseManager.instance.flags.isActive)
    {
        canMove = false;
    }
    */

    #endregion
}