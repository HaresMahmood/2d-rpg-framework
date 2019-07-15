using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharMovement: MovingObject
{
    private Vector3 direction;

    protected override void Start()
    {
        ChangeDirection();

        //Call the Start function of the MovingObject base class.
        base.Start();
    }

    void Update()
    {
        //We do nothing if the player is still moving.
        if (isMoving || onCooldown || onExit) return;

        Debug.Log(direction);

        if (direction != null)
        {
            moveTime = 0.2f;

            StartCoroutine(actionCooldown(0.2f));
            Move((int)direction.x, (int)direction.y);
        }
    }

    //TODO NPC doesn't go all the way down when moving down.
    void ChangeDirection()
    {
        //int orientation = Random.Range(0, 3

        int orientation = 0; // Debug

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
        }

    }
}