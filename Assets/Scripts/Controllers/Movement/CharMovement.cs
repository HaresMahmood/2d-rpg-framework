//TODO Clean up code, document and comment, fix animation bugs.

using UnityEngine;

public class CharMovement: MovingObject
{
    public Collider2D bounds;

    public Vector2 decisionTime = new Vector2(0, 1.5f);
    private float decisionTimeCount;

    private Vector3 direction;
    private readonly bool canMove = !Interact.playerInRange;

    protected override void Start()
    {
        moveTime = 0.3f;

        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
        ChangeDirection();
        base.Start(); // Calls the Start-function of the MovingObject base-class.
    }

    void Update()
    {
        if (isMoving || onCoolDown || onExit) return; // We wait until Character is done moving.
        
        Debug.Log(direction);

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

        if (canMove) // If Player is not in range, ...
        {
            if (bounds.bounds.Contains(transform.position + direction))
            {
                StartCoroutine(CoolDown(moveTime)); // Starts cool-down timer.
                CheckCollision((int)direction.x, (int)direction.y); // Moves Character if possible.
                if (hasObstacle)
                    CheckDecisionTime();
                else
                    CheckDecisionTime();
            }
            else
                CheckDecisionTime();
        }
    }

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
    
    void ChangeDirection()
    {
        int orientation = Random.Range(0, 6);

        //int orientation = 0; // Debug

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
            default:
                break;
        }

    }
}