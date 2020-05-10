using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleButtonController : MonoBehaviour
{
    public Button[] buttons;
    public GameObject selector;
    public Canvas canvas;
    public AudioClip switchClip;
    public BattleSystem battleSystem;
    private AudioSource audioSource;
    private int activeButtonId = 0;
    private bool inputEnabled = true;
    private Vector3 selectorPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        selectorPosition = selector.transform.position;
    }

    void Update()
    {
        // Ekki taka við skipunum ef leikurinn er pásaður
        if (Time.timeScale == 0f)
        {
            return;
        }
        // Ef komið er að leikmanni
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            // Kveikja á selector
            selector.SetActive(true);

            // Velja mismunandi takka með lyklaborðinu
            float input = Input.GetAxisRaw("Horizontal");
            if (input == 0f)
            {
                inputEnabled = true;
            }
            else if (inputEnabled)
            {
                inputEnabled = false;
                int direction = System.Math.Sign(input);
                activeButtonId += direction;
                // Passa að ID fari ekki út fyrir leyfileg mörk (0..length-1)
                if (activeButtonId < 0)
                {
                    activeButtonId = buttons.Length + activeButtonId;
                }
                else if (activeButtonId >= buttons.Length)
                {
                    activeButtonId %= buttons.Length;
                }
                audioSource.PlayOneShot(switchClip);
            }
            // Staðsetja selector fyrir neðan takka (færa x-hnit)
            Button activeButton = buttons[activeButtonId];
            selectorPosition.x = activeButton.transform.position.x;
            selector.transform.position = selectorPosition;

            // Ef leikmaður velur attack/heal
            if (Input.GetButtonDown("Interact"))
            {
                activeButton.onClick.Invoke();
                audioSource.PlayOneShot(switchClip);
            }
        }
        // Slökkva á selector ef ekki er komið að leikmanni
        else
        {
            selector.SetActive(false);
        }
    }
}
