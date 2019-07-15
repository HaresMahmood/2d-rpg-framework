//TODO Move Running functionality from MovingObject to PlayerMovement
//TODO Move all animations to seperate function, and all walk animations to MovingObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject
{
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        //Call the Start function of the MovingObject base class.
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //We do nothing if the player is still moving.
        if (isMoving || onCooldown || onExit) return;

        isRunning = false; // Since Player is standing still, isRunning is false by default

        //To store move directions.
        int horizontal = 0;
        int vertical = 0;

        //To get move directions
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //We can't go in both directions at the same time
        if (horizontal != 0)
            vertical = 0;

        //If there's a direction, we are trying to move.
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetFloat("moveX", horizontal);
            animator.SetFloat("moveY", vertical);

            if (isRunning = (int)Input.GetAxisRaw("Run") != 0)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            if (isRunning)
            {
                moveTime = 0.2f;

                animator.SetBool("isRunning", isRunning);
                animator.SetBool("isWalking", false);

                StartCoroutine(actionCooldown(0.125f)); // Action cooldown and Move Time must be same value to avoid blocky movement
                Move(horizontal, vertical);
            }
            else
            {
                moveTime = 0.3f;

                animator.SetBool("isRunning", isRunning);
                animator.SetBool("isWalking", true);

                StartCoroutine(actionCooldown(0.2f));
                Move(horizontal, vertical);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", isRunning);
        }
    }
}
