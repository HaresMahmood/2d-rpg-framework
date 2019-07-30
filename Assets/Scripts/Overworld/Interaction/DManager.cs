//TODO: 
//- Change variable names.
//- Look into animations by watching
//Brackey's video.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DManager : MonoBehaviour //TODO: Change class name to "DialogManager"
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject dBox;

    public bool isActive;
    public bool isTyping;

    public Animator animator;

    public float waitTime = 0.02f;

    private Queue<string> sentences;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialog dialogue)
    {
        //animator.SetBool("IsOpen", true);

        dBox.SetActive(true);
        isActive = true;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogueText.text = ""; //TODO: Turn into function called "ResetText()".

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(waitTime);

            if (Input.GetButtonDown("Interact") && isTyping)
            {
                dialogueText.text = "";
                dialogueText.text = sentence;

                break;
            }
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        //animator.SetBool("IsOpen", false);
        dBox.SetActive(false);
        isActive = false;
    }

}