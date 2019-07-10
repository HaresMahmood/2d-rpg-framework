using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharMovement : MonoBehaviour
{
    public Tilemap groundTiles;
    public Tilemap obstacleTiles;

    private bool isMoving = false;

    private bool onCooldown = false;
    private bool onExit = false;

    private float moveTime = 0.2f;

    private Animator animator;

    // Start is used for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //We do nothing if the player is still moving.
        if (isMoving || onCooldown || onExit) return;

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
            Debug.Log(horizontal + " " + vertical);
            animator.SetFloat("moveX", horizontal);
            animator.SetFloat("moveY", vertical);
            animator.SetBool("isWalking", true);

            StartCoroutine(actionCooldown(0.2f));
            Move(horizontal, vertical);
        } else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Move(int xDir, int yDir)
    {

        Vector2 startCell = transform.position;
        Vector2 targetCell = startCell + new Vector2(xDir, yDir);

        bool isOnGround = getCell(groundTiles, startCell) != null; //If the player is on the ground
        bool hasGroundTile = getCell(groundTiles, targetCell) != null; //If target Tile has a ground
        bool hasObstacleTile = getCell(obstacleTiles, targetCell) != null; //if target Tile has an obstacle

        //If the player starts their movement from a ground tile.
        if (isOnGround)
        {
            //If the front tile is a walkable ground tile, the player moves here.
            if (hasGroundTile && !hasObstacleTile)
            {
                StartCoroutine(SmoothMovement(targetCell));
            }
        }

        if (!isMoving)
            StartCoroutine(BlockedMovement(targetCell));
    }

    private IEnumerator SmoothMovement(Vector3 end)
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

    //Blocked animation
    private IEnumerator BlockedMovement(Vector3 end)
    {
        isMoving = true;

        Vector3 originalPos = transform.position;

        end = transform.position + ((end - transform.position) / 3);
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = (1 / (moveTime * 2));

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        sqrRemainingDistance = (transform.position - originalPos).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, originalPos, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - originalPos).sqrMagnitude;

            yield return null;
        }
        isMoving = false;
    }

    private IEnumerator actionCooldown(float cooldown)
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

    public Collider2D whatsThere(Vector2 targetPos)
    {
        RaycastHit2D hit;
        hit = Physics2D.Linecast(targetPos, targetPos);
        return hit.collider;
    }

    private TileBase getCell(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(cellWorldPos));
    }
    private bool hasTile(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.HasTile(tilemap.WorldToCell(cellWorldPos));
    }

}