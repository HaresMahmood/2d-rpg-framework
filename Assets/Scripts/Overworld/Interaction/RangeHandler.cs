using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHandler : MonoBehaviour
{
    [HideInInspector] public static bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
        {
            Debug.Log("In range");
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
            Debug.Log("Out of range");
        playerInRange = false;
    }
}
