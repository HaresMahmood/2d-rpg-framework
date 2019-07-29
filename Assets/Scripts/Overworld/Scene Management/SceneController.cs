//TODO come up with better implemenation for int orientation.
//TODO CLEAN UP CODE (animator function should preferably not be here, document and 
// comment, do additional bug-testing.
//TODO Come up with better implementation of SceneTransition, instead of putting
// starting-position for current scene in SceneManager, starting-position should be
// Player's position upon SWITCHING scene.
// Make prefab out of GameObject SceneTransition, with SceneManager, Canvas and
// EventSystem.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Interact
{
    public string scene;
    public Vector2 startPosition;
    public int orientation;
    private Vector2 startOrientation;

    public GameObject bubble;

    private Animator anim;
    public Animator transitionAnim;

    public new bool playerInRange;

    private PlayerMovement player;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        anim = player.GetComponent<Animator>();

        // Sets player's position and orientation upon loading scene. (Move to seperate class and attach to empty GameObject?)
        SetOrientation(orientation);

        SetAnimations((int)startOrientation.x, (int)startOrientation.y);
        player.transform.position = startPosition;

        bubble.SetActive(false);
    }

    void Update()
    {
        if (playerInRange)
        {
            bubble.SetActive(true);
            if (Input.GetButtonDown("Interact"))
            {
                bubble.SetActive(false);
                StartCoroutine(LoadScene());
            }
        }
        else
        {
            bubble.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            //Debug.Log("In range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            //Debug.Log("Not in range");
        }
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("fade-out");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);

    }

    private void SetOrientation(int orientation)
    {
        switch (orientation)
        {
            case 0:
                startOrientation = Vector2.down;  // Down
                break;
            case 1:
                startOrientation = Vector2.left; // Left
                break;
            case 2:
                startOrientation = Vector2.right; // Right
                break;
            case 3:
                startOrientation = Vector2.up; // Up
                break;
            default:
                break;
        }
    }

    /*
     * 
     */
    private void SetAnimations(int xDir, int yDir)
    {
        anim.SetFloat("moveX", xDir);
        anim.SetFloat("moveY", yDir);
    } 

}
