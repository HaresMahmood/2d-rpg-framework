using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour
{
    public GameObject targetObj;
    private Vector3 targetPos;
    public float moveSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPos = new Vector3(targetObj.transform.position.x, targetObj.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}