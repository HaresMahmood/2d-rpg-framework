using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public LayerMask layerMask = 1 << 8;

    // Update is called once per frame
    void Update()
    {
        bool hasOrientation = Physics2D.Raycast(transform.position, GetComponent<PlayerMovement>().orientation, 1f, layerMask);

        Debug.DrawRay(transform.position, GetComponent<PlayerMovement>().orientation, Color.red); // Debug
        Debug.Log(transform.position); // Debug

        if (hasOrientation)
            InteractableObject.orientation = true;
        else
            InteractableObject.orientation = false;

    }
}
