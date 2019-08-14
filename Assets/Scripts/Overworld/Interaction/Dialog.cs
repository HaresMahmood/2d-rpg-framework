using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class Dialog
{
    //[UnityEngine.Header("General")]
    //[UnityEngine.Header("Properties)]   TODO: Use to group bools?

    public string charName;

    //[UnityEngine.Header("Dialog")]
    public DialogConfig[] dialog; //TODO: Change name of DialogConfig (DialogProperties/SentenceProperties?)
}
