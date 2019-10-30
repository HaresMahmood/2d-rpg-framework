using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MovePositioner : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform positioner;
    private Vector3 initialPosition;

    #endregion
    
    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        //this.transform.position = new Vector2(this.transform.position.x, positioner.position.y);
        initialPosition = positioner.position;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        ChangePosition();
    }

    private void ChangePosition()
    {
        if (initialPosition != positioner.position)
        {
            initialPosition = positioner.position;
            this.transform.position = new Vector2(this.transform.position.x, positioner.position.y);
        }
    }

    #endregion
}
