using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public TimelineController timelineController;
    public AudioClip[] blibClips;
    private Queue<string> sentences;
    private int currentAudioClipIndex;
    private AudioSource audioSource;
    
    void Start()
    {
        sentences = new Queue<string>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
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
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            // Ef blibClips eru til staðar, spila hljóð
            if (blibClips.Length > 0)
            {
                audioSource.PlayOneShot(blibClips[currentAudioClipIndex++ % blibClips.Length]);
            }
            // TODO: Experiment with wait times, disable interact button while still writing?
            yield return new WaitForSeconds(0.04f);
            // yield return null;
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
