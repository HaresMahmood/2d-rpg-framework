using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject nameBox;
    public GameObject optionBoxes;

    public GameObject continueIcon;
    public GameObject stopIcon;
    public GameObject optionIcon;

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;

    public float typingSpeed = 0.05f;
    public float speedMultiplier = 0.005f;

    public Animator anim;
    public PlayerMovement player;
    public InteractableObject interactable;

    public bool isActive = false;
    public bool isTyping = false;
    public bool hasEnded = false;

    void Start()
    {
        anim = dialogBox.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
        optionBoxes.SetActive(false); // Make into something like "HideOptions"-function

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
                nameBox.SetActive(false);
                nameText.text = "";

                player.canMove = true;

                //dialogBox.SetActive(false);
            }
        }
        else
        {
            player.canMove = false;
        }

        if (interactable.hasOptions)
        {
            Debug.Log("HAS OPTIONS");
            if (hasEnded && !isTyping)
            {
                Debug.Log("HAS ENDED");

                optionBoxes.SetActive(true);
                option1.SetText(interactable.option1);
                option2.SetText(interactable.option2);

                if (Input.GetAxisRaw("Vertical") == -1 && optionIcon.transform.position != new Vector3(300, 323))
                {
                    optionIcon.transform.position = new Vector3(300, 243);
                }
                else if (Input.GetAxisRaw("Vertical") == 1 && optionIcon.transform.position != new Vector3(300, 243))
                {
                    optionIcon.transform.position = new Vector3(300, 323);
                }

               
            }
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
