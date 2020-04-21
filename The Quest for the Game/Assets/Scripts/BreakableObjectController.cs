using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectController : InteractableController
{
    public bool isBreakable = true;
    public AudioClip errorClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetBreakableStatus(bool value)
    {
        isBreakable = value;
    }

    public void BreakObject()
    {
        if (isBreakable)
        {
            Destroy(gameObject);
        }
        // Spila hljóð ef ekki er hægt að brjóta hlut
        else if (errorClip && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(errorClip);
        }
    }

    public override void Interact()
    {
        // Debug.Log("Player interacted with breakable object.");
        BreakObject();
    }
}
