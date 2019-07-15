using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MovingObject : MonoBehaviour
{
    public List<Tilemap> groundTiles = new List<Tilemap>();
    public List<Tilemap> obstacleTiles = new List<Tilemap>();


    public bool isMoving = false;

    public bool onCooldown = false;
    public bool onExit = false;

    public float moveTime;

    private Collider2D coll;

    public bool isOnGround;
    public bool hasGroundTile;
    public bool hasObstacleTile;
    public bool hasChar;

    public bool isRunning;

    // Start is used for initialization
    protected virtual void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    protected void Move(int xDir, int yDir)
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
            if (hasGroundTile && !hasObstacleTile & !hasChar)
            {
                StartCoroutine(SmoothMovement(targetCell));
            }
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //while (isMoving) yield return null;

        isMoving = true;


        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
    }

    protected IEnumerator actionCooldown(float cooldown)
    {
        onCooldown = true;

        //float cooldown = 0.2f;
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        onCooldown = false;
    }

    protected TileBase getCell(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(cellWorldPos));
    }

    protected bool hasTile(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.HasTile(tilemap.WorldToCell(cellWorldPos));
    }

    protected bool CheckCollision(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D filter = new ContactFilter2D() { };

            int numHits = moveCollider.Cast(direction, filter, hits, distance);

            for (int i = 0; i < numHits; i++)
            {
                if (!hits[i].collider.isTrigger)
                {
                    // Hit blocking collision
                    return true;
                }
            }
        }

        return false;
    }

}