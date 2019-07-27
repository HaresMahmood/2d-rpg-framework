using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject continueIcon;
    public GameObject stopIcon;

    public TextMeshProUGUI dialogText;

    public float typingSpeed = 0.05f;
    public float speedMultiplier = 0.005f;

    public Animator anim;
    public PlayerMovement player;

    public bool isActive = false;
    public bool isTyping = false;
    public bool hasEnded = false;

    void Start()
    {
        anim = dialogBox.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        ResetText();
        HideIcons();
    }

    void Update()
    {
        if (!isTyping && !isActive)
        {
            if (anim.gameObject.activeSelf)
            {
                //Debug.Log("TRUE");

                StartCoroutine(PlayAnimation());

                player.canMove = true;

                //dialogBox.SetActive(false);
            }
        }
        else
        {
            player.canMove = false;
        }
    }

    public IEnumerator StartDialog(string[] sentences)
    {
        int dialogLength = sentences.Length;
        int currentSentence = 0;

        while (currentSentence < dialogLength || !isTyping)
        {
            if (!isTyping)
            {
                isTyping = true;
                StartCoroutine(TypeSentence(sentences[currentSentence++]));

                if (currentSentence >= dialogLength)
                {
                    hasEnded = true;
                }
            }

            yield return 0;
        }

        while (true)
        {
            if (Input.GetButtonDown("Interact"))
                break;

            yield return 0;
        }

        HideIcons();

        hasEnded = false;
        isActive = false;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        int sentenceLength = sentence.Length;
        int currentChar = 0;

        HideIcons();

        ResetText();

        while (currentChar < sentenceLength)
        {
            dialogText.text += sentence[currentChar];
            currentChar++;

            if (currentChar < sentenceLength)
            {
                if (Input.GetButton("Interact"))
                    yield return new WaitForSeconds(typingSpeed * speedMultiplier);
                else
                    yield return new WaitForSeconds(typingSpeed);
            }
            else
                break;
        }

        ShowIcon();

        while (true)
        {
            if (Input.GetButtonDown("Interact"))
                break;

            yield return 0;
        }

        HideIcons();
        isTyping = false;
        ResetText();
    }

    public IEnumerator PlayAnimation()
    {
        anim.SetBool("isOpen", true);
        yield return new WaitForSeconds(0.1f);
    }

    private void ResetText()
    {
        dialogText.text = "";
    }

    private void HideIcons()
    {
        continueIcon.SetActive(false);
        stopIcon.SetActive(false);
    }

    private void ShowIcon()
    {
        if (hasEnded)
        {
            stopIcon.SetActive(true);
            return;
        }

        continueIcon.SetActive(true);
    }
    
    /*public void SetAnimations()
    {
        if (isActive)
            anim.SetBool("isOpen", true);
            
    }*/
}
