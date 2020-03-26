using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    //public AudioSource blib; 

    public Animator animator;
    public TimelineController timelineController;

    private Queue<string> sentences;
    
    void Start()
    {
        sentences = new Queue<string>();
    }

    void FixedUpdate()
    {
        // Birtir næstu setningu ef playerinn ýtir á Interact (sjálfgefið Z)
        if (Input.GetButtonDown("Interact"))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Pása tímalínu ef hún er tengd
        if (timelineController)
        {
            timelineController.PauseTimeline();
        }
        
        // Birtir dialogue box á skjáinn
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();
        // Setur allar setningar í queue
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
        // Tekur setningu úr queue
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        
    }
    // Birtir textann í dialogue boxið
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //blib.Play();
            yield return null;
        }
    }

    public void EndDialogue()
    {
        // Dialogue boxið fer af skjánum
        animator.SetBool("IsOpen", false);
        // Ef tímalína er tengd við boxið, halda áfram að spila hana
        if (timelineController)
        {
            timelineController.PlayTimeline();
        }
    }
    
}
