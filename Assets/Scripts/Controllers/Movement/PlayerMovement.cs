using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves Player to tile depending on input.
/// 
/// Inherits from MovingObject.
/// </summary>
public class PlayerMovement : MovingObject
{
    #region Variables

    [SerializeField] [Range(0.1f, 1f)] private float runTime = 0.2f;
    [SerializeField] [Range(1f, 30f)] private float fidgetDelay = 10f;

    [Header("Values")]
    [SerializeField] [ReadOnly] private float fidgetTimer;

    /// <summary>
    /// Used to determine the state of running.
    /// </summary>
    private bool isRunning, toggleRunning;

    private Queue<Vector2> input = new Queue<Vector2>();

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
        base.Update();

        if (Input.GetButtonDown("Toggle"))
        {
            ToggleRunning();
        }

        if (!isMoving)
        {
            SetMoveAnimations(); // Turns all move animations off.
        }

        if (!canMove || isMoving || onCoolDown || onExit || PauseManager.instance.flags.isActive) return; // We wait until Player is done moving or while game is paused.

        if (input.Count > 1)
        {
            input.Dequeue();
        }

        //isRunning = false; // By default, Player is not running.
        canMove = true; // By default, Player is able to move.

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

        input.Enqueue(new Vector2(horizontal, vertical));


        if (horizontal != 0 || vertical != 0) // If there is an input, ...
        {
            ResetFidget();

            if (!orientation.Equals(new Vector2(horizontal, vertical)) && input.Peek().Equals(Vector2.zero) && !isRunning)
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
        if (input.Count > 2)
        {
            input.Dequeue();
        }

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

        input.Enqueue(new Vector2(horizontal, vertical));

        return new Vector2(horizontal, vertical);
    }

    private IEnumerator EnableFidget()
    {
        fidgetTimer += Time.deltaTime;

        if (fidgetTimer > fidgetDelay)
        {
            anim.SetBool("isFidgeting", true);

            yield return new WaitForEndOfFrame();
            float waitTime = anim.GetAnimationTime();

            yield return new WaitForSeconds(waitTime);

            anim.SetBool("isFidgeting", false);
            fidgetTimer = 0;
        }
    }

    private void ResetFidget()
    {
        if (anim.GetBool("isFidgeting"))
        {
            anim.SetBool("isFidgeting", false);
        }

        fidgetTimer = 0;
    }

    private IEnumerator ChangeOrientation(int horizontal, int vertical, float duration)
    {
        /*
        //int counter = 0;
        //while (counter < 10)
        //{
        yield return new WaitForSeconds(0.05f);
        //    counter++;
        //}

        if (!GetInput().Equals(Vector2.zero))
        {
            yield break;
        }
        */

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
        if (!DialogManager.instance.isActive)
        {
            isRunning = !isRunning;
            toggleRunning = !toggleRunning;
        }

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
            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
        }
        else if (!isMoving) // If Player is standing still, ...
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
        }
        else // If Player is walking, ...
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
        }
    }
}
