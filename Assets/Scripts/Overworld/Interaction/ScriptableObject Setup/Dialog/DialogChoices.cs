﻿using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Choice", menuName = "Dialog/Choice")]
public class DialogChoices : ScriptableObject
{
    [System.Serializable]
    public class Choices
    {
        public string choiceText;
        public Dialog nextDialog;
        public UnityEvent choiceEvent;

    }

    public Choices[] choices;
}
