//TODO: 
//- Change variable names.
//- Look into animations by watching
//Brackey's video.

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI dialogueText;

    public GameObject dBox;

    public GameObject continueIcon;

    public bool isActive;
    public bool isTyping;

    public Animator animator;

    public float waitTime = 0.03f;

    private Queue<string> sentences;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        DisplayIcon();

        /*
        if (!isTyping && !isActive)
        {
            if (animator.gameObject.activeSelf)
            {
                Debug.Log("ACTIVE");
                StartCoroutine(PlayAnimation());
            }
        }
        */
    }

    public void StartDialogue(Dialog dialogue)
    {
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

        //DisplayIcon();

        string sentence = sentences.Dequeue();
        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogText.SetText(sentence);

        dialogText.ForceMeshUpdate();

        int totalVisibleCharacters = dialogText.textInfo.characterCount;
        int counter = 0;

        int visibleCount = 0;

        while (visibleCount < totalVisibleCharacters)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            dialogText.maxVisibleCharacters = visibleCount;

            /*
            if (visibleCount >= totalVisibleCharacters)
            {
                yield break;
            }*/

            counter++;
            yield return new WaitForSeconds(waitTime);

            if (Input.GetButtonDown("Interact") && isTyping)
            {
                visibleCount = totalVisibleCharacters;
                dialogText.maxVisibleCharacters = visibleCount;
                dialogText.ForceMeshUpdate();
                break;
            }
        }

        isTyping = false;
    }

    IEnumerator RevealText(string sentence)
    {
        isTyping = true;

        dialogText.text = ""; //TODO: Turn into function called "ResetText()".

        int charsRevealed = 0;

        while (charsRevealed < sentence.Length)
        {
            while (sentence[charsRevealed] == ' ')
                ++charsRevealed;

            ++charsRevealed;

            dialogText.text = sentence.Substring(0, charsRevealed);

            yield return new WaitForSeconds(waitTime);


            if (Input.GetButtonDown("Interact") && isTyping)
            {
                dialogText.text = "";
                dialogText.text = sentence;

                break;
            }
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        isActive = false;

        if (animator.gameObject.activeSelf)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    void DisplayIcon() //TODO: Rename to "ToggleIcon()".
    {
        if (!isTyping && isActive)
            continueIcon.SetActive(true);
        else
            continueIcon.SetActive(false);
    }

    IEnumerator PlayAnimation()
    {
        animator.SetTrigger("isInactive");
        yield return new WaitForSeconds(0.25f);

        dBox.SetActive(false);
    }

    //TODO: Make into rich-text parsing function.
    private string ParseRichText(string text)
    {
        bool loop = false;

        string ret = "";

        foreach (char x in text.ToCharArray())
        {
            if (x == '>')
            {
                loop = true;

                if (x == '<')
                {
                    loop = false;
                }
            }

            while (loop)
            {
                ret += x;
                continue;
            }

        }

        return ret;
    }

}