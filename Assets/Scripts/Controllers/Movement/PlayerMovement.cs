using System.Timers;
using UnityEngine;

/// <summary>
/// Moves Player to tile depending on input.
/// 
/// Inherits from MovingObject.
/// </summary>
public class PlayerMovement : MovingObject
{
    #region Variables

    [SerializeField] [Range(1f, 30f)] private float fidgetDelay = 10f;

    [Header("Values")]
    [SerializeField] [ReadOnly] private float fidgetTimer;
    [SerializeField] [ReadOnly] private bool isRunning;

    private new BoxCollider2D collider;

    private InteractionController interactionController;

    #endregion

    #region Miscellaneous Methods

    protected override void GetInput()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) == 1)
        {
            vertical = 0;
        }

        Vector3 orientation = new Vector3(horizontal, vertical);

        GetInput(orientation);
    }

    protected override bool GetInput(Vector3 orienation)
    {
        bool input = base.GetInput(orienation);

        if (input)
        {
            ResetFidget();
        }

        return input;
    }

    protected override bool IsTappingButton()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            ResetFidget();
            ChangeOrienation(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            return true;
        }

        return base.IsTappingButton();
    }

    protected override void DisableMovement()
    {
        base.DisableMovement();

        EnableFidget();
    }

    protected override void ChangeOrienation(float horizontal, float vertical)
    {
        base.ChangeOrienation(horizontal, vertical);

        collider.offset = new Vector2(horizontal, vertical);
    }

    private void ToggleRun()
    {
        animator.SetBool(animatedMovement, false);

        moveSpeed = isRunning ? 5f : 3f;
        animatedMovement = isRunning ? "isRunning" : "isWalking";
    }

    private void EnableFidget()
    {
        fidgetTimer += Time.deltaTime;

        if (fidgetTimer > fidgetDelay)
        {
            ResetFidget();
            animator.SetTrigger("isFidgeting");
        }
    }

    private void ResetFidget()
    {
        animator.ResetTrigger("isFidgeting");

        fidgetTimer = 0;
    }

    #endregion

    #region Event Methods

    private void InteractionController_OnInteract(object sender, bool condition)
    {
        if (condition)
        {
            canMove = !canMove;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        interactionController = GetComponent<InteractionController>();
        interactionController.OnInteract += InteractionController_OnInteract;

        collider = transform.Find("Interaction Collider").GetComponent<BoxCollider2D>();

        base.Awake();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Input.GetButtonDown("Toggle"))
        {
            isRunning = !isRunning;

            ToggleRun();
        }
        if (Input.GetButtonDown("Run") || Input.GetButtonUp("Run"))
        {
            isRunning = Input.GetButton("Run");

            ToggleRun();
        }

        base.Update();
    }

    #endregion

    /*
    #region Variables

    [SerializeField] [Range(0.1f, 1f)] private float runTime = 0.2f;
    [SerializeField] [Range(1f, 30f)] private float fidgetDelay = 10f;

    [Header("Values")]
    [SerializeField] [ReadOnly] private float fidgetTimer;

    /// <summary>
    /// Used to determine the state of running.
    /// </summary>
    private bool isRunning, toggleRunning;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// 
    /// Overrides Start function from the MovingObject base-class.
    /// </summary>
    protected override void Start()
    {
        SetMoveTimes();

        base.Start(); // Calls the Start function of the MovingObject base-class.
    }

    /// <summary>
    /// Update is called once per frame.
    /// 
    /// Overrides Update function from the MovingObject base-class.
    /// </summary>
    protected override void Update()
    {
        if (Input.GetButtonDown("Toggle"))
        {
            ToggleRunning();
        }

        if (!isMoving)
        {
            SetMoveAnimations(); // Turns all move animations off.
        }

        if (!canMove || isMoving || onCoolDown) return; // We wait until Player is done moving or while game is paused. || PauseUserInterfaceController.instance.flags.isActive

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
            ResetFidget();

            if (!orientation.Equals(new Vector2(horizontal, vertical)) && !isRunning)
            {
                StartCoroutine(ChangeOrientation(horizontal, vertical, walkTime));
                return;
            }

            if (Input.GetButton("Run"))
            {
                isRunning = true;
            }
            else if (!Input.GetButton("Run") && isRunning && !toggleRunning)
            {
                isRunning = false;
            }

            if (canMove) // If Player is able to move, ...
            {
                SetAnimations(horizontal, vertical); // Sets direction the player is facing in, based on input.

                StartCoroutine(CoolDown(moveTime)); // Starts cool-down timer.
                AttemptMove(horizontal, vertical); // Moves Player if possible.

                SetMoveAnimations(); // Sets animations based on if Player is walking or running.
            }
        }
        else
        {
            SetMoveAnimations(); // Turns all move animations off.
            StartCoroutine(EnableFidget());
        }
    }

    #endregion

    private Vector2 GetInput()
    {
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

        return new Vector2(horizontal, vertical);
    }

    private IEnumerator EnableFidget()
    {
        fidgetTimer += Time.deltaTime;

        if (fidgetTimer > fidgetDelay)
        {
            animator.SetBool("isFidgeting", true);

            yield return new WaitForEndOfFrame();
            float waitTime = animator.GetAnimationTime();

            yield return new WaitForSeconds(waitTime);

            animator.SetBool("isFidgeting", false);
            fidgetTimer = 0;
        }
    }

    private void ResetFidget()
    {
        if (animator.GetBool("isFidgeting"))
        {
            animator.SetBool("isFidgeting", false);
        }

        fidgetTimer = 0;
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

    /// <summary>
    /// Used to permanently toggle running, instead of holding
    /// down the "Shift"-key all the time.
    /// </summary>
    /// <returns></returns>
    private void ToggleRunning()
    {
        //if (!DialogManager.instance.isActive)
        //{
        //    isRunning = !isRunning;
        //    toggleRunning = !toggleRunning;
        //}

        SetMoveTimes();
    }

    private void SetMoveTimes()
    {
        if (isRunning)
        {
            moveTime = runTime; // Move-time when running.
        }
        else
        {
            moveTime = walkTime; // Move-time when walking.
        }
    }

    /// <summary>
    /// Sets the walk and run animations for Player. 
    /// 
    /// Overrides SetMoveAnimions function from the MovingObject base-class.
    /// </summary>
    protected override void SetMoveAnimations()
    {
        if (isRunning && isMoving) // If Player is running, ...
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else if (!isMoving) // If Player is standing still, ...
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
        else // If Player is walking, ...
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
    }
    */
}
