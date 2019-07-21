using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * File-name: MovingObject.cs
 * Author: Hares Mahmood
 * Initial implementation: 11/07/2019
 * 
 * Sets up TileMap and character GameObject collision, as 
 * well as TileMap, grid-based movement.
 */
public abstract class MovingObject : MonoBehaviour
{
    // Lists of ground- and obstacle-tiles.
    public List<Tilemap> groundTiles = new List<Tilemap>();
    public List<Tilemap> obstacleTiles = new List<Tilemap>();

    // Time it takes for Character to move 1 tile.
    public float moveTime;

    [HideInInspector]
    public bool canMove = true, isMoving = false, onCoolDown = false, onExit = false;
    [HideInInspector]
    public bool onGround, hasGround, hasObstacle, hasChar;

    [HideInInspector]
    public Collider2D coll;
    [HideInInspector]
    public Animator anim;

    /*
     * Start is called before the first frame update
     */
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    /*
     * Checks if Character can move to target-tile.
     * Takes 2 parameters: int xDir and int yDir for direction.
     */ 
    protected virtual void CheckCollision(int xDir, int yDir)
    {
        // Starting position of Character.
        Vector2 startTile = transform.position;
        // Target-tile Character wants to move to.
        Vector2 targetTile = startTile + new Vector2(xDir, yDir);
        Vector2 direction = new Vector2(xDir, yDir);

        hasObstacle = false; // By default, there is no obstacle-tile in front of Character.
        hasChar = false; // By default, there is no character in front of Character.
        hasGround = false;
        onGround = false;

        foreach (var t in groundTiles)
        {
            if (getTile(t, startTile) != null) // If Character is currently standing on a ground-tile, ...
                onGround = true;

            if (getTile(t, targetTile) != null) // If target-tile is a ground-tile, ...
                hasGround = true;
        }

        foreach (var t in obstacleTiles)
        {
            if (getTile(t, targetTile) != null) // If target-tile is an obstacle-tile, ..
                hasObstacle = true; 
        }

        if (getChar(coll, direction, 0.5f))
            hasChar = true; // If a character is standing on the target-tile, ...

        // If Character starts movement from a ground-tile.
        if (onGround)
        {
            // If the front tile is a walkable ground-tile and doesn't already have 
            // a character standing there, Character will move towards said tile.
            if (hasGround && !hasObstacle & !hasChar)
            {
                // Moves Character to target-tile
                StartCoroutine(Move(targetTile));
            }
        }
    }

    /*
     * Moves Character to the center of target-tile.
     * Takes 1 parameter: Vector 2 end for end-position.
     */
    protected IEnumerator Move(Vector3 end)
    {
        isMoving = true; // Character is now moving.
        
        float remainingDistance = (transform.position - end).sqrMagnitude; // Calculates squared magnitude of the remaining distance, since it is much faster.
        float inverseMoveTime = 1 / moveTime; // Calculates the inverse move-time, since it is mathematically "cheaper" to divide than to multiply.

        while (remainingDistance > float.Epsilon) // Compares the remaining distance to a very small value.
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime); // Calculate a position between the points specified to get the new position.

            transform.position = newPosition; // Sets current position to new position.
            remainingDistance = (transform.position - end).sqrMagnitude; // Calculates new remaining distance.

            yield return null;
        }

        isMoving = false; // Once player has reached the new position, it is not moving anymore.
    }

    /*
     * Prevents Character from doing anything by setting cool-down.
     * Takes 1 parameter: float coolDown for duration of cool-down.
     */
    protected IEnumerator CoolDown(float cooldown)
    {
        onCoolDown = true;
        
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        onCoolDown = false;
    }

    /*
     * Sets the walk and run animations for Character.
     */
    protected void SetAnimations(int xDir, int yDir)
    {
        anim.SetFloat("moveX", xDir);
        anim.SetFloat("moveY", yDir);
    }

    /*
     * Sets the correct direction in the Animator for Character.
     * Takes 2 parameters: int xDir and int yDir for direction.
     */
    protected virtual void SetMoveAnimations()
    {
        if (isMoving)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);
    }

    /*
     * Returns true if the position specified has a tile.
     * Takes 2 parameters: Tilemap tilemap to check in a specific Tilemap and
     * Vector 2 pos for position.
     * Returns a boolean (true/false).
     */
    protected bool hasTile(Tilemap tilemap, Vector2 pos)
    {
        return tilemap.HasTile(tilemap.WorldToCell(pos));
    }

    /*
     * Returns the tile at the specified position.
     * Takes 2 parameters: Tilemap tilemap to check in a specific Tilemap and
     * Vector 2 pos for position.
     * Returns a TileBase (tile).
     */
    protected TileBase getTile(Tilemap tilemap, Vector2 pos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(pos));
    }

    /*
     * Returns true if the Collider2D of Character collides with that of
     * another character.
     * Takes 3 parameters: Collider2D moveCollider for Character's collider,
     * Vector2 direction for direction and float distance for distance.
     * Returns a boolean (true/false).
     */ 
    protected bool getChar(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10]; // Array to store all detected colliders withing set distance.
            ContactFilter2D filter = new ContactFilter2D() { }; // Filter not being used at the moment.

            int numHits = moveCollider.Cast(direction, filter, hits, distance); // Casts a RayCast in the direction and for the distance specified.

            for (int i = 0; i < numHits; i++)
            {
                if (!hits[i].collider.isTrigger) // If the particular collider is not a Trigger, ...
                {
                    return true; // Hit blocking collision
                }
            }
        }
        return false;
    }

}