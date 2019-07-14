using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool storable; // If true, object can be stored in inventory
    public bool openable; // If true, object can be opened
    public bool locked; // If true, object is locked
    public bool talks; // If true, object has dialogue 

    public GameObject itemNeeded; // Item needed in order to interact with this object
    public string dialogue;

    public Animator animator;

    
}
