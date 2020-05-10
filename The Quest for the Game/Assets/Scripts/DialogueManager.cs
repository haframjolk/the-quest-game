using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public TimelineController timelineController;
    public AudioClip[] blibClips;
    public float charInterval = 0.05f;  // Hversu margar sekúndur líða milli þess að hver stafur er birtur (í upphafi)
    private float currentCharInterval;  // Hversu margar sekúndur eiga að líða fyrir núverandi setningu (breytilegt)
    private Queue<string> sentences;
    private int currentAudioClipIndex;
    private AudioSource audioSource;
    private bool sentenceFinished = false;
    private string transparentColorHex;
    private string transparentColorTagOpen;
    private string transparentColorTagClose;
    
    void Start()
    {
        sentences = new Queue<string>();
        audioSource = GetComponent<AudioSource>();

        // Finna lit á textaboxi og gera transparent
        transparentColorHex = ColorUtility.ToHtmlStringRGBA(new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0));
        // Litatög (rich text)
        transparentColorTagOpen = $"<color=#{transparentColorHex}>";
        transparentColorTagClose = "</color>";
    }

    void Update()
    {
        // Ef spilarinn ýtir á Interact (sjálfgefið Z) og dialogue-box er virkt
        if (Input.GetButtonDown("Interact") && animator.GetBool("IsOpen"))
        {
            // Ef setningin er búin, sýna næstu setningu
            if (sentenceFinished)
            {
                DisplayNextSentence();
            }
            // Ef setningin er ekki búin og hún er á venjulegum hraða, skrifa restina tvöfalt hraðar
            else if (currentCharInterval == charInterval)
            {
                currentCharInterval /= 2;
            }
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
        // currentCharInterval núllstillist við upphaf hverrar setningar
        currentCharInterval = charInterval;
        sentenceFinished = false;
        StartCoroutine(TypeSentence(sentence));
    }
    // Birtir textann í dialogue boxið
    IEnumerator TypeSentence(string sentence)
    {
        // Upphaflega er setningin öll ósýnileg (transparent litatagið nær utan um alla setninguna)
        StringBuilder sentenceB = new StringBuilder(transparentColorTagOpen + sentence + transparentColorTagClose);

        // Sýna einn staf í einu
        for (int i = 0; i < sentence.Length; i++)
        {
            // Eyða opnunartagi frá núverandi staðsetningu og bæta því við einum staf aftar
            sentenceB.Remove(i, transparentColorTagOpen.Length);
            sentenceB.Insert(i+1, transparentColorTagOpen);
            // Uppfæra texta
            dialogueText.text = sentenceB.ToString();

            // Ef blibClips eru til staðar, spila hljóð
            if (blibClips.Length > 0)
            {
                audioSource.PlayOneShot(blibClips[currentAudioClipIndex++ % blibClips.Length]);
            }
            yield return new WaitForSeconds(currentCharInterval);

        }
        // Stillist þegar setning er búin
        sentenceFinished = true;
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
