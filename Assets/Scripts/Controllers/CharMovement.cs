//TODO Avoid copying over whole Move function, keep Move functionality in MovingObjects as much as possible
//TODO Change Interact.playerInRange to seperate, local bool (canMove)
//TODO Better name for movingTime and movingTimeSecs (see respective comments)
//TODO More elegant way to handle movingTime and waitTime (NOT WORKING! LOOK FOR OTHER WAY TO RANDOMIZE MOVEMENT!)
//TODO Animate NPC

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement: MovingObject
{
    private Vector2 direction;
    public Collider2D bounds;

    public float movingTime; // Come up with better name
    private float movingTimeSecs; // COme up with better name
    public float waitTime;
    private float waitTimeSecs;

    protected override void Start()
    {
        movingTimeSecs = movingTime;
        waitTimeSecs = waitTime;
        ChangeDirection();

        //Call the Start function of the MovingObject base class.
        base.Start();
    }

    void Update()
    {
        //We do nothing if the player is still moving.
        if (isMoving || onCooldown || onExit) return;

        if (!Interact.playerInRange)
        {
            if (direction != null)
            {
                movingTimeSecs -= Time.deltaTime;
                if (movingTimeSecs <= 0)
                {
                    movingTimeSecs = movingTime;
                    isMoving = false;
                    ChooseNewDirection();
                }

                moveTime = 0.3f;

                StartCoroutine(actionCooldown(0.2f));
                Move((int)direction.x, (int)direction.y);
            }
            else
            {
                waitTimeSecs -= Time.deltaTime;
                if (waitTimeSecs <= 0)
                {
                    ChooseNewDirection();
                    isMoving = true;
                    waitTimeSecs = waitTime;
                }
            }
        }
    }


    protected override void Move(int xDir, int yDir)
    {
        Vector2 startCell = transform.position;
        Vector2 targetCell = startCell + new Vector2(xDir, yDir);
        Vector2 direction = new Vector2(xDir, yDir);

        hasGroundTile = false;
        isOnGround = false;
        hasObstacleTile = false;
        hasChar = false;

        foreach (var t in groundTiles)
        {
            if (getCell(t, startCell) != null)
            {
                isOnGround = true; //If the player is on the ground
            }
        }
        foreach (var t in groundTiles)
        {
            if (getCell(t, targetCell) != null)
            {
                hasGroundTile = true; //If target Tile has a ground
            }
        }

        foreach (var t in obstacleTiles)
        {
            if (getCell(t, targetCell) != null)
            {
                hasObstacleTile = true; //if target Tile has an obstacle
            }
        }

        if (CheckCollision(coll, direction, 0.5f))
        {
            hasChar = true;
        }
        
        //If the player starts their movement from a ground tile.
        if (isOnGround)
        {
            //If the front tile is a walkable ground tile, the player moves here.
            if (hasGroundTile && !hasObstacleTile && !hasChar && bounds.bounds.Contains(targetCell))
            {
                StartCoroutine(SmoothMovement(targetCell));
            }
            else
            {
                ChooseNewDirection();
            }
        }
    }

    private void ChooseNewDirection()
    {
        Vector2 currentDirection = direction;
        ChangeDirection();

        int i = 0;
        while (currentDirection == direction && i < 100)
        {
            i++;
            ChangeDirection();
        }
    }
    
    void ChangeDirection()
    {
        int orientation = Random.Range(0, 4);

        //int orientation = 0; // Debug

        switch (orientation)
        {
            case 0:
                // Down
                direction = Vector2.down;
                break;
            case 1:
                // Left
                direction = Vector2.left;
                break;
            case 2:
                // Right
                direction = Vector2.right;
                break;
            case 3:
                // Up
                direction = Vector2.up;
                break;
            default:
                break;
        }

    }
}