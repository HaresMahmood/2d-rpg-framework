using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public static LayerMask layerMask = 1 << 8;

    private static Vector2 position;
    private static Vector2 orientation;

    private void Update()
    {
        position = transform.position;
        orientation = GetComponent<PlayerMovement>().orientation;
    }

    public static bool hasOrientation()
    {
        bool hasOrientation = Physics2D.Raycast(position, orientation, 1f, layerMask);

        if (hasOrientation)
            return true;
        else
            return false;
    }
}
