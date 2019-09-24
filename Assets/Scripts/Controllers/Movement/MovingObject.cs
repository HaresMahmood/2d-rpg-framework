using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Defines collision for Tilemaps and Characters and
/// defines grid-based movement for Characters.
/// </summary>
public abstract class MovingObject : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Lists that keep track of Tilemaps. These can
    /// be dynamically updated depending on the scene Character is in.
    /// </summary>
    public static List<Tilemap> groundTiles = new List<Tilemap>(), obstacleTiles = new List<Tilemap>();

    /// <summary>
    /// Time it takes for Character to move 1 tile.
    /// </summary>
    [HideInInspector] public float moveTime;

    /// <summary>
    /// Determines the state of movement of Character.
    /// </summary>
    [HideInInspector] public bool canMove = true, isMoving = false, onCoolDown = false, onExit = false, onGround, hasGround, hasObstacle, hasChar;

    /// <summary>
    /// Collider2D attached to Character.
    /// </summary>
    [HideInInspector] public Collider2D coll;
    /// <summary>
    /// Animator attached to Character.
    /// </summary>
    [HideInInspector] public Animator anim;

    /// <summary>
    /// Orientation of Character.
    /// </summary>
    [HideInInspector] public Vector2 orientation;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    #endregion

    /// <summary>
    /// Checks if Character can move to target-tile.
    /// </summary>
    /// <param name="xDir"> X-coordinate of the target-direction. </param>
    /// <param name="yDir"> Y-coordinate of the target-direction. </param>
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
            if (GetTile(t, startTile) != null) // If Character is currently standing on a ground-tile, ...
                onGround = true;

            if (GetTile(t, targetTile) != null) // If target-tile is a ground-tile, ...
                hasGround = true;
        }

        foreach (var t in obstacleTiles)
        {
            if (GetTile(t, targetTile) != null) // If target-tile is an obstacle-tile, ..
                hasObstacle = true;
        }

        if (GetCharacter(coll, direction, 1f))
            hasChar = true; // If a character is standing on the target-tile, ...

        // If Character starts movement from a ground-tile.
        if (onGround)
        {
            // If the front tile is a walkable ground-tile and doesn't already have 
            // a character standing there, Character will move towards said tile.
            if (hasGround && !hasObstacle & !hasChar)
                StartCoroutine(Move(targetTile)); // Moves Character to target-tile.
        }
    }

    /// <summary>
    /// Moves Character to the center of target-tile.
    /// </summary>
    /// <param name="end"> End-position of Character. </param>
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

    /// <summary>
    /// Prevents Character from doing anything by setting cool-down timer.
    /// </summary>
    /// <param name="coolDown"> Duration of cool-down. </param>
    protected IEnumerator CoolDown(float duration)
    {
        onCoolDown = true;

        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        onCoolDown = false;
    }

    /// <summary>
    ///  Sets the walk and run animations for Character and
    ///  sets the appriopriate orientation values of Character.
    /// </summary>
    protected void SetAnimations(int xDir, int yDir)
    {
        anim.SetFloat("moveX", xDir);
        anim.SetFloat("moveY", yDir);

        orientation = new Vector2(xDir, yDir);
    }

    /// <summary>
    /// Sets the correct direction in the Animator for Character.
    /// </summary>
    protected virtual void SetMoveAnimations()
    {
        if (isMoving)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);
    }

    /// <summary>
    /// Returns true if the position specified has a Tilemap (tile).
    /// </summary>
    /// <param name="tilemap"> Tilemap that needs to be checked. </param>
    /// <param name="pos"> Position of Tilemap. </param>
    /// <returns> Boolean. </returns>
    protected bool HasTile(Tilemap tilemap, Vector2 pos)
    {
        return tilemap.HasTile(tilemap.WorldToCell(pos));
    }

    /// <summary>
    /// Returns the Tilemap at the specified position.
    /// </summary>
    /// <param name="tilemap"> Tilemap that needs to be checked. </param>
    /// <param name="pos"> Position of Tilemap. </param>
    /// <returns> TileBase. </returns>
    protected TileBase GetTile(Tilemap tilemap, Vector2 pos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(pos));
    }

    /// <summary>
    /// Returns true if the Collider2D of Character collides with that of 
    /// another character.
    /// </summary>
    /// <param name="moveCollider"> Collider of the character that is being checked. </param>
    /// <param name="direction"> Direction Character is facing towards. </param>
    /// <param name="distance"> Maximum distance the character that is being checked can be located. </param>
    /// <returns> Boolean. </returns>
    public static bool GetCharacter(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10]; // Array to store all detected colliders withing set distance.
            ContactFilter2D filter = new ContactFilter2D() { }; // Filter not being used at the moment.

            int numHits = moveCollider.Cast(direction, filter, hits, distance); // Casts a RayCast in the direction and for the distance specified.

            for (int i = 0; i < numHits; i++)
            {
                if (!hits[i].collider.isTrigger) // If the particular collider is not a Trigger, ...
                    return true; // Hit blocking collision
            }
        }

        return false;
    }
}