using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<string> sentences;
    private Queue<string> names;

    private bool inDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    private void Update()
    {
        if(inDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation");
        inDialogue = true;
        animator.SetBool("IsOpen", true);

        sentences.Clear();
        names.Clear();

        foreach (Textbox textbox in dialogue.textbox)
        {
            sentences.Enqueue(textbox.sentence);
            names.Enqueue(textbox.name);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        
        StopAllCoroutines();
        nameText.text = name;
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        Debug.Log("Done printing sentence");
    }

    void EndDialogue()
    {
        inDialogue = false;
        animator.SetBool("IsOpen", false);
        Debug.Log("Ending conversation");
    }
}
