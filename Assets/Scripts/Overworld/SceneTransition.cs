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

public class SceneTransition : MonoBehaviour
{
    public string loadScene;
    public Vector2 startPosition;
    public int orientation;

    private Animator anim;
    public Animator transitionAnim;

    [HideInInspector]
    public Vector2 startOrientation;

    void Start()
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

        PlayerMovement player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        anim = player.GetComponent<Animator>();

        SetAnimations((int)startOrientation.x, (int)startOrientation.y);
        player.transform.position = startPosition;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("fade-out");
        yield return new WaitForSeconds(0f);

        SceneManager.LoadScene(loadScene);
    }

    /*
     * 
     */
    protected void SetAnimations(int xDir, int yDir)
    {
        anim.SetFloat("moveX", xDir);
        anim.SetFloat("moveY", yDir);
    }
}
