//TODO come up with better implemenation for int orientation.
//TODO Clean up code, document and comment, do additional bug-testing.
//TODO Come up with better implementation of SceneTransition, player's orientation does NOT change.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string loadScene;
    public Vector2 startPosition;
    public int orientation;

    //[HideInInspector]
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
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            SceneManager.LoadScene(loadScene);
        }
    }
}
