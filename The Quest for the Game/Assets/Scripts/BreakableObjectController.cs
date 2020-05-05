using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectController : InteractableController
{
    public bool isBreakable = true;
    public AudioClip errorClip;
    public AudioClip breakClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetBreakableStatus(bool value)
    {
        isBreakable = value;
    }

    IEnumerator WaitThenDisable()
    {
        float timeToWait = 0f;
        if (breakClip)
        {
            timeToWait = breakClip.length;
        }
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }

    public void BreakObject()
    {
        if (isBreakable)
        {
            if (breakClip && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(breakClip);
            }
            // Slökkva á collider og renderer á meðan hljóðið spilast, áður en slökkt er á hlutnum, svo hann hverfi strax
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            // Eyða honum þegar brothljóðið er búið að spilast
            StartCoroutine(WaitThenDisable());
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
