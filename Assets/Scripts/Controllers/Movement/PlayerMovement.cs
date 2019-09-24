using UnityEngine;

/// <summary>
/// Moves Player to tile depending on input.
/// 
/// Inherits from MovingObject.
/// </summary>
public class PlayerMovement : MovingObject
{
    #region Variables
    /// <summary>
    /// Used to determine the state of running.
    /// </summary>
    private bool isRunning, toggleRunning;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Update is called once per frame.
    /// 
    /// Overrides Update function from the MovingObject base-class.
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Toggle"))
            ToggleRunning();

        if (!isMoving)
            SetMoveAnimations(); // Turns all move animations off.

        if (!canMove || isMoving || onCoolDown || onExit) return; // We wait until Player is done moving.

        //isRunning = false; // By default, Player is not running.
        canMove = true; // By default, Player is able to move.

        // To store the direction in which Player wants to move.
        int horizontal = 0;
        int vertical = 0;

        // To get move directions.
        horizontal = (int)(Input.GetAxis("Horizontal"));
        vertical = (int)(Input.GetAxis("Vertical"));

        // We can't go diagonally, or in both directions at the same time.
        if (horizontal != 0)
            vertical = 0;

        if (horizontal > 0 && horizontal < 1 || vertical > 0 && vertical < 1) // If there is an input, ...
        {
            SetAnimations(horizontal, vertical); // Sets direction the player is facing in, based on input.
            StartCoroutine(CoolDown(moveTime)); // Starts cool-down timer.
        }

        else if (horizontal != 0 || vertical != 0) // If there is an input, ...
        {
            Debug.Log(Input.GetButton("Run"));
            if (Input.GetButton("Run"))
                isRunning = true;
            else if (!Input.GetButton("Run") && isRunning && !toggleRunning)
                isRunning = false;

            if (canMove) // If Player is able to move, ...
            {
                SetAnimations(horizontal, vertical); // Sets direction the player is facing in, based on input.

                if (isRunning)
                    moveTime = 0.2f; // Move-time when running.
                else
                    moveTime = 0.3f; // Move-time when walking.

                StartCoroutine(CoolDown(moveTime)); // Starts cool-down timer.
                CheckCollision(horizontal, vertical); // Moves Player if possible.

                SetMoveAnimations(); // Sets animations based on if Player is walking or running.
            }
        }
        else
            SetMoveAnimations(); // Turns all move animations off.
    }
    #endregion

    /// <summary>
    /// Used to permanently toggle running, instead of holding
    /// down the "Shift"-key all the time.
    /// </summary>
    /// <returns></returns>
    private void ToggleRunning()
    {
        isRunning = !isRunning;
        toggleRunning = !toggleRunning;
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
