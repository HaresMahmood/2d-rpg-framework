using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;

    // Update is called once per frame
    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);
        }
    }
}