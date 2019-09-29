using UnityEngine;

public class RangeHandler : MonoBehaviour
{
    [HideInInspector] public bool playerInRange;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    /// </summary>
    /// <param name="other"> The Collider2D attached to the entering object. </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
            playerInRange = true;
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
    /// </summary>
    /// <param name="other"> The Collider2D attached to the entering object. </param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
            playerInRange = false;
    }
}
