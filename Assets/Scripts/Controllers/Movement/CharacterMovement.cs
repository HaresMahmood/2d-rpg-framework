using System.Timers;
using System.Collections;
using UnityEngine;

/// <summary>
/// Randomly moves NPC to tile, making 
/// sure they stay within the set boundary.
/// 
/// Inherits from MovingObject.
/// </summary>
public class CharacterMovement : MovingObject
{
    #region Variables

    [SerializeField] private MovementType movementType;
    [SerializeField] [ConditionalField("type", false, MovementType.Natural)] private Vector2 idleTime;

    private Collider2D bounds;
    private RangeHandler rangeHandler;

    private Timer timer;

    private Task task;

    #endregion

    #region Enums

    private enum MovementType
    {
        Natural,
        Still,
    }

    #endregion

    #region Miscellaneous Methods

    protected override bool IsTappingButton()
    {
        /*
        int direction = Random.Range(0, 4);

        if (direction == 3 && task != null && !task.Running)
        {
            Debug.Log("Oh yes");

            task = new Task(ChangeOrientation(idleTime));

            return true;
        }
        */

        return false;
    }

    private bool GetInput(Vector3 orientation)
    {
        if (orientation != Vector3.zero && !Physics2D.OverlapCircle(movePoint.position + orientation, radius, collisionLayer) 
            && bounds.bounds.Contains(movePoint.position + orientation) && !rangeHandler.IsPlayerInRange
            && movementType != MovementType.Still)
        {
            movePoint.position += orientation;

            animator.SetFloat("moveX", orientation.x);
            animator.SetFloat("moveY", orientation.y);

            return true;
        }
        else
        {
            DisableMovement();

            if (task != null && !task.Running)
            {
                task = new Task(ChangeOrientation(idleTime));
            }
        }

        return false;
    }

    /// <summary>
    /// Chooses a random direction for the NPC to move in,
    /// or ensures NPC stays in place.
    /// </summary>
    private Vector3 ChangeOrientation()
    {
        // Debug
        System.Random rnd = new System.Random();
        int direction = rnd.Next(0, 5);
        Vector3 orientation = new Vector3();

        switch (direction)
        {
            case 0:
                orientation = Vector3.down;  // Down
                break;
            case 1:
                orientation = Vector3.left; // Left
                break;
            case 2:
                orientation = Vector3.right; // Right
                break;
            case 3:
                orientation = Vector3.up; // Up
                break;
            case 4:
                orientation = Vector3.zero; // Stay in place;
                break;
            default:
                break;
        }

        return orientation;
    }

    private IEnumerator ChangeOrientation(Vector2 idleTimer)
    {
        timer.Stop();

        Vector3 orientation = ChangeOrientation();

        animator.SetFloat("moveX", orientation.x);
        animator.SetFloat("moveY", orientation.y);

        float rand = Random.Range(idleTime.x, idleTime.y);
        //Debug.Log(orientation);
        yield return new WaitForSeconds(rand);

        timer.Start();

        this.orientation = orientation;
    }


    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        //System.Random rnd = new System.Random();
        //int interval = rnd.Next((int)idleTime.x, (int)idleTime.y) * 1000;
        //Debug.Log(interval);
        //timer.Interval = interval;

        orientation = ChangeOrientation();
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        bounds = transform.parent.Find("Bounds").GetComponent<Collider2D>();
        rangeHandler = GetComponent<RangeHandler>();

        task = new Task(null);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        orientation = ChangeOrientation();

        timer = new Timer();
        
        timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        timer.Interval = Random.Range(idleTime.x, idleTime.y) * 1000;
        timer.Enabled = true;
    }

    // TODO: Debug
    protected override void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f) //  && !IsTappingButton()
            {
                if (!IsTappingButton())
                {
                    GetInput(orientation);
                }
            }
            else
            {
                animator.SetBool(animatedMovement, true);
            }
        }
    }

    #endregion

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