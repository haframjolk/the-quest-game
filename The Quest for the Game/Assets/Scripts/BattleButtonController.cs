using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleButtonController : MonoBehaviour
{
    public Button[] buttons;
    public Image selector;
    public Canvas canvas;
    public AudioClip switchClip;
    public BattleSystem battleSystem;
    private AudioSource audioSource;
    private int activeButtonId = 0;
    private bool inputEnabled = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Ef komið er að leikmanni
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            // Kveikja á selector
            selector.enabled = true;

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
            // Staðsetja selector fyrir neðan takka
            Button activeButton = buttons[activeButtonId];
                                                                                                                                                    // Þessum parti bætt við af mér, þarf ef notað sem prefab
            selector.rectTransform.position = activeButton.transform.position - activeButton.transform.up * (activeButton.transform.localScale.y / 2f) * canvas.scaleFactor * 56;

            // Ef leikmaður velur attack/heal
            if (Input.GetButtonDown("Submit"))
            {
                activeButton.onClick.Invoke();
                audioSource.PlayOneShot(switchClip);
            }
        }
        // Slökkva á selector ef ekki er komið að leikmanni
        else
        {
            selector.enabled = false;
        }
    }
}
