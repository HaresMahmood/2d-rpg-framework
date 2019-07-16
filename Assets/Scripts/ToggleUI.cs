using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    //Declare RectTransform in script
    RectTransform faceButton;
    //The new position of your button
    Vector2 newPos = new Vector2(500, 0);
    //Reference value used for the Smoothdamp method
    private Vector2 buttonVelocity = Vector2.zero;
    //Smooth time
    private float smoothTime = 0.5f;

    void Start()
    {
        //Get the RectTransform component
        faceButton = GetComponent<RectTransform>();
    }

    void Update()
    {
        //Update the localPosition towards the newPos
        faceButton.localPosition = Vector2.SmoothDamp(faceButton.localPosition, newPos, ref buttonVelocity, smoothTime);
    }
}
