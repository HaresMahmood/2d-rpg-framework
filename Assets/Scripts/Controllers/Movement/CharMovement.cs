using UnityEngine;

/// <summary>
/// Randomly moves NPC to tile, making 
/// sure they stay within the set boundary.
/// 
/// Inherits from MovingObject.
/// </summary>
public class CharMovement : MonoBehaviour
{
    /*
    #region Variables

    private RangeHandler rangeHandler;

    /// <summary>
    /// The boundary within which NPC can move.
    /// </summary>
    [UnityEngine.Header("Setup")]
    [Tooltip("The boundary within which NPC can move.")]
    public Collider2D bounds;

    /// <summary>
    /// Minimum and maximum values from which a random decision value is taken.
    /// </summary>
    [UnityEngine.Header("Settings")]
    [Tooltip("Minimum and maximum values from which a random decision value is taken.")]
    public Vector2 decisionTime = new Vector2(0, 1.5f);
    private float decisionTimeCount;

    /// <summary>
    /// Direction in which NPC will move.
    /// </summary>
    [HideInInspector] public Vector3 direction;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// 
    /// Overrides Start function from the MovingObject base-class.
    /// </summary>
    protected override void Start()
    {
        rangeHandler = gameObject.transform.GetComponentInChildren<RangeHandler>();

        TilemapManager.instance.GetTilemaps(this.gameObject.scene.GetRootGameObjects(), groundTiles, obstacleTiles);

        moveTime = walkTime; // Default move-time is set at the beginning.

        // Choose a random time delay for taking a decision (changing direction, or standing in place for a while).
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
        ChangeDirection();

        base.Start(); // Calls the Start function of the MovingObject base-class.
    }

    /// <summary>
    /// Update is called once per frame.
    /// 
    /// Overrides Update function from the MovingObject base-class.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (!canMove || isMoving || onCoolDown || onExit) return; // We wait until NPC is done moving or while the game is paused. || PauseUserInterfaceController.instance.flags.isActive

        if (direction == Vector3.zero)
        {
            isMoving = false;
            SetMoveAnimations();
        }
        else
        {
            SetMoveAnimations();
            SetAnimations((int)direction.x, (int)direction.y);
        }

        if (!rangeHandler.playerInRange && canMove) // If Player is not in range and NPC is able to move, ...
        {
            if (bounds.bounds.Contains(transform.position + direction)) // If target-tile is withing the set boundary.
            {
                StartCoroutine(CoolDown(moveTime)); // Starts cool-down timer.
                AttemptMove((int)direction.x, (int)direction.y); // Moves NPC into direction if possible.

                CheckDecisionTime();
            }
            else
                CheckDecisionTime();
        }
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void CheckDecisionTime()
    {
        SetMoveAnimations();

        if (decisionTimeCount > 0)
            decisionTimeCount -= 0.1f + Time.deltaTime;
        else
        {
            // Choose a random time delay for taking a decision (changing direction, or standing in place for a while).
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
            ChooseNewDirection(); // Choose a movement direction, or stay in place.
        }
    }

    /// <summary>
    /// Chooses a random new direction until an 
    /// arbitrary timer is up or a completely new
    /// direction has been choses.
    /// </summary>
    private void ChooseNewDirection()
    {
        isMoving = false;

        Vector3 currentDirection = direction;
        ChangeDirection();

        int i = 0;
        while (currentDirection == direction && i < 100)
        {
            ChangeDirection();
            i++;
        }
    }

    /// <summary>
    /// Chooses a random direction for the NPC to move in,
    /// or ensures NPC stays in place.
    /// </summary>
    void ChangeDirection()
    {
        int orientation = Random.Range(0, 6);

        switch (orientation)
        {
            case 0:
                direction = Vector3.down;  // Down
                break;
            case 1:
                direction = Vector3.left; // Left
                break;
            case 2:
                direction = Vector3.right; // Right
                break;
            case 3:
                direction = Vector3.up; // Up
                break;
            case 4:
                direction = Vector3.zero; // Stay in place;
                break;
            case 5:
                direction = Vector3.zero; // Stay in place;
                break;
            case 6:
                direction = Vector3.zero; // Stay in place;
                break;
            default:
                break;
        }
    }
    */
}