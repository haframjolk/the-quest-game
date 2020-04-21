using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSentenceManager : MonoBehaviour
{
    public Dialogue[] dialogueArray;
    private DialogueManager dialogueManager;
    private int currentId = 0;

    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }

    public void TriggerDialogue(int id)
    {
        dialogueManager.StartDialogue(dialogueArray[id]);
    }

    // Sýna næsta dialogue-box (hækka id um 1)
    public void TriggerNextDialogue()
    {
        if (dialogueArray.Length > currentId)
        {
            TriggerDialogue(currentId++);
        }
    }
}
