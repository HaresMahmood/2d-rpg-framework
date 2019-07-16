using UnityEngine;

/**
 * File-name: PlayerMovement.cs
 * Author: Hares Mahmood
 * Initial implementation: 10/07/2019
 * 
 * Moves Player to tile depending on input. Inherits
 * from MovingObjects.
 */
public class PlayerMovement : MovingObject
{
    private bool isRunning;

    /*
     * Update is called once per frame
     */
    void Update()
    {
        if (isMoving || onCoolDown || onExit) return; // We wait until Player is done moving.

        isRunning = false; // By default, Player is not running.

        // To store the direction in which Player wants to move.
        int horizontal = 0;
        int vertical = 0;

        // To get move directions.
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        // We can't go in both directions at the same time
        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0) // If there is an input, ...
        {
           if ((int)Input.GetAxisRaw("Run") != 0)
                isRunning = true;

            SetAnimations(horizontal, vertical); // Sets direction the player is facing in, based on input.

            if (isRunning)
                moveTime = 0.2f; // Move-time when running.
            else
                moveTime = 0.3f; // Move-time when walking.

            StartCoroutine(CoolDown(moveTime)); // Starts cool-down timer.
            CheckCollision(horizontal, vertical); // Moves Player if possible.

            SetMoveAnimations(); // Sets animations based on if Player is walking or running.
        }
        else
        {
            SetMoveAnimations(); // Turns all move animations off.
        }
    }

    /*
     * Sets the walk and run animations for Player.
     * Overrides SetMoveAnimions function from the MovingObject base-class.
     */
    protected override void SetMoveAnimations()
    {
        if (isRunning) // If Player is running, ...
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
